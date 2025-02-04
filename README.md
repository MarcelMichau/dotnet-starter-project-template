# .NET Starter Project Template

This is a .NET project template which serves as a good starting point to scale to more complex projects.

[![Build + Test](https://github.com/MarcelMichau/dotnet-starter-project-template/actions/workflows/build-test.yml/badge.svg)](https://github.com/MarcelMichau/dotnet-starter-project-template/actions/workflows/build-test.yml)
[![CodeQL](https://github.com/MarcelMichau/dotnet-starter-project-template/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/MarcelMichau/dotnet-starter-project-template/actions/workflows/github-code-scanning/codeql)
[![Nuget](https://img.shields.io/nuget/v/MarcelMichau.Templates.DotNetStarterProject?label=NuGet)](https://www.nuget.org/packages/MarcelMichau.Templates.DotNetStarterProject)

[Open in github.dev](https://github.dev/MarcelMichau/dotnet-starter-project-template)

This template includes some opinionated defaults based on personal preference.

It has the following notable features & pre-configured defaults:

- ASP.NET Core Minimal API project
- ASP.NET Core Worker project - for background/long-running tasks
- Docker support using `Dockerfile`
- .NET Aspire Support
- .NET Aspire Test Project for E2E tests
- GitHub Actions Workflow for building & testing solution
- Dependabot configuration for automatic dependency updates
- [NuGet Package Auditing](https://learn.microsoft.com/en-us/nuget/concepts/auditing-packages)
- [NuGet Central Package Management](https://learn.microsoft.com/en-us/nuget/consume-packages/Central-Package-Management)
- [NuGet Lock Files Enabled](https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files#enabling-the-lock-file)
- [.NET Chiseled Containers](https://devblogs.microsoft.com/dotnet/announcing-dotnet-chiseled-containers/)
- Centralised project configuration using `Directory.Build.Props` - to consolidate duplicated project properties across projects
- Basic `index.html` landing page for the API project - to avoid a 404 when navigating to the root URL
- Default `global.json` file with roll-forward policy set to `latestFeature` to always use the latest installed feature band

## Getting Started

Install the [.NET template](https://www.nuget.org/packages/MarcelMichau.Templates.DotNetStarterProject):
```
dotnet new install MarcelMichau.Templates.DotNetStarterProject::1.0.4
```
