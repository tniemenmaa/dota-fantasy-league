namespace DotaFantasyLeague.Api.Models;

/// <summary>
/// Represents a subset of the information returned by the OpenDota match endpoint.
/// </summary>
public class MatchDetails
{
    /// <summary>
    /// Gets or sets the identifier of the match.
    /// </summary>
    public long MatchId { get; set; }

    /// <summary>
    /// Gets or sets the duration of the match in seconds.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the Unix timestamp describing when the match started.
    /// </summary>
    public int StartTime { get; set; }

    /// <summary>
    /// Gets or sets the number of kills made by the Radiant team.
    /// </summary>
    public int RadiantScore { get; set; }

    /// <summary>
    /// Gets or sets the number of kills made by the Dire team.
    /// </summary>
    public int DireScore { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether Radiant won the match.
    /// </summary>
    public bool? RadiantWin { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the Radiant team, if available.
    /// </summary>
    public long? RadiantTeamId { get; set; }

    /// <summary>
    /// Gets or sets the name of the Radiant team, if available.
    /// </summary>
    public string? RadiantName { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the Dire team, if available.
    /// </summary>
    public long? DireTeamId { get; set; }

    /// <summary>
    /// Gets or sets the name of the Dire team, if available.
    /// </summary>
    public string? DireName { get; set; }

    /// <summary>
    /// Gets or sets the league identifier the match is associated with.
    /// </summary>
    public long? LeagueId { get; set; }

    /// <summary>
    /// Gets or sets the league name the match is associated with.
    /// </summary>
    public string? LeagueName { get; set; }
}
