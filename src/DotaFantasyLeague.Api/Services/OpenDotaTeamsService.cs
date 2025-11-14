using System.Net.Http.Headers;
using System.Text.Json;
using DotaFantasyLeague.Api.Models;
using Microsoft.Extensions.Logging;

namespace DotaFantasyLeague.Api.Services;

/// <summary>
/// Implementation of <see cref="IOpenDotaTeamsService"/> that communicates with the OpenDota API.
/// </summary>
public class OpenDotaTeamsService : IOpenDotaTeamsService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenDotaTeamsService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenDotaTeamsService"/> class.
    /// </summary>
    public OpenDotaTeamsService(HttpClient httpClient, ILogger<OpenDotaTeamsService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TeamPlayer>> GetPlayersAsync(long teamId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/teams/{teamId}/players";

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = $"Failed to retrieve players for team {teamId}. Status code: {response.StatusCode}";
                _logger.LogError(error);
                throw new HttpRequestException(error);
            }

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
}
