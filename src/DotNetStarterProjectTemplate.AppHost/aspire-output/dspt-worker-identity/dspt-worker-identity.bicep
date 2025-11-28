@description('The location for the resource(s) to be deployed.')
param location string = resourceGroup().location

resource dspt_worker_identity 'Microsoft.ManagedIdentity/userAssignedIdentities@2024-11-30' = {
  name: take('dspt_worker_identity-${uniqueString(resourceGroup().id)}', 128)
  location: location
}

output id string = dspt_worker_identity.id

output clientId string = dspt_worker_identity.properties.clientId

output principalId string = dspt_worker_identity.properties.principalId

output principalName string = dspt_worker_identity.name

output name string = dspt_worker_identity.name