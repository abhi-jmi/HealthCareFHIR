# HL7 FHIR R4 Schema Boundary

This platform does not recreate Patient, Observation, DiagnosticReport, Medication, Encounter, Claim, Measure or other HL7 FHIR resources as application-owned relational tables. The canonical schema for those resources is the HL7 FHIR R4 resource model implemented by Microsoft FHIR Server for Azure.

## Persistence rule

- Clinical, administrative, workflow, financial and clinical-reasoning resources are persisted in Microsoft FHIR Server using FHIR REST APIs.
- The application database stores only operational metadata: extension registry entries, saved API Explorer requests, ingestion job state, audit correlations, rule configuration and rule execution audit.
- Application code must not query or modify Microsoft FHIR Server SQL tables directly.
- The `/api/fhir-levels` catalog maps the HL7 levels shown on the FHIR R4 home page to supported UI routes, API routes and FHIR resource types.

## Operational tables

The SQL initialization script intentionally creates only operational platform tables. Any table that would duplicate a FHIR resource, such as `Patients`, `Observations`, `DiagnosticReports`, `Practitioners`, `Claims` or `Measures`, is intentionally excluded.

## Seeing the Microsoft FHIR Server tables locally

HL7 FHIR defines the resource model and REST semantics; it does not define a portable relational table-per-resource schema. In local development, the `fhir-sql` database is initialized as `FHIR`, and Microsoft FHIR Server applies its own SQL persistence schema on startup. That is the correct production boundary for this platform.

To inspect the actual Microsoft FHIR Server SQL schema after `docker compose up --build`, connect to the `fhir-sql` container and run `deploy/sql/002_verify_fhir_server_schema.sql`. Those tables are owned by Microsoft FHIR Server and must not be queried or modified by custom application code.

The separate `application-sql` database is initialized from `deploy/sql/001_app_init.sql` and contains only non-clinical operational tables. This separation is intentional so the platform can later move from self-hosted Microsoft FHIR Server to Azure Health Data Services without rewriting business workflows around SQL internals.
