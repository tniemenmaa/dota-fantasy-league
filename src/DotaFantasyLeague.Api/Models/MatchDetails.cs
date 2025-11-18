using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotaFantasyLeague.Api.Models;

/// <summary>
/// Represents the data returned by the OpenDota match endpoint.
/// </summary>
public class MatchDetails
{
    /// <summary>
    /// Gets or sets the identifier of the match.
    /// </summary>
    [JsonPropertyName("match_id")]
    public long MatchId { get; set; }

    /// <summary>
    /// Gets or sets the duration of the match in seconds.
    /// </summary>
    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the Unix timestamp describing when the match started.
    /// </summary>
    [JsonPropertyName("start_time")]
    public int StartTime { get; set; }

    /// <summary>
    /// Gets or sets the value describing whether Radiant won the match.
    /// </summary>
    [JsonPropertyName("radiant_win")]
    public bool? RadiantWin { get; set; }

    /// <summary>
    /// Gets or sets the Radiant tower status bitmask.
    /// </summary>
    [JsonPropertyName("tower_status_radiant")]
    public int? TowerStatusRadiant { get; set; }

    /// <summary>
    /// Gets or sets the Dire tower status bitmask.
    /// </summary>
    [JsonPropertyName("tower_status_dire")]
    public int? TowerStatusDire { get; set; }

    /// <summary>
    /// Gets or sets the Radiant barracks status bitmask.
    /// </summary>
    [JsonPropertyName("barracks_status_radiant")]
    public int? BarracksStatusRadiant { get; set; }

    /// <summary>
    /// Gets or sets the Dire barracks status bitmask.
    /// </summary>
    [JsonPropertyName("barracks_status_dire")]
    public int? BarracksStatusDire { get; set; }

    /// <summary>
    /// Gets or sets the cluster identifier the match was played on.
    /// </summary>
    [JsonPropertyName("cluster")]
    public int? Cluster { get; set; }

    /// <summary>
    /// Gets or sets the region identifier the match was played in.
    /// </summary>
    [JsonPropertyName("region")]
    public int? Region { get; set; }

    /// <summary>
    /// Gets or sets the game mode identifier.
    /// </summary>
    [JsonPropertyName("game_mode")]
    public int? GameMode { get; set; }

    /// <summary>
    /// Gets or sets the lobby type identifier.
    /// </summary>
    [JsonPropertyName("lobby_type")]
    public int? LobbyType { get; set; }

    /// <summary>
    /// Gets or sets the series identifier for the match, if applicable.
    /// </summary>
    [JsonPropertyName("series_id")]
    public long? SeriesId { get; set; }

    /// <summary>
    /// Gets or sets the type of series for the match, if applicable.
    /// </summary>
    [JsonPropertyName("series_type")]
    public int? SeriesType { get; set; }

    /// <summary>
    /// Gets or sets the replay URL, if available.
    /// </summary>
    [JsonPropertyName("replay_url")]
    public string? ReplayUrl { get; set; }

    /// <summary>
    /// Gets or sets the number of kills made by the Radiant team.
    /// </summary>
    [JsonPropertyName("radiant_score")]
    public int RadiantScore { get; set; }

