# .NET Starter Project Template

> Brief description of what this project does.

## Prerequisites

- [.NET 11 SDK](https://dotnet.microsoft.com/download)
- The [Aspire 17 CLI bundle](https://aspire.dev/get-started/aspire-sdk/#use-the-aspire-cli-bundle-for-orchestration-dependencies) for the AppHost
- A container runtime for local runs and end-to-end tests. Start [Docker](https://www.docker.com/get-started) or use [WSLc 3.0.4.0 or later on Windows](https://aka.ms/aspire/containers).

## Getting Started

### Set up the Aspire CLI bundle

From the repository root, restore the pinned .NET tools and install the bundle. Do this before running `dotnet build` or `dotnet test` on the solution or a project that builds the AppHost.

```sh
dotnet tool restore
dotnet tool run aspire setup --install-path "$HOME/.aspire"
```

If you use WSLc, select it in PowerShell before running the AppHost or its end-to-end tests. Set it in each PowerShell session where you run them.

```powershell
$env:ASPIRE_CONTAINER_RUNTIME = 'wslc'
```

### Run locally with Aspire

```sh
dotnet tool run aspire run
```

Aspire will automatically start all required infrastructure (database containers, etc.) and launch
the API. The Aspire Dashboard URL will be printed to the console.

### API Reference

Once running, the Scalar interactive API docs are available at:

```
http://localhost:<port>/scalar
```

## Running Tests

### Unit / Integration Tests

```bash
dotnet test --project src/DotNetStarterProjectTemplate.Tests
```

### End-to-End Tests (Aspire)

```bash
dotnet test --project src/DotNetStarterProjectTemplate.AppHost.Tests
```

These tests start the full Aspire application, including its SQL Server container. Set up the bundle and your container runtime as described above.

## Database Migrations

To add a new EF Core migration, run the following from the repository root:

```bash
dotnet ef migrations add <MigrationName> \
  -p src/DotNetStarterProjectTemplate.Application \
  -s src/DotNetStarterProjectTemplate.Api \
  -o Infrastructure/Persistence/Migrations
```

Migrations are automatically applied on startup by the Worker service when running in the
`Development` environment.

## Project Structure

```
src/
├── DotNetStarterProjectTemplate.Api                # ASP.NET Core Minimal API
├── DotNetStarterProjectTemplate.AppHost            # Aspire AppHost (orchestration)
├── DotNetStarterProjectTemplate.AppHost.Tests      # Aspire E2E tests (TUnit)
├── DotNetStarterProjectTemplate.Application        # Application logic (CQRS, domain, EF)
├── DotNetStarterProjectTemplate.ServiceDefaults    # Shared Aspire service defaults
├── DotNetStarterProjectTemplate.Tests              # Unit / integration tests (TUnit)
└── DotNetStarterProjectTemplate.Worker             # Background worker (migrations, timers)
```

## Environment Variables

Key environment variables used by the application (set automatically by Aspire in local dev):

| Variable | Description |
|---|---|
| `ConnectionStrings__database` | SQL Server connection string |
| `OTEL_EXPORTER_OTLP_ENDPOINT` | OpenTelemetry collector endpoint |

## Contributing

1. Create a feature branch from `main`
2. Make your changes
3. Run `dotnet build DotNetStarterProjectTemplate.slnx` and `dotnet test --solution DotNetStarterProjectTemplate.slnx` to verify everything passes
4. Open a pull request
