# HealthCareFHIR

A phased, production-oriented FHIR R4 healthcare platform using Microsoft FHIR Server for Azure as the internal FHIR persistence and API engine.

## Prerequisites

- Docker and Docker Compose
- Optional: .NET 9 SDK for host builds

## Local startup

```bash
docker compose up --build
```

## URLs

- Web UI: http://localhost:5001
- Swagger: http://localhost:5000/swagger
- Health checks: http://localhost:5000/health, `/health/live`, `/health/ready`
- FHIR server: http://localhost:8080
- FHIR metadata: http://localhost:8080/metadata
- Keycloak: http://localhost:8081


## Implemented levels

- Level 1 foundation: FHIR JSON/XML validation, core datatype display components, OperationOutcome display, raw resource viewing, Bundle viewing, and extension registry enforcement.
- Level 2 implementer support: CapabilityStatement-derived conformance dashboard details, terminology search and operations, constrained FHIR API Explorer, API-enforced authorization policies, and AuditEvent creation through FHIR REST.

## Development credentials

Keycloak local users include `admin`, `clinician`, and `auditor` with matching development passwords. These are local-only and must not be used in production.

## Architecture

See `docs/architecture.md`. The UI calls the custom API; the API uses `IFhirResourceClient`; all clinical data access goes through FHIR REST APIs. The application database stores only operational data.

## Production warnings

Do not use local secrets, disabled FHIR auth, or development Keycloak passwords in production. Regulatory compliance requires legal, organizational, and operational controls in addition to this software.

## Implemented FHIR levels

- **Level 1 Foundation:** JSON/XML validation, OperationOutcome handling, datatype display components, extension registry, and recursive custom-extension checks.
- **Level 2 Implementer support:** CapabilityStatement dashboard, terminology operations, OAuth/OIDC authorization policies, AuditEvent creation, and constrained API Explorer.
- **Levels 3-4 resource workflows:** Patient-specific APIs plus generic resource management for administration, clinical, diagnostics, medications, workflow, and financial resources through the custom API layer.
- **Level 5 clinical reasoning:** Deterministic versioned rule execution with GuidanceResponse output, rule execution audit records, and a prominent no-medical-advice safety boundary.

Automated test projects are intentionally omitted per the latest request. Run `dotnet build FhirPlatform.sln` and `docker compose up --build` in a .NET 9/Docker environment before deployment.

## HL7 FHIR R4 level map and schema boundary

The `/fhir-levels` UI route and `/api/fhir-levels` API expose the Level 1-5 map from the HL7 FHIR R4 home page, including Foundation, Implementer Support, Security & Privacy, Conformance, Terminology, Exchange, Administration, Clinical, Diagnostics, Medications, Workflow, Financial and Clinical Reasoning. The application database intentionally excludes HL7 clinical resource tables; canonical resource persistence uses Microsoft FHIR Server's implementation of the HL7 FHIR R4 resource model. See `docs/hl7-fhir-schema-boundary.md`.

## Local database schemas

`docker compose up --build` creates two SQL Server databases: `FHIR` on `fhir-sql` for Microsoft FHIR Server's own schema, and `FhirPlatform` on `application-sql` for operational platform metadata. Microsoft FHIR Server owns and migrates the FHIR schema; custom code must access those resources through FHIR REST APIs only. Use `deploy/sql/002_verify_fhir_server_schema.sql` to inspect the server-created FHIR tables locally.

## Optional HL7 FHIR projection tables

For local schema visibility, the Infrastructure project includes EF Core models and `DbSet` properties for the FHIR resource types in the Level 1-5 catalog. Docker Compose runs `deploy/sql/003_hl7_fhir_projection_schema.sql` after the operational database initialization so these optional projection tables are visible in `FhirPlatform`. Microsoft FHIR Server remains the canonical clinical database; these projection tables are not a replacement for FHIR REST persistence.
