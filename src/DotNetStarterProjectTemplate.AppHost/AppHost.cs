#pragma warning disable ASPIREPIPELINES001

using Aspire.Hosting;
using Aspire.Hosting.Azure;
using Azure.Provisioning;
using Azure.Provisioning.Roles;
using DotNetStarterProjectTemplate.AppHost.Annotations;
using DotNetStarterProjectTemplate.Application.Shared;
using Microsoft.Extensions.Logging;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddAzureContainerAppEnvironment("env");

var database = builder.AddAzureSqlServer("sql-server")
    .RunAsContainer()
    .AddDatabase("database")
    .WithProvisioningRequestEmail("sql-my-awesome-app-dev-001");

builder.Pipeline.AddStep("test-step",
    async context => { context.Logger.LogInformation("This is a test pipeline step."); });

builder.Pipeline.AddStep("get-compute-resources", async context =>
{
    foreach (var resource in context.Model.GetComputeResources())
    {
        context.Logger.LogInformation("Compute Resource: {resource}", resource.Name);
    }
});

List<string> storageAccounts = ["audit", "content", "logs"];

var bicepStorageAccounts = new List<IResourceBuilder<AzureBicepResource>>();

foreach (var storageAccount in storageAccounts)

{
    bicepStorageAccounts.Add(builder.AddBicepTemplate(
            name: storageAccount,
            bicepFile: "./custom-storage.bicep")
        .WithParameter("storageAccountName", $"st{storageAccount}dev001"));
}

var worker = builder.AddProject<Projects.DotNetStarterProjectTemplate_Worker>($"{Constants.AppAbbreviation}-worker")
    .WithReference(database)
    .WithEnvironment($"ConnectionStrings:Storage:{storageAccounts[0]}", bicepStorageAccounts[0].GetOutput("connectionString"))
    .WithEnvironment($"ConnectionStrings:Storage:{storageAccounts[1]}", bicepStorageAccounts[1].GetOutput("connectionString"))
    .WithEnvironment($"ConnectionStrings:Storage:{storageAccounts[2]}", bicepStorageAccounts[2].GetOutput("connectionString"))
    .WaitFor(database);

builder.AddProject<Projects.DotNetStarterProjectTemplate_Api>($"{Constants.AppAbbreviation}-api")
    .WithReference(database)
    .WaitFor(worker)
    .WithHttpHealthCheck("health")
    .WithHttpHealthCheck("alive")
    .WithExternalHttpEndpoints()
    .WithAnnotation(new DisableForwardedHeadersAnnotation());


builder.Build().Run();