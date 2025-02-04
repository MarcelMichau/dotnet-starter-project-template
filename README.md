<h1 align="center">
  .NET Starter Project Template
</h1>

<p align="center">
This is a .NET project template which serves as a good starting point to scale to more complex projects.
</p>

[Open in github.dev](https://github.dev/MarcelMichau/dotnet-starter-project-template)

This template includes some opinionated defaults based on personal preference.

It has the following notable features:

- ASP.NET Core Minimal API project
- ASP.NET Core Worker project - for background/long-running tasks
- Docker support using `Dockerfile`
- .NET Aspire Support
- .NET Aspire Test Project for E2E tests
- [NuGet Package Auditing](https://learn.microsoft.com/en-us/nuget/concepts/auditing-packages)
- [NuGet Central Package Management](https://learn.microsoft.com/en-us/nuget/consume-packages/Central-Package-Management)
- [NuGet Lock Files Enabled](https://learn.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files#enabling-the-lock-file)
- Centralised project configuration using `Directory.Build.Props` - to consolidate duplicated project properties across projects
- Basic `index.html` landing page for the API project - so that you don't get a 404 when navigating to the root URL
- Default `global.json` file with roll-forward policy set to `latestFeature` to always use the latest installed feature band
