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
    private readonly IOpenDotaService _openDotaService;
    private readonly IStratzGraphQlService _stratzGraphQlService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TeamsController"/> class.
    /// </summary>
    /// <param name="openDotaService">Service used to retrieve team information.</param>
    /// <param name="stratzGraphQlService">Service used to retrieve team rosters from Stratz.</param>
    public TeamsController(IOpenDotaService openDotaService, IStratzGraphQlService stratzGraphQlService)
    {
        _openDotaService = openDotaService;
        _stratzGraphQlService = stratzGraphQlService;
    }

    /// <summary>
    /// Retrieves the metadata and roster for the provided team identifier via Stratz GraphQL.
    /// </summary>
    /// <param name="teamId">Identifier of the team.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    [HttpGet("{teamId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TeamDetails>> GetTeam(long teamId, CancellationToken cancellationToken)
    {
        var team = await _stratzGraphQlService.GetTeamAsync(teamId, cancellationToken);

        if (team is null)
        {
            return NotFound();
        }

        return Ok(team);
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
        var players = await _openDotaService.GetPlayersAsync(teamId, cancellationToken);
        return Ok(players);
    }
}
