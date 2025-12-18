targetScope = 'subscription'

param resourceGroupName string

param location string

param principalId string

resource rg 'Microsoft.Resources/resourceGroups@2023-07-01' = {
  name: resourceGroupName
  location: location
}

module storage_account 'storage-account/storage-account.bicep' = {
  name: 'storage-account'
  scope: rg
  params: {
    location: location
  }
}

module env 'env/env.bicep' = {
  name: 'env'
  scope: rg
  params: {
    location: location
  }
}