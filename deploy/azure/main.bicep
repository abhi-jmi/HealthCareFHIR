@description('Azure region for the FHIR platform infrastructure.')
param location string = resourceGroup().location

@description('Workload/environment suffix, for example dev, test, prod.')
param environmentName string = 'dev'

@description('Container image for the custom API.')
param apiImage string

@description('Container image for the Blazor web app.')
param webImage string

@description('Container image for the worker.')
param workerImage string

@secure()
@description('SQL administrator password. Use Key Vault or deployment-time secret injection in production.')
param sqlAdministratorPassword string

var prefix = 'fhirplat-${environmentName}'

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: '${prefix}-law'
  location: location
  properties: { sku: { name: 'PerGB2018' } retentionInDays: 30 }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: '${prefix}-appi'
  location: location
  kind: 'web'
  properties: { Application_Type: 'web'; WorkspaceResourceId: logAnalytics.id }
}

resource containerEnv 'Microsoft.App/managedEnvironments@2024-03-01' = {
  name: '${prefix}-cae'
  location: location
  properties: { appLogsConfiguration: { destination: 'log-analytics'; logAnalyticsConfiguration: { customerId: logAnalytics.properties.customerId; sharedKey: logAnalytics.listKeys().primarySharedKey } } }
}

resource sql 'Microsoft.Sql/servers@2023-08-01-preview' = {
  name: '${prefix}-sql'
  location: location
  properties: { administratorLogin: 'fhiradmin'; administratorLoginPassword: sqlAdministratorPassword; minimalTlsVersion: '1.2'; publicNetworkAccess: 'Disabled' }
}

resource appDb 'Microsoft.Sql/servers/databases@2023-08-01-preview' = { parent: sql; name: 'FhirPlatform'; location: location; sku: { name: 'GP_S_Gen5'; tier: 'GeneralPurpose'; family: 'Gen5'; capacity: 2 } }
resource fhirDb 'Microsoft.Sql/servers/databases@2023-08-01-preview' = { parent: sql; name: 'FHIR'; location: location; sku: { name: 'GP_S_Gen5'; tier: 'GeneralPurpose'; family: 'Gen5'; capacity: 2 } }

resource api 'Microsoft.App/containerApps@2024-03-01' = {
  name: '${prefix}-api'
  location: location
  properties: {
    managedEnvironmentId: containerEnv.id
    configuration: { ingress: { external: false; targetPort: 8080 }; secrets: [] }
    template: { containers: [{ name: 'api'; image: apiImage; env: [{ name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'; value: appInsights.properties.ConnectionString }] }] scale: { minReplicas: 1; maxReplicas: 5 } }
  }
}

resource web 'Microsoft.App/containerApps@2024-03-01' = {
  name: '${prefix}-web'
  location: location
  properties: {
    managedEnvironmentId: containerEnv.id
    configuration: { ingress: { external: true; targetPort: 8080; transport: 'auto' } }
    template: { containers: [{ name: 'web'; image: webImage; env: [{ name: 'Api__BaseUrl'; value: 'https://${api.name}.internal' }] }] scale: { minReplicas: 1; maxReplicas: 3 } }
  }
}

resource worker 'Microsoft.App/containerApps@2024-03-01' = {
  name: '${prefix}-worker'
  location: location
  properties: {
    managedEnvironmentId: containerEnv.id
    configuration: { ingress: { external: false; targetPort: 8080 } }
    template: { containers: [{ name: 'worker'; image: workerImage }] scale: { minReplicas: 1; maxReplicas: 2 } }
  }
}

output webUrl string = 'https://${web.properties.configuration.ingress.fqdn}'
output sqlServerName string = sql.name
output applicationInsightsName string = appInsights.name
