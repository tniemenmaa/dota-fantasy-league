using DotaFantasyLeague.Api.Models;

namespace DotaFantasyLeague.Api.Services;

/// <summary>
/// Provides access to Stratz GraphQL resources.
/// </summary>
public interface IStratzGraphQlService
{
    /// <summary>
    /// Retrieves the specified team along with its roster via the Stratz GraphQL API.
    /// </summary>
    /// <param name="teamId">Identifier of the team.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The requested team, or <c>null</c> if it does not exist.</returns>
    Task<TeamDetails?> GetTeamAsync(long teamId, CancellationToken cancellationToken = default);
}
