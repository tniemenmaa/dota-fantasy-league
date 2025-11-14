using DotaFantasyLeague.Api.Models;
using DotaFantasyLeague.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotaFantasyLeague.Api.Controllers;

/// <summary>
/// Provides operations for working with Dota leagues.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LeaguesController : ControllerBase
{
    private readonly IOpenDotaService _openDotaService;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeaguesController"/> class.
    /// </summary>
    /// <param name="openDotaService">Service used to retrieve league information.</param>
    public LeaguesController(IOpenDotaService openDotaService)
    {
        _openDotaService = openDotaService;
    }

    /// <summary>
    /// Retrieves all matches associated with the provided league identifier.
    /// </summary>
    /// <param name="leagueId">Identifier of the league.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>A collection of matches sourced from the OpenDota API.</returns>
    [HttpGet("{leagueId:long}/matches")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<LeagueMatch>>> GetMatches(long leagueId, CancellationToken cancellationToken)
    {
        var matches = await _openDotaService.GetMatchesAsync(leagueId, cancellationToken);
        return Ok(matches);
    }

    /// <summary>
    /// Retrieves all match identifiers associated with the provided league identifier.
    /// </summary>
    /// <param name="leagueId">Identifier of the league.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>A collection of match identifiers sourced from the OpenDota API.</returns>
    [HttpGet("{leagueId:long}/matchIds")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<long>>> GetMatchIds(long leagueId, CancellationToken cancellationToken)
    {
        var matchIds = await _openDotaService.GetMatchIdsAsync(leagueId, cancellationToken);
        return Ok(matchIds);
    }
}
