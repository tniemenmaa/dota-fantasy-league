# Dota Fantasy League

This repository hosts the ASP.NET Core solution for the Dota Fantasy League platform. The initial setup contains a Web API project that provides a foundation for future features while following modern .NET best practices.

## Solution layout

```
DotaFantasyLeague.sln       # Solution file
Directory.Build.props       # Shared configuration for all projects
src/
  DotaFantasyLeague.Api/    # ASP.NET Core Web API project
```

### API project highlights

* Targets **.NET 8** with nullable reference types and implicit usings enabled.
* Enforces analyzer warnings as errors via the shared `Directory.Build.props` file.
* Exposes a `/api/health` endpoint for quick service diagnostics.
* Configures Swagger/OpenAPI (including XML comments when available) for interactive documentation.
* Uses centralized error handling that returns RFC 7807 compliant problem details responses.

## Getting started

1. Install the [.NET 8 SDK](https://dotnet.microsoft.com/download).
2. Restore dependencies and build the solution:

   ```bash
   dotnet restore
   dotnet build
   ```

3. Run the API project:

   ```bash
   dotnet run --project src/DotaFantasyLeague.Api
   ```

4. Navigate to `https://localhost:7261/swagger` to explore the interactive API documentation.

## Running with Azure Cosmos DB locally

The API can be configured to persist data to Azure Cosmos DB through Entity Framework Core. A Docker Compose file is included to spin up the Linux Cosmos DB emulator quickly.

1. Start the emulator:

   ```bash
   docker compose -f docker-compose.cosmosdb.yml up -d
   ```

2. Trust the emulator's TLS certificate so the .NET SDK can connect over HTTPS:

   ```bash
   mkdir -p ./certs
   curl -k https://localhost:8081/_explorer/emulator.pem -o ./certs/cosmos-emulator.crt
   sudo cp ./certs/cosmos-emulator.crt /usr/local/share/ca-certificates/
   sudo update-ca-certificates
   ```

3. The default `appsettings.Development.json` already points the API at the local emulator using the emulator master key. To use a different database name or a real Cosmos DB account, update the `CosmosDb` section of `appsettings.json` (or user secrets/environment variables in production):

   ```json
   "CosmosDb": {
     "AccountEndpoint": "https://localhost:8081/",
     "AccountKey": "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5mYQ=",
     "DatabaseName": "DotaFantasyLeague"
   }
   ```

4. Run the API as normal. On startup the database will be created automatically if the Cosmos DB configuration is present.

## Next steps

* Add domain-specific endpoints, services, and persistence layers under the `src` directory.
* Create test projects under a `tests` folder to accompany new features.
* Configure CI/CD pipelines to build, test, and deploy the solution.
