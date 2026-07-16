# Deployment

Option A deploys the self-hosted Microsoft FHIR Server, custom API, web UI, worker, SQL databases, Keycloak or managed OIDC, and observability into Azure Container Apps or AKS with Azure SQL, Key Vault, Application Insights, private endpoints, and managed identity where possible.

Option B points `IFhirResourceClient` at Azure Health Data Services FHIR service. Because the platform never depends on FHIR SQL internals, migration is configuration and authentication oriented.
