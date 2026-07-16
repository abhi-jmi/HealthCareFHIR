# HL7 FHIR R4 Schema Boundary

This platform does not recreate Patient, Observation, DiagnosticReport, Medication, Encounter, Claim, Measure or other HL7 FHIR resources as application-owned relational tables. The canonical schema for those resources is the HL7 FHIR R4 resource model implemented by Microsoft FHIR Server for Azure.

## Persistence rule

- Clinical, administrative, workflow, financial and clinical-reasoning resources are persisted in Microsoft FHIR Server using FHIR REST APIs.
- The application database stores only operational metadata: extension registry entries, saved API Explorer requests, ingestion job state, audit correlations, rule configuration and rule execution audit.
- Application code must not query or modify Microsoft FHIR Server SQL tables directly.
- The `/api/fhir-levels` catalog maps the HL7 levels shown on the FHIR R4 home page to supported UI routes, API routes and FHIR resource types.

## Operational tables

The SQL initialization script intentionally creates only operational platform tables. Any table that would duplicate a FHIR resource, such as `Patients`, `Observations`, `DiagnosticReports`, `Practitioners`, `Claims` or `Measures`, is intentionally excluded.
