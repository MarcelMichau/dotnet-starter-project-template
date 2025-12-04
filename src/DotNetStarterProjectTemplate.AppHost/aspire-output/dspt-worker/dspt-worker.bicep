@description('The location for the resource(s) to be deployed.')
param location string = resourceGroup().location

param env_outputs_azure_container_apps_environment_default_domain string

param env_outputs_azure_container_apps_environment_id string

param dspt_worker_containerimage string

param dspt_worker_identity_outputs_id string

param sql_server_outputs_sqlserverfqdn string

param audit_outputs_connectionstring string

param content_outputs_connectionstring string

param logs_outputs_connectionstring string

param dspt_worker_identity_outputs_clientid string

param env_outputs_azure_container_registry_endpoint string

param env_outputs_azure_container_registry_managed_identity_id string

resource dspt_worker 'Microsoft.App/containerApps@2025-02-02-preview' = {
  name: 'dspt-worker'
  location: location
  properties: {
    configuration: {
      activeRevisionsMode: 'Single'
      registries: [
        {
          server: env_outputs_azure_container_registry_endpoint
          identity: env_outputs_azure_container_registry_managed_identity_id
        }
      ]
      runtime: {
        dotnet: {
          autoConfigureDataProtection: true
        }
      }
    }
    environmentId: env_outputs_azure_container_apps_environment_id
    template: {
      containers: [
        {
          image: dspt_worker_containerimage
          name: 'dspt-worker'
          env: [
            {
              name: 'OTEL_DOTNET_EXPERIMENTAL_OTLP_EMIT_EXCEPTION_LOG_ATTRIBUTES'
              value: 'true'
            }
            {
              name: 'OTEL_DOTNET_EXPERIMENTAL_OTLP_EMIT_EVENT_LOG_ATTRIBUTES'
              value: 'true'
            }
            {
              name: 'OTEL_DOTNET_EXPERIMENTAL_OTLP_RETRY'
              value: 'in_memory'
            }
            {
              name: 'ConnectionStrings__database'
              value: 'Server=tcp:${sql_server_outputs_sqlserverfqdn},1433;Encrypt=True;Authentication="Active Directory Default";Database=database'
            }
            {
              name: 'ConnectionStrings:Storage:audit'
              value: audit_outputs_connectionstring
            }
            {
              name: 'ConnectionStrings:Storage:content'
              value: content_outputs_connectionstring
            }
            {
              name: 'ConnectionStrings:Storage:logs'
              value: logs_outputs_connectionstring
            }
            {
              name: 'AZURE_CLIENT_ID'
              value: dspt_worker_identity_outputs_clientid
            }
            {
              name: 'AZURE_TOKEN_CREDENTIALS'
              value: 'ManagedIdentityCredential'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 1
      }
    }
  }
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${dspt_worker_identity_outputs_id}': { }
      '${env_outputs_azure_container_registry_managed_identity_id}': { }
    }
  }
}