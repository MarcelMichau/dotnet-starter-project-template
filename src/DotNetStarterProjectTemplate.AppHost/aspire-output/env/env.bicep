@description('The location for the resource(s) to be deployed.')
param location string = resourceGroup().location

resource env 'Microsoft.App/managedEnvironments@2025-01-01' = {
  name: 'cae-my-awesome-app'
  location: location
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: 'bla'
        sharedKey: 'di-bla'
      }
    }
    workloadProfiles: [
      {
        name: 'consumption'
        workloadProfileType: 'Consumption'
      }
    ]
  }
}