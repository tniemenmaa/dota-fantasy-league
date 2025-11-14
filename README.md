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

## Next steps

* Add domain-specific endpoints, services, and persistence layers under the `src` directory.
* Create test projects under a `tests` folder to accompany new features.
* Configure CI/CD pipelines to build, test, and deploy the solution.