    /// <summary>
    /// Gets or sets the number of kills made by the Dire team.
    /// </summary>
    [JsonPropertyName("dire_score")]
    public int DireScore { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the Radiant team, if available.
    /// </summary>
    [JsonPropertyName("radiant_team_id")]
    public long? RadiantTeamId { get; set; }

    /// <summary>
    /// Gets or sets the name of the Radiant team, if available.
    /// </summary>
    [JsonPropertyName("radiant_name")]
    public string? RadiantName { get; set; }

    /// <summary>
    /// Gets or sets the Radiant team metadata.
    /// </summary>
    [JsonPropertyName("radiant_team")]
    public TeamSummary? RadiantTeam { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the Dire team, if available.
    /// </summary>
    [JsonPropertyName("dire_team_id")]
    public long? DireTeamId { get; set; }

    /// <summary>
    /// Gets or sets the name of the Dire team, if available.
    /// </summary>
    [JsonPropertyName("dire_name")]
    public string? DireName { get; set; }

    /// <summary>
    /// Gets or sets the Dire team metadata.
    /// </summary>
    [JsonPropertyName("dire_team")]
    public TeamSummary? DireTeam { get; set; }

    /// <summary>
    /// Gets or sets the league identifier the match is associated with.
    /// </summary>
    [JsonPropertyName("leagueid")]
    public long? LeagueId { get; set; }

    /// <summary>
    /// Gets or sets the league name, if provided by the endpoint.
    /// </summary>
    [JsonPropertyName("league_name")]
    public string? LeagueName { get; set; }

    /// <summary>
    /// Gets or sets the league metadata, if provided by the endpoint.
    /// </summary>
    [JsonPropertyName("league")]
    public LeagueSummary? League { get; set; }

    /// <summary>
    /// Gets or sets the Radiant gold advantage progression, if available.
    /// </summary>
    [JsonPropertyName("radiant_gold_adv")]
    public IReadOnlyList<int>? RadiantGoldAdvantage { get; set; }

    /// <summary>
    /// Gets or sets the Radiant experience advantage progression, if available.
    /// </summary>
    [JsonPropertyName("radiant_xp_adv")]
    public IReadOnlyList<int>? RadiantExperienceAdvantage { get; set; }

    /// <summary>
    /// Gets or sets the player statistics for the match.
    /// </summary>
    [JsonPropertyName("players")]
    public IReadOnlyList<MatchPlayer> Players { get; set; } = Array.Empty<MatchPlayer>();

    /// <summary>
    /// Gets or sets the pick/ban sequence for the match, if available.
    /// </summary>
    [JsonPropertyName("picks_bans")]
    public IReadOnlyList<MatchPickBan> PicksBans { get; set; } = Array.Empty<MatchPickBan>();

    /// <summary>
    /// Gets or sets the draft timing information for the match, if available.
    /// </summary>
    [JsonPropertyName("draft_timings")]
    public IReadOnlyList<MatchDraftTiming> DraftTimings { get; set; } = Array.Empty<MatchDraftTiming>();

    /// <summary>
    /// Holds any additional properties returned by the API that are not explicitly modelled.
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalProperties { get; set; }
}

/// <summary>
/// Represents metadata describing a team within a match.
/// </summary>
public class TeamSummary
{
    /// <summary>
    /// Gets or sets the team identifier.
    /// </summary>
    [JsonPropertyName("team_id")]
    public long? TeamId { get; set; }

    /// <summary>
    /// Gets or sets the team name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the team tag.
    /// </summary>
    [JsonPropertyName("tag")]
    public string? Tag { get; set; }

    /// <summary>
    /// Gets or sets the logo URL for the team.
    /// </summary>
    [JsonPropertyName("logo_url")]
    public string? LogoUrl { get; set; }
}

/// <summary>
/// Represents metadata describing a league associated with a match.
/// </summary>
public class LeagueSummary
{
    /// <summary>
    /// Gets or sets the league identifier.
    /// </summary>
    [JsonPropertyName("leagueid")]
    public long? LeagueId { get; set; }

    /// <summary>
    /// Gets or sets the league name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the league tier.
    /// </summary>
    [JsonPropertyName("tier")]
    public string? Tier { get; set; }

    /// <summary>
    /// Gets or sets the ticket URL associated with the league.
    /// </summary>
    [JsonPropertyName("ticket")]
    public string? Ticket { get; set; }

    /// <summary>
    /// Gets or sets the banner URL associated with the league.
    /// </summary>
    [JsonPropertyName("banner")]
    public string? Banner { get; set; }
}

/// <summary>
/// Represents pick/ban information for a match draft.
/// </summary>
public class MatchPickBan
{
    /// <summary>
    /// Gets or sets a value indicating whether this entry is a pick.
    /// </summary>
    [JsonPropertyName("is_pick")]
    public bool IsPick { get; set; }

    /// <summary>
    /// Gets or sets the team (0 Radiant, 1 Dire) associated with the action.
    /// </summary>
    [JsonPropertyName("team")]
    public int Team { get; set; }

    /// <summary>
    /// Gets or sets the hero identifier involved in the action.
    /// </summary>
    [JsonPropertyName("hero_id")]
    public int HeroId { get; set; }

    /// <summary>
    /// Gets or sets the order of the pick or ban.
    /// </summary>
    [JsonPropertyName("order")]
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets the player slot, if provided by the API.
    /// </summary>
    [JsonPropertyName("player_slot")]
    public int? PlayerSlot { get; set; }

    /// <summary>
    /// Holds additional unmodelled data from the API.
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalProperties { get; set; }
}

/// <summary>
/// Represents draft timing data for a match.
/// </summary>
public class MatchDraftTiming
{
    /// <summary>
    /// Gets or sets the draft order of the event.
    /// </summary>
    [JsonPropertyName("order")]
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the event was a pick.
    /// </summary>
    [JsonPropertyName("pick")]
    public bool IsPick { get; set; }

    /// <summary>
    /// Gets or sets the hero identifier associated with the event.
    /// </summary>
    [JsonPropertyName("hero_id")]
    public int HeroId { get; set; }

    /// <summary>
    /// Gets or sets the player slot responsible for the event.
    /// </summary>
    [JsonPropertyName("player_slot")]
    public int PlayerSlot { get; set; }

    /// <summary>
    /// Gets or sets the total time taken by the team when the event occurred.
    /// </summary>
    [JsonPropertyName("total_time_taken")]
    public int TotalTimeTaken { get; set; }

    /// <summary>
    /// Holds additional unmodelled data from the API.
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalProperties { get; set; }
}

/// <summary>
/// Represents player statistics within a match.
/// </summary>
public class MatchPlayer
{
    /// <summary>
    /// Gets or sets the match identifier associated with the player entry.
    /// </summary>
    [JsonPropertyName("match_id")]
    public long MatchId { get; set; }

    /// <summary>
    /// Gets or sets the account identifier for the player, if available.
    /// </summary>
    [JsonPropertyName("account_id")]
    public long? AccountId { get; set; }

    /// <summary>
    /// Gets or sets the slot the player occupied during the match.
    /// </summary>
    [JsonPropertyName("player_slot")]
    public int PlayerSlot { get; set; }

    /// <summary>
    /// Gets or sets the hero identifier the player selected.
    /// </summary>
    [JsonPropertyName("hero_id")]
    public int HeroId { get; set; }

    /// <summary>
    /// Gets or sets the number of kills for the player.
    /// </summary>
    [JsonPropertyName("kills")]
    public int Kills { get; set; }

    /// <summary>
    /// Gets or sets the number of deaths for the player.
    /// </summary>
    [JsonPropertyName("deaths")]
    public int Deaths { get; set; }

    /// <summary>
    /// Gets or sets the number of assists for the player.
    /// </summary>
    [JsonPropertyName("assists")]
    public int Assists { get; set; }

    /// <summary>
    /// Gets or sets the player's net worth at the end of the match.
    /// </summary>
    [JsonPropertyName("net_worth")]
    public int NetWorth { get; set; }

    /// <summary>
    /// Gets or sets the amount of hero damage dealt by the player.
    /// </summary>
    [JsonPropertyName("hero_damage")]
    public int HeroDamage { get; set; }

    /// <summary>
    /// Gets or sets the amount of tower damage dealt by the player.
    /// </summary>
    [JsonPropertyName("tower_damage")]
    public int TowerDamage { get; set; }

    /// <summary>
    /// Gets or sets the amount of hero healing performed by the player.
    /// </summary>
    [JsonPropertyName("hero_healing")]
    public int HeroHealing { get; set; }

    /// <summary>
    /// Gets or sets the amount of last hits recorded for the player.
    /// </summary>
    [JsonPropertyName("last_hits")]
    public int LastHits { get; set; }

    /// <summary>
    /// Gets or sets the number of denies recorded for the player.
    /// </summary>
    [JsonPropertyName("denies")]
    public int Denies { get; set; }

    /// <summary>
    /// Gets or sets the player's gold per minute.
    /// </summary>
    [JsonPropertyName("gold_per_min")]
    public int GoldPerMinute { get; set; }

    /// <summary>
    /// Gets or sets the player's experience per minute.
    /// </summary>
    [JsonPropertyName("xp_per_min")]
    public int ExperiencePerMinute { get; set; }

    /// <summary>
    /// Gets or sets the player's level at the end of the match.
    /// </summary>
    [JsonPropertyName("level")]
    public int Level { get; set; }

    /// <summary>
    /// Gets or sets the player's team number (0 Radiant, 1 Dire).
    /// </summary>
    [JsonPropertyName("team_number")]
    public int TeamNumber { get; set; }

    /// <summary>
    /// Gets or sets the player's team slot (0-4).
    /// </summary>
    [JsonPropertyName("team_slot")]
    public int TeamSlot { get; set; }

    /// <summary>
    /// Gets or sets the amount of damage taken by the player.
    /// </summary>
    [JsonPropertyName("damage_taken")]
    public int? DamageTaken { get; set; }

    /// <summary>
    /// Gets or sets the array of ability upgrades for the player.
    /// </summary>
    [JsonPropertyName("ability_upgrades_arr")]
    public IReadOnlyList<int>? AbilityUpgrades { get; set; }

    /// <summary>
    /// Gets or sets the array of item identifiers for the player's inventory.
    /// </summary>
    [JsonPropertyName("item_ids")]
    public IReadOnlyList<int>? ItemIds { get; set; }

    /// <summary>
    /// Holds any additional properties returned by the API that are not explicitly modelled.
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalProperties { get; set; }
}
