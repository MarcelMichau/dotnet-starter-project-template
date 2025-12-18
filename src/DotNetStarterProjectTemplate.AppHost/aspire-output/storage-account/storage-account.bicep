@description('The location for the resource(s) to be deployed.')
param location string = resourceGroup().location

resource bla 'Microsoft.Storage/storageAccounts@2024-01-01' = {
  name: 'stblablabla'
  kind: 'StorageV2'
  location: location
  sku: {
    name: 'Standard_LRS'
  }
}