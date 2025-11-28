targetScope = 'subscription'

param resourceGroupName string

param location string

param principalId string

resource rg 'Microsoft.Resources/resourceGroups@2023-07-01' = {
  name: resourceGroupName
  location: location
}

module env 'env/env.bicep' = {
  name: 'env'
  scope: rg
  params: {
    location: location
    userPrincipalId: principalId
  }
}

module sql_server 'sql-server/sql-server.bicep' = {
  name: 'sql-server'
  scope: rg
  params: {
    location: location
  }
}

module dspt_worker_identity 'dspt-worker-identity/dspt-worker-identity.bicep' = {
  name: 'dspt-worker-identity'
  scope: rg
  params: {
    location: location
  }
}

module dspt_worker_roles_sql_server 'dspt-worker-roles-sql-server/dspt-worker-roles-sql-server.bicep' = {
  name: 'dspt-worker-roles-sql-server'
  scope: rg
  params: {
    location: location
    sql_server_outputs_name: sql_server.outputs.name
    sql_server_outputs_sqlserveradminname: sql_server.outputs.sqlServerAdminName
    principalId: dspt_worker_identity.outputs.principalId
    principalName: dspt_worker_identity.outputs.principalName
  }
}

module dspt_api_identity 'dspt-api-identity/dspt-api-identity.bicep' = {
  name: 'dspt-api-identity'
  scope: rg
  params: {
    location: location
  }
}

module dspt_api_roles_sql_server 'dspt-api-roles-sql-server/dspt-api-roles-sql-server.bicep' = {
  name: 'dspt-api-roles-sql-server'
  scope: rg
  params: {
    location: location
    sql_server_outputs_name: sql_server.outputs.name
    sql_server_outputs_sqlserveradminname: sql_server.outputs.sqlServerAdminName
    principalId: dspt_api_identity.outputs.principalId
    principalName: dspt_api_identity.outputs.principalName
  }
}

output env_AZURE_CONTAINER_REGISTRY_NAME string = env.outputs.AZURE_CONTAINER_REGISTRY_NAME

output env_AZURE_CONTAINER_REGISTRY_ENDPOINT string = env.outputs.AZURE_CONTAINER_REGISTRY_ENDPOINT

output env_AZURE_CONTAINER_REGISTRY_MANAGED_IDENTITY_ID string = env.outputs.AZURE_CONTAINER_REGISTRY_MANAGED_IDENTITY_ID

output env_AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN string = env.outputs.AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN

output env_AZURE_CONTAINER_APPS_ENVIRONMENT_ID string = env.outputs.AZURE_CONTAINER_APPS_ENVIRONMENT_ID

output dspt_worker_identity_id string = dspt_worker_identity.outputs.id

output sql_server_sqlServerFqdn string = sql_server.outputs.sqlServerFqdn

output dspt_worker_identity_clientId string = dspt_worker_identity.outputs.clientId

output dspt_api_identity_id string = dspt_api_identity.outputs.id

output dspt_api_identity_clientId string = dspt_api_identity.outputs.clientId