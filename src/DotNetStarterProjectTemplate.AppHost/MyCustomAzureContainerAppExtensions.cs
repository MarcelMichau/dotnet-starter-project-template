using System.Reflection;
using Aspire.Hosting.Azure.AppContainers;
using Azure.Provisioning;
using Azure.Provisioning.AppContainers;
using Azure.Provisioning.Storage;

namespace DotNetStarterProjectTemplate.AppHost;

internal static class MyCustomAzureContainerAppExtensions
{
    public static IResourceBuilder<AzureContainerAppEnvironmentResource> AddMinimalAzureContainerAppEnvironment(
        this IDistributedApplicationBuilder builder, string name)
    {
        builder.AddAzureProvisioning();

        var containerAppEnvResource = new AzureContainerAppEnvironmentResource(name, static infra =>
        {
            var appEnvResource = (AzureContainerAppEnvironmentResource)infra.AspireResource;

            var containerAppEnvironment = new ContainerAppManagedEnvironment(appEnvResource.GetBicepIdentifier())
            {
                Name = "cae-my-awesome-app",
                WorkloadProfiles = [
                    new ContainerAppWorkloadProfile
                    {
                        WorkloadProfileType = "Consumption",
                        Name = "consumption"
                    }
                ],
                AppLogsConfiguration = new()
                {
                    Destination = "log-analytics",
                    LogAnalyticsConfiguration = new()
                    {
                        CustomerId = "bla",
                        SharedKey = "di-bla"
                    }
                },
            };

            infra.Add(containerAppEnvironment);
        });

        var customStorageAccountResource = new MyCustomStorageAccountResource("storage-account", static infra =>
        {
            var storageAccount = new StorageAccount("bla")
            {
                Name =  "stmystorage001",
                Kind = StorageKind.StorageV2,
                Sku = new StorageSku
                {
                    Name = StorageSkuName.StandardLrs
                }
            };

            infra.Add(storageAccount);
        });

        builder.AddResource(customStorageAccountResource);

        return builder.AddResource(containerAppEnvResource);
    }
}
