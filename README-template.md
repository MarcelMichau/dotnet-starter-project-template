# {appFriendlyName}

> Brief description of what this project does.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started) (required by Aspire to run local containers)
- [Aspire CLI](https://aspire.dev/docs/get-started/) (`dotnet tool install -g aspire`)

## Getting Started

### Run locally with Aspire

From the repository root:

```bash
cd src/{appName}.AppHost
dotnet run
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
dotnet test src/{appName}.Tests
```

### End-to-End Tests (Aspire)

```bash
dotnet test src/{appName}.AppHost.Tests
```

> **Note:** E2E tests start the full Aspire application stack (including Docker containers).
> Ensure Docker is running before executing them.

## Database Migrations

To add a new EF Core migration, run the following from the repository root:

```bash
dotnet ef migrations add <MigrationName> \
  -p src/{appName}.Application \
  -s src/{appName}.Api \
  -o Infrastructure/Persistence/Migrations
```

Migrations are automatically applied on startup by the Worker service when running in the
`Development` environment.

## Project Structure

```
src/
├── {appName}.Api                # ASP.NET Core Minimal API
├── {appName}.AppHost            # Aspire AppHost (orchestration)
├── {appName}.AppHost.Tests      # Aspire E2E tests (TUnit)
├── {appName}.Application        # Application logic (CQRS, domain, EF)
├── {appName}.ServiceDefaults    # Shared Aspire service defaults
├── {appName}.Tests              # Unit / integration tests (TUnit)
└── {appName}.Worker             # Background worker (migrations, timers)
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
3. Run `dotnet build` and `dotnet test` to verify everything passes
4. Open a pull request
