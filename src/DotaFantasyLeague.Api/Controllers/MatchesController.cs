using DotaFantasyLeague.Api.Models;
using DotaFantasyLeague.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotaFantasyLeague.Api.Controllers;

/// <summary>
/// Provides operations for working with Dota matches.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MatchesController : ControllerBase
{
    private readonly IOpenDotaService _openDotaService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchesController"/> class.
    /// </summary>
    /// <param name="openDotaService">Service used to retrieve match details.</param>
    public MatchesController(IOpenDotaService openDotaService)
    {
        _openDotaService = openDotaService;
    }

    /// <summary>
    /// Retrieves the details of a match for the provided match identifier.
    /// </summary>
    /// <param name="matchId">Identifier of the match to retrieve.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The match details sourced from the OpenDota API.</returns>
    [HttpGet("{matchId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MatchDetails>> GetMatch(long matchId, CancellationToken cancellationToken)
    {
        var match = await _openDotaService.GetMatchAsync(matchId, cancellationToken);
        return Ok(match);
    }
}
