namespace DotaFantasyLeague.Api.Configuration;

/// <summary>
/// Options used to configure the Cosmos DB connection.
/// </summary>
public class CosmosDbOptions
{
    /// <summary>
    /// Configuration section name for Cosmos DB settings.
    /// </summary>
    public const string SectionName = "CosmosDb";

    /// <summary>
    /// The Cosmos DB account endpoint.
    /// </summary>
    public string? AccountEndpoint { get; set; }

    /// <summary>
    /// The Cosmos DB account key.
    /// </summary>
    public string? AccountKey { get; set; }

    /// <summary>
    /// The Cosmos DB database name to use.
    /// </summary>
    public string? DatabaseName { get; set; }

    /// <summary>
    /// Indicates whether the options contain enough data to configure the provider.
    /// </summary>
    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(AccountEndpoint)
        && !string.IsNullOrWhiteSpace(AccountKey)
        && !string.IsNullOrWhiteSpace(DatabaseName);
}
