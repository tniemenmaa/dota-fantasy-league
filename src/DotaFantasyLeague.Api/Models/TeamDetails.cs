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
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the display name of the team.
    /// </summary>
    public string? Name { get; set; }

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
    public required long PlayerId { get; set; }
    public required string Name { get; set; }
}
