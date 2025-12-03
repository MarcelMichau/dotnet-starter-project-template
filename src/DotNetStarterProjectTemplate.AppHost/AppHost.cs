#pragma warning disable ASPIREPIPELINES001

using DotNetStarterProjectTemplate.AppHost.Annotations;
using DotNetStarterProjectTemplate.Application.Shared;
using Microsoft.Extensions.Logging;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddAzureContainerAppEnvironment("env");

var database = builder.AddAzureSqlServer("sql-server")
    .RunAsContainer()
    .AddDatabase("database")
    .WithProvisioningRequestEmail("sql-my-awesome-app-dev-001");

builder.Pipeline.AddStep("test-step", async context =>
{
    context.Logger.LogInformation("This is a test pipeline step.");
});

builder.Pipeline.AddStep("get-compute-resources", async context =>
{
    foreach (var resource in context.Model.GetComputeResources())
    {
        context.Logger.LogInformation("Compute Resource: {resource}", resource.Name);
    }
});

var worker = builder.AddProject<Projects.DotNetStarterProjectTemplate_Worker>($"{Constants.AppAbbreviation}-worker")
    .WithReference(database)
    .WaitFor(database);

builder.AddProject<Projects.DotNetStarterProjectTemplate_Api>($"{Constants.AppAbbreviation}-api")
    .WithReference(database)
    .WaitFor(worker)
    .WithHttpHealthCheck("health")
    .WithHttpHealthCheck("alive")
    .WithExternalHttpEndpoints()
    .WithAnnotation(new DisableForwardedHeadersAnnotation());


builder.Build().Run();