using DotaFantasyLeague.Api.Models;
using DotaFantasyLeague.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotaFantasyLeague.Api.Controllers;

/// <summary>
/// Provides operations for working with professional Dota teams.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly IOpenDotaTeamsService _teamsService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TeamsController"/> class.
    /// </summary>
    /// <param name="teamsService">Service used to retrieve team information.</param>
    public TeamsController(IOpenDotaTeamsService teamsService)
    {
        _teamsService = teamsService;
    }

    /// <summary>
    /// Retrieves the players associated with the provided team identifier.
    /// </summary>
    /// <param name="teamId">Identifier of the team.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>A collection of players sourced from the OpenDota API.</returns>
    [HttpGet("{teamId:long}/players")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<TeamPlayer>>> GetPlayers(long teamId, CancellationToken cancellationToken)
    {
        var players = await _teamsService.GetPlayersAsync(teamId, cancellationToken);
        return Ok(players);
    }
}
