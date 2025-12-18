using Aspire.Hosting.Azure;
using Azure.Provisioning.Storage;
namespace DotNetStarterProjectTemplate.AppHost;

internal class MyCustomStorageAccountResource: AzureProvisioningResource
{
    public MyCustomStorageAccountResource(string name, Action<AzureResourceInfrastructure> configureInfrastructure) : base(name, configureInfrastructure)
    {
        var storageAccount = new StorageAccount("bla")
        {
            Name = "stmystorage001",
            Kind = StorageKind.StorageV2,
            Sku = new StorageSku
            {
                Name = StorageSkuName.StandardLrs
            }
        };
    }
}
