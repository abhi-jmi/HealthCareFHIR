@description('FHIR Platform environment name')
param environmentName string = 'dev'
resource rgMarker 'Microsoft.Resources/tags@2021-04-01' = { name: 'default' properties: { tags: { workload: 'fhir-platform', environment: environmentName } } }
