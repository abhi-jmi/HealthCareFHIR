# FHIR Platform Architecture

This repository implements Phase 1 of a production-oriented FHIR R4 platform. Microsoft FHIR Server for Azure is deployed as an internal service and remains the canonical clinical persistence and REST API engine. The custom ASP.NET Core API adds business policies, audit boundaries, operational metadata, and UI-facing contracts. The Blazor Web App calls only the custom API and never reaches into the FHIR server or its SQL database directly.

```text
Blazor Web UI -> Custom ASP.NET Core API -> Application Services -> IFhirResourceClient -> Microsoft FHIR Server -> SQL Server
```

Clean Architecture boundaries:

- `FhirPlatform.Domain` contains pure domain rules such as workflow status transitions.
- `FhirPlatform.Application` orchestrates use cases and depends on abstractions.
- `FhirPlatform.FhirClient` owns FHIR REST transport, serialization, OperationOutcome parsing, and correlation IDs.
- `FhirPlatform.Infrastructure` stores only operational data such as extension registry entries and saved API explorer requests.
- `FhirPlatform.Api` enforces authentication, authorization policies, health checks, Swagger, and ProblemDetails.
- `FhirPlatform.Web` provides the clinical and administrative Blazor UI shell.

Phase 1 scope includes solution setup, Docker Compose, Microsoft FHIR Server connectivity, Keycloak-compatible authentication settings, CapabilityStatement dashboard API, Patient CRUD API, and a basic web UI. Later phases add diagnostics, terminology, validation depth, clinical reasoning, import/export, and hardening.

## Phase 1 continuation

The continuation adds API-backed FHIR resource validation, a reusable OperationOutcome UI panel, and an explicit role-to-permission map. Validation accepts JSON or XML, parses with Firely R4 serializers, checks the expected resource type, calls the FHIR server `$validate` operation when local checks pass, and returns normalized JSON, XML, warnings, errors, and the raw OperationOutcome for UI display and downloads.

Operational database entities remain deliberately non-clinical: extension registry, saved API requests, ingestion job state, and audit correlations. Patient, Observation, DiagnosticReport, and other clinical resources continue to live only in the FHIR server and are accessed by REST.
