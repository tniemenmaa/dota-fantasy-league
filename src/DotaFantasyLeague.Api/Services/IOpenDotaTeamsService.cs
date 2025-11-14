using DotaFantasyLeague.Api.Models;

namespace DotaFantasyLeague.Api.Services;

/// <summary>
/// Provides operations for retrieving team data from the OpenDota API.
/// </summary>
public interface IOpenDotaTeamsService
{
    /// <summary>
    /// Retrieves the players who have played for the specified team identifier.
    /// </summary>
    /// <param name="teamId">The team identifier to filter players.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>A collection of team players returned by the OpenDota API.</returns>
    Task<IReadOnlyList<TeamPlayer>> GetPlayersAsync(long teamId, CancellationToken cancellationToken = default);
}
