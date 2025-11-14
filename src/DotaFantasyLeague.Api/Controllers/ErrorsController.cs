using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DotaFantasyLeague.Api.Controllers;

/// <summary>
/// Provides consistent error responses for unhandled exceptions.
/// </summary>
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ControllerBase
{
    /// <summary>
    /// Returns a standardized problem details response for exceptions.
    /// </summary>
    /// <returns>An RFC 7807 problem details payload.</returns>
    [Route("error")]
    [HttpGet]
    public IActionResult HandleError()
    {
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        return Problem(detail: exceptionFeature?.Error.Message);
    }
}
