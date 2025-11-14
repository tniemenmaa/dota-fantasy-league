using System.Net.Http.Headers;
using System.Text.Json;
using DotaFantasyLeague.Api.Models;
using Microsoft.Extensions.Logging;

namespace DotaFantasyLeague.Api.Services;

/// <summary>
/// Implementation of <see cref="IOpenDotaLeagueService"/> that communicates with the OpenDota API.
/// </summary>
public class OpenDotaLeagueService : IOpenDotaLeagueService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenDotaLeagueService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenDotaLeagueService"/> class.
    /// </summary>
    public OpenDotaLeagueService(HttpClient httpClient, ILogger<OpenDotaLeagueService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<LeagueMatch>> GetMatchesAsync(long leagueId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/leagues/{leagueId}/matches";

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = $"Failed to retrieve matches for league {leagueId}. Status code: {response.StatusCode}";
                _logger.LogError(error);
                throw new HttpRequestException(error);
            }

            await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var matches = await JsonSerializer.DeserializeAsync<List<LeagueMatch>>(contentStream, SerializerOptions, cancellationToken);

            return matches ?? [];
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Fetching matches for league {LeagueId} was canceled.", leagueId);
            throw;
        }
        catch (JsonException jsonException)
        {
            _logger.LogError(jsonException, "Failed to deserialize matches for league {LeagueId}.", leagueId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<string>> GetMatchIdsAsync(long leagueId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/leagues/{leagueId}/matchIds";

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = $"Failed to retrieve match IDs for league {leagueId}. Status code: {response.StatusCode}";
                _logger.LogError(error);
                throw new HttpRequestException(error);
            }

            await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var matchIds = await JsonSerializer.DeserializeAsync<List<string>>(contentStream, SerializerOptions, cancellationToken);

            return matchIds ?? [];
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Fetching match IDs for league {LeagueId} was canceled.", leagueId);
            throw;
        }
        catch (JsonException jsonException)
        {
            _logger.LogError(jsonException, "Failed to deserialize match IDs for league {LeagueId}.", leagueId);
            throw;
        }
    }
}
