var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.DotNetStarterProjectTemplate_Api>("dotnet-starter-project-template-api");

builder.AddProject<Projects.DotNetStarterProjectTemplate_Worker>("dotnet-starter-project-template-worker");

builder.Build().Run();
