using System.Net.Http.Headers;
using System.Text.Json;
using DotaFantasyLeague.Api.Models;
using Microsoft.Extensions.Logging;

namespace DotaFantasyLeague.Api.Services;

/// <summary>
/// Provides access to OpenDota resources via HTTP requests.
/// </summary>
public class OpenDotaService : IOpenDotaService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenDotaService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenDotaService"/> class.
    /// </summary>
    public OpenDotaService(HttpClient httpClient, ILogger<OpenDotaService> logger)
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
            using var request = CreateJsonRequest(requestUri);
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            await EnsureSuccessStatusCode(response, $"matches for league {leagueId}");

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
            using var request = CreateJsonRequest(requestUri);
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            await EnsureSuccessStatusCode(response, $"match IDs for league {leagueId}");

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

    /// <inheritdoc />
    public async Task<MatchDetails> GetMatchAsync(long matchId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/matches/{matchId}";

        try
        {
            using var request = CreateJsonRequest(requestUri);
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            await EnsureSuccessStatusCode(response, $"match details for match {matchId}");

            await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var match = await JsonSerializer.DeserializeAsync<MatchDetails>(contentStream, SerializerOptions, cancellationToken);

            if (match is null)
            {
                var error = $"Received empty match details response for match {matchId}.";
                _logger.LogError(error);
                throw new JsonException(error);
            }

            return match;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Fetching match details for match {MatchId} was canceled.", matchId);
            throw;
        }
        catch (JsonException jsonException)
        {
            _logger.LogError(jsonException, "Failed to deserialize match details for match {MatchId}.", matchId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TeamPlayer>> GetPlayersAsync(long teamId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/teams/{teamId}/players";

        try
        {
            using var request = CreateJsonRequest(requestUri);
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            await EnsureSuccessStatusCode(response, $"players for team {teamId}");

            await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var players = await JsonSerializer.DeserializeAsync<List<TeamPlayer>>(contentStream, SerializerOptions, cancellationToken);

            return players ?? [];
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Fetching players for team {TeamId} was canceled.", teamId);
            throw;
        }
        catch (JsonException jsonException)
        {
            _logger.LogError(jsonException, "Failed to deserialize players for team {TeamId}.", teamId);
            throw;
        }
    }

    private static HttpRequestMessage CreateJsonRequest(string requestUri)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return request;
    }

    private async Task EnsureSuccessStatusCode(HttpResponseMessage response, string resourceDescription)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var error = $"Failed to retrieve {resourceDescription}. Status code: {response.StatusCode}. Response: {responseContent}";
        _logger.LogError(error);
        throw new HttpRequestException(error);
    }
}
