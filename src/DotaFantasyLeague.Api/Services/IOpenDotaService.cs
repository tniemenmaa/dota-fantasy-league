using DotaFantasyLeague.Api.Models;

namespace DotaFantasyLeague.Api.Services;

/// <summary>
/// Provides access to data from the OpenDota API.
/// </summary>
public interface IOpenDotaService
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
    Task<IReadOnlyList<long>> GetMatchIdsAsync(long leagueId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the details for a match with the provided identifier.
    /// </summary>
    /// <param name="matchId">Identifier of the match.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The match details, if found.</returns>
    Task<MatchDetails> GetMatchAsync(long matchId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the players who have played for the specified team identifier.
    /// </summary>
    /// <param name="teamId">The team identifier to filter players.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>A collection of team players returned by the OpenDota API.</returns>
    Task<IReadOnlyList<TeamPlayer>> GetPlayersAsync(long teamId, CancellationToken cancellationToken = default);
}
