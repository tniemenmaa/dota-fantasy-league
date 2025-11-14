using System.Text.Json.Serialization;

namespace DotaFantasyLeague.Api.Models;

/// <summary>
/// Represents a player who has played for a professional team according to the OpenDota API.
/// </summary>
public record TeamPlayer
{
    /// <summary>
    /// The Steam32 account identifier for the player.
    /// </summary>
    [JsonPropertyName("account_id")]
    public int AccountId { get; init; }

    /// <summary>
    /// The verified or professional name of the player, if available.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// The total number of games the player has played for the team.
    /// </summary>
    [JsonPropertyName("games_played")]
    public int GamesPlayed { get; init; }

    /// <summary>
    /// The total number of wins recorded while the player was on the team.
    /// </summary>
    [JsonPropertyName("wins")]
    public int Wins { get; init; }

    /// <summary>
    /// Indicates whether the player is currently on the team's roster.
    /// </summary>
    [JsonPropertyName("is_current_team_member")]
    public bool IsCurrentTeamMember { get; init; }
}
