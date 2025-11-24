using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using DotaFantasyLeague.Api.Models;
using Microsoft.Extensions.Logging;

namespace DotaFantasyLeague.Api.Services;

/// <summary>
/// Provides strongly-typed access to Stratz GraphQL operations.
/// </summary>
public class StratzGraphQlService : IStratzGraphQlService
{
    private const string TeamMembersQuery = """
        query GetTeam($teamId: Int!)
        {
          team(teamId: $teamId) {
            id
            name
            leagues {
              id
              displayName
            }
            members {
              player {
                steamAccount {
                  name
                  id
                }
              }
            }
          }
        }
        """;

    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly ILogger<StratzGraphQlService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="StratzGraphQlService"/> class.
    /// </summary>
    public StratzGraphQlService(HttpClient httpClient, ILogger<StratzGraphQlService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<TeamDetails?> GetTeamAsync(long teamId, CancellationToken cancellationToken = default)
    {
        var request = new GraphQlRequest
        {
            Query = TeamMembersQuery,
            Variables = new Dictionary<string, object?>
            {
                ["teamId"] = teamId
            }
        };

        using var response = await _httpClient.PostAsJsonAsync(string.Empty, request, SerializerOptions, cancellationToken);

        await EnsureSuccessStatusCodeAsync(response, teamId);

        await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var graphQlResponse = await JsonSerializer.DeserializeAsync<GraphQlResponse<TeamResponse>>(contentStream, SerializerOptions, cancellationToken);

        if (graphQlResponse?.Errors is { Length: > 0 } errors)
        {
            var errorMessages = string.Join(", ", errors.Select(error => error.Message).Where(message => !string.IsNullOrWhiteSpace(message)));
            _logger.LogError("GraphQL errors occurred while fetching team {TeamId}: {Errors}", teamId, errorMessages);
            throw new HttpRequestException($"Stratz GraphQL returned errors: {errorMessages}");
        }

        return graphQlResponse?.Data?.Team?.ToModel();
    }

    private async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response, long teamId)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var error = $"Failed to retrieve team {teamId} via Stratz GraphQL. Status code: {response.StatusCode}. Response: {responseContent}";
        _logger.LogError(error);
        throw new HttpRequestException(error);
    }

    private sealed record GraphQlRequest
    {
        public string Query { get; init; } = string.Empty;

        public Dictionary<string, object?> Variables { get; init; } = new();
    }

    private sealed record GraphQlResponse<T>
    {
        public T? Data { get; init; }

        public GraphQlError[]? Errors { get; init; }
    }

    private sealed record GraphQlError
    {
        public string? Message { get; init; }
    }

    private sealed record TeamResponse
    {
        public TeamResult? Team { get; init; }
    }

    private sealed record TeamResult
    {
        public long Id { get; init; }

        public string? Name { get; init; }

        public string? Tag { get; init; }

        public IReadOnlyList<TeamMemberResult>? Members { get; init; }

        public TeamDetails ToModel()
        {
            return new TeamDetails
            {
                Id = Id,
                Name = Name,
                Members = (Members ?? Array.Empty<TeamMemberResult>())
                    .Select(member => member.ToModel())
                    .ToList()
            };
        }
    }

    private sealed record TeamMemberResult
    {
        public required TeamPlayerResult Player { get; init; }
     
        public TeamMember ToModel()
        {
            return new TeamMember
            {
                PlayerId = Player.SteamAccount.Id,
                Name = Player.SteamAccount.Name
            };
        }
    }

    private sealed record TeamPlayerResult
    {
        public required SteamAccountResult SteamAccount { get; init; }
        public required int MatchCount { get; init; }
    }

    public sealed record SteamAccountResult
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
