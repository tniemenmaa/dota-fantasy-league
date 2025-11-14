using System.Net.Http.Headers;
using System.Text.Json;
using DotaFantasyLeague.Api.Models;
using Microsoft.Extensions.Logging;

namespace DotaFantasyLeague.Api.Services;

/// <summary>
/// Implementation of <see cref="IOpenDotaMatchesService"/> that communicates with the OpenDota API.
/// </summary>
public class OpenDotaMatchesService : IOpenDotaMatchesService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenDotaMatchesService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenDotaMatchesService"/> class.
    /// </summary>
    public OpenDotaMatchesService(HttpClient httpClient, ILogger<OpenDotaMatchesService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<MatchDetails> GetMatchAsync(long matchId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/matches/{matchId}";

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = $"Failed to retrieve match details for match {matchId}. Status code: {response.StatusCode}";
                _logger.LogError(error);
                throw new HttpRequestException(error);
            }

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
}
