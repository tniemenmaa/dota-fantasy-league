using System;
using System.Collections.Generic;

namespace DotaFantasyLeague.Api.Models;

/// <summary>
/// Represents an individual Dota team with roster information.
/// </summary>
public sealed class TeamDetails
{
    /// <summary>
    /// Gets or sets the unique identifier of the team.
    /// </summary>
    public long TeamId { get; set; }

    /// <summary>
    /// Gets or sets the display name of the team.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the short tag for the team.
    /// </summary>
    public string? Tag { get; set; }

    /// <summary>
    /// Gets or sets the roster for the team.
    /// </summary>
    public IReadOnlyList<TeamMember> Members { get; set; } = Array.Empty<TeamMember>();
}

/// <summary>
/// Represents an individual team member.
/// </summary>
public sealed class TeamMember
{
    public long PlayerId { get; set; }

    public string? Name { get; set; }

    public bool IsActive { get; set; }

    public string? Role { get; set; }

    public string? Rank { get; set; }

    public DateTimeOffset? StartDateTime { get; set; }

    public DateTimeOffset? EndDateTime { get; set; }

    public TeamMemberSteamAccount? SteamAccount { get; set; }
}

/// <summary>
/// Contains details about the professional Steam account associated with a team member.
/// </summary>
public sealed class TeamMemberSteamAccount
{
    public long? SteamAccountId { get; set; }

    public string? Name { get; set; }

    public string? RealName { get; set; }
}
