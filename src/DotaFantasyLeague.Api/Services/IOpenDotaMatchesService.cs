namespace DotaFantasyLeague.Api.Services;

using DotaFantasyLeague.Api.Models;

/// <summary>
/// Provides access to match information from the OpenDota API.
/// </summary>
public interface IOpenDotaMatchesService
{
    /// <summary>
    /// Retrieves the details for a match with the provided identifier.
    /// </summary>
    /// <param name="matchId">Identifier of the match.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The match details, if found.</returns>
    Task<MatchDetails> GetMatchAsync(long matchId, CancellationToken cancellationToken = default);
}
