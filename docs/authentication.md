# Authentication and Authorization

The API uses JWT bearer authentication and policy-based authorization. The Web project is configured for OpenID Connect with cookie sessions. Configuration supports Keycloak-compatible authority, metadata, client, audience, HTTPS metadata, and role claim settings.

Roles include SystemAdministrator, FhirAdministrator, Clinician, DiagnosticReviewer, Patient, Auditor, and IntegrationClient. Permissions are represented as explicit claims such as `Patient.Read`, `Patient.Write`, `Conformance.Manage`, and `Audit.Read`; UI visibility is not treated as an authorization boundary.
