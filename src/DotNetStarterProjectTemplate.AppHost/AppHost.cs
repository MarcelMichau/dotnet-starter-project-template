using DotNetStarterProjectTemplate.Application.Shared;

var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddAzureSqlServer("sql-server")
    .RunAsContainer()
    .AddDatabase("database");

var worker = builder.AddDotnetProject($"{Constants.AppAbbreviation}-worker", "../DotNetStarterProjectTemplate.Worker/DotNetStarterProjectTemplate.Worker.csproj")
    .WithReference(database)
    .WaitFor(database);

builder.AddDotnetProject($"{Constants.AppAbbreviation}-api", "../DotNetStarterProjectTemplate.Api/DotNetStarterProjectTemplate.Api.csproj")
    .WithReference(database)
    .WaitFor(worker)
    .WithHttpHealthCheck("health")
    .WithHttpHealthCheck("alive")
    .WithExternalHttpEndpoints();

builder.Build().Run();
