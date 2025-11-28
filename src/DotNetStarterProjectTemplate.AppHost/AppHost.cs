using DotNetStarterProjectTemplate.AppHost.Annotations;
using DotNetStarterProjectTemplate.Application.Shared;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddAzureContainerAppEnvironment("env");

var database = builder.AddAzureSqlServer("sql-server")
    .RunAsContainer()
    .AddDatabase("database")
    .WithProvisioningRequestEmail("sql-server");

var worker = builder.AddProject<Projects.DotNetStarterProjectTemplate_Worker>($"{Constants.AppAbbreviation}-worker")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Projects.DotNetStarterProjectTemplate_Api>($"{Constants.AppAbbreviation}-api")
    .WithReference(database)
    .WaitFor(worker)
    .WithHttpHealthCheck("health")
    .WithHttpHealthCheck("alive")
    .WithExternalHttpEndpoints();

builder.Build().Run();