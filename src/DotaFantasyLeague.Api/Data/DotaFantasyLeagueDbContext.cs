using Microsoft.EntityFrameworkCore;

namespace DotaFantasyLeague.Api.Data;

/// <summary>
/// Database context configured to use Azure Cosmos DB.
/// </summary>
public class DotaFantasyLeagueDbContext(DbContextOptions<DotaFantasyLeagueDbContext> options) : DbContext(options)
{
}
