# Security

The platform is secure-by-default in design: HTTPS/HSTS outside development, JWT validation, policy authorization, CSRF protection for browser forms, rate-limit and upload-size hardening backlog, PHI-safe structured logging, audit trail, consent-aware access points, and break-glass audit workflow. Software alone does not establish HIPAA, GDPR, or other regulatory compliance; legal, organizational, and operational controls are also required.

## Authorization model

API policies are generated from the explicit permission catalog. A request is authorized when the user has a matching `permission` claim or a mapped platform role whose grants include the requested permission. This keeps UI navigation separate from enforcement and supports Keycloak deployments that emit either role claims or dedicated permission claims.

## PHI-safe audit boundary

The application database stores correlation IDs and FHIR AuditEvent references rather than complete clinical payloads. Clinical audit records should be written to the FHIR server as AuditEvent resources; the relational audit table is for operational correlation and troubleshooting without duplicating PHI.
