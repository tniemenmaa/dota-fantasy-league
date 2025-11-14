using System.Text.Json.Serialization;

namespace DotaFantasyLeague.Api.Models;

/// <summary>
/// Represents a match returned from the OpenDota league matches endpoint.
/// </summary>
public record LeagueMatch
{
    /// <summary>
    /// The Valve-assigned identifier for the match.
    /// </summary>
    [JsonPropertyName("match_id")]
    public long MatchId { get; init; }

    /// <summary>
    /// Indicates whether the tracked team played as Radiant.
    /// </summary>
    [JsonPropertyName("radiant")]
    public bool Radiant { get; init; }

    /// <summary>
    /// Indicates whether Radiant won the match.
    /// </summary>
    [JsonPropertyName("radiant_win")]
    public bool? RadiantWin { get; init; }

    /// <summary>
    /// The number of kills for the Radiant team.
    /// </summary>
    [JsonPropertyName("radiant_score")]
    public int RadiantScore { get; init; }

    /// <summary>
    /// The number of kills for the Dire team.
    /// </summary>
    [JsonPropertyName("dire_score")]
    public int DireScore { get; init; }

    /// <summary>
    /// Duration of the match in seconds.
    /// </summary>
    [JsonPropertyName("duration")]
    public int Duration { get; init; }

    /// <summary>
    /// Unix timestamp for when the match started.
    /// </summary>
    [JsonPropertyName("start_time")]
    public int StartTime { get; init; }

    /// <summary>
    /// Identifier for the league the match belongs to.
    /// </summary>
    [JsonPropertyName("leagueid")]
    public int LeagueId { get; init; }

    /// <summary>
    /// Name of the league the match belongs to.
    /// </summary>
    [JsonPropertyName("league_name")]
    public string? LeagueName { get; init; }

    /// <summary>
    /// Identifier of the cluster (server) the match was played on.
    /// </summary>
    [JsonPropertyName("cluster")]
    public int Cluster { get; init; }

    /// <summary>
    /// Identifier for the opposing team.
    /// </summary>
    [JsonPropertyName("opposing_team_id")]
    public int? OpposingTeamId { get; init; }

    /// <summary>
    /// Name of the opposing team.
    /// </summary>
    [JsonPropertyName("opposing_team_name")]
    public string? OpposingTeamName { get; init; }

    /// <summary>
    /// URL for the opposing team's logo image.
    /// </summary>
    [JsonPropertyName("opposing_team_logo")]
    public string? OpposingTeamLogo { get; init; }
}
