using DotaFantasyLeague.Api.Models;

namespace DotaFantasyLeague.Api.Services;

/// <summary>
/// Provides operations for retrieving league data from the OpenDota API.
/// </summary>
public interface IOpenDotaLeagueService
{
    /// <summary>
    /// Retrieves all matches for the specified league identifier.
    /// </summary>
    /// <param name="leagueId">The league identifier to filter matches.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>A collection of matches returned by the OpenDota API.</returns>
    Task<IReadOnlyList<LeagueMatch>> GetMatchesAsync(long leagueId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all match identifiers for the specified league identifier.
    /// </summary>
    /// <param name="leagueId">The league identifier to filter match identifiers.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>A collection of match identifiers returned by the OpenDota API.</returns>
    Task<IReadOnlyList<string>> GetMatchIdsAsync(long leagueId, CancellationToken cancellationToken = default);
}
