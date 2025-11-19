using System.Linq;
using System.Security.Claims;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace DotaFantasyLeague.Api.Controllers;

[ApiController]
[Route("auth")]
[IgnoreAntiforgeryToken]
public class AuthController : ControllerBase
{
    private const string SteamOpenIdEndpoint = "https://steamcommunity.com/openid/login";
    private const string SteamIssuer = "Steam";
    private const string StateProtectorPurpose = "SteamAuthenticationState";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IDataProtector _stateProtector;
    private readonly string? _steamApiKey;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IHttpClientFactory httpClientFactory, IDataProtectionProvider dataProtectionProvider, IConfiguration configuration, ILogger<AuthController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _stateProtector = dataProtectionProvider.CreateProtector(StateProtectorPurpose);
        _steamApiKey = configuration["Authentication:Steam:ApiKey"];
        _logger = logger;
    }

    [HttpGet("signin/steam")]
    public IActionResult SignInWithSteam([FromQuery] string? returnUrl = null)
    {
        var redirectTarget = NormalizeReturnUrl(returnUrl);
        var protectedState = _stateProtector.Protect(redirectTarget);
        var callbackUrl = BuildCallbackUrl(protectedState);
        var realm = BuildRealm();

        var parameters = new Dictionary<string, string?>
        {
            ["openid.ns"] = "http://specs.openid.net/auth/2.0",
            ["openid.mode"] = "checkid_setup",
            ["openid.return_to"] = callbackUrl,
            ["openid.realm"] = realm,
            ["openid.identity"] = "http://specs.openid.net/auth/2.0/identifier_select",
            ["openid.claimed_id"] = "http://specs.openid.net/auth/2.0/identifier_select"
        };

        var authenticationUrl = QueryHelpers.AddQueryString(SteamOpenIdEndpoint, parameters!);

        return Redirect(authenticationUrl);
    }

    [HttpGet("signin/steam/callback")]
    public async Task<IActionResult> HandleSteamCallback(CancellationToken cancellationToken)
    {
        if (!Request.Query.TryGetValue("state", out var stateValues))
        {
            return BadRequest("Missing state parameter.");
        }

        string? redirectTarget;
        try
        {
            redirectTarget = _stateProtector.Unprotect(stateValues);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Unable to unprotect Steam authentication state.");
            return BadRequest("Invalid state parameter.");
        }

        if (!await ValidateSteamResponseAsync(Request.Query, cancellationToken))
        {
            return Unauthorized("Steam login could not be validated.");
        }

        var steamId = ExtractSteamId(Request.Query["openid.claimed_id"].ToString());
        if (string.IsNullOrWhiteSpace(steamId))
        {
            return Unauthorized("Missing Steam identifier.");
        }

        var personaName = await ResolvePersonaNameAsync(steamId, cancellationToken);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, steamId, ClaimValueTypes.String, SteamIssuer),
            new(ClaimTypes.Name, personaName ?? steamId, ClaimValueTypes.String, SteamIssuer)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return LocalRedirect(redirectTarget ?? "/");
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            UserName = User.Identity?.Name,
            SteamId = User.FindFirstValue(ClaimTypes.NameIdentifier)
        });
    }

    [HttpGet("signout")]
    public async Task<IActionResult> SignOutCurrentUser([FromQuery] string? returnUrl = null)
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var redirectTarget = string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl;

        return LocalRedirect(redirectTarget);
    }

    private string BuildCallbackUrl(string state)
    {
        var callbackUrl = Url.Action(nameof(HandleSteamCallback), "Auth", values: null, Request.Scheme, Request.Host.ToString()) ?? string.Empty;
        return QueryHelpers.AddQueryString(callbackUrl, "state", state);
    }

    private string BuildRealm()
    {
        var baseUri = $"{Request.Scheme}://{Request.Host}";
        return baseUri.EndsWith('/') ? baseUri : baseUri + "/";
    }

    private static string NormalizeReturnUrl(string? returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && (returnUrl.StartsWith('/') || returnUrl.StartsWith("~/")))
        {
            return returnUrl;
        }

        return "/";
    }

    private async Task<bool> ValidateSteamResponseAsync(IQueryCollection query, CancellationToken cancellationToken)
    {
        if (!string.Equals(query["openid.mode"], "id_res", StringComparison.Ordinal))
        {
            return false;
        }

        var formValues = new Dictionary<string, string?>();
        foreach (var pair in query.Where(kvp => kvp.Key.StartsWith("openid.", StringComparison.Ordinal)))
        {
            formValues[pair.Key] = pair.Value.ToString();
        }

        formValues["openid.mode"] = "check_authentication";

        var client = _httpClientFactory.CreateClient("SteamOpenId");
        using var response = await client.PostAsync("/openid/login", new FormUrlEncodedContent(formValues!), cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Steam OpenID verification failed with status code {StatusCode}", response.StatusCode);
            return false;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        return body.Contains("is_valid:true", StringComparison.OrdinalIgnoreCase);
    }

    private async Task<string?> ResolvePersonaNameAsync(string steamId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_steamApiKey))
        {
            return null;
        }

        var client = _httpClientFactory.CreateClient("SteamWebApi");
        var requestUri = $"/ISteamUser/GetPlayerSummaries/v0002/?key={Uri.EscapeDataString(_steamApiKey!)}&steamids={Uri.EscapeDataString(steamId)}";

        try
        {
            using var response = await client.GetAsync(requestUri, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var summary = await JsonSerializer.DeserializeAsync<PlayerSummariesResponse>(stream, cancellationToken: cancellationToken);
            return summary?.Response?.Players?.FirstOrDefault()?.PersonaName;
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Failed to resolve Steam persona name for {SteamId}", steamId);
            return null;
        }
    }

    private static string? ExtractSteamId(string claimedIdentifier)
    {
        if (Uri.TryCreate(claimedIdentifier, UriKind.Absolute, out var uri) && uri.Segments.Length > 0)
        {
            var lastSegment = uri.Segments[^1].Trim('/');
            return lastSegment;
        }

        return null;
    }

    private sealed record PlayerSummariesResponse(
        [property: JsonPropertyName("response")] PlayerSummariesResponseBody? Response);

    private sealed record PlayerSummariesResponseBody(
        [property: JsonPropertyName("players")] IReadOnlyList<PlayerSummary>? Players);

    private sealed record PlayerSummary(
        [property: JsonPropertyName("steamid")] string SteamId,
        [property: JsonPropertyName("personaname")] string PersonaName);
}
