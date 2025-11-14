using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DotaFantasyLeague.Api.Swagger;

/// <summary>
/// Sets a default value for the leagueId parameter in Swagger operations.
/// </summary>
public sealed class DefaultLeagueIdOperationFilter : IOperationFilter
{
    private const long DefaultLeagueId = 18650;

    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters is null)
        {
            return;
        }

        foreach (var parameter in operation.Parameters.Where(parameter => parameter.Name == "leagueId"))
        {
            parameter.Schema.Default = new OpenApiLong(DefaultLeagueId);
            parameter.Example = new OpenApiLong(DefaultLeagueId);
        }
    }
}
