# Deployment

Option A deploys the self-hosted Microsoft FHIR Server, custom API, web UI, worker, SQL databases, Keycloak or managed OIDC, and observability into Azure Container Apps or AKS with Azure SQL, Key Vault, Application Insights, private endpoints, and managed identity where possible.

Option B points `IFhirResourceClient` at Azure Health Data Services FHIR service. Because the platform never depends on FHIR SQL internals, migration is configuration and authentication oriented.

## Production Azure template

`deploy/azure/main.bicep` provisions a production-oriented starting point for Azure Container Apps, Log Analytics, Application Insights, and Azure SQL databases for both Microsoft FHIR Server persistence (`FHIR`) and application operational metadata (`FhirPlatform`). The template intentionally expects deployment-time container image names and secure SQL password injection; production rollouts should connect it to Azure Key Vault, private endpoints, managed identities, and the selected FHIR hosting model.
