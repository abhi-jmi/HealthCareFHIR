# HealthCareFHIR

A phased, production-oriented FHIR R4 healthcare platform using Microsoft FHIR Server for Azure as the internal FHIR persistence and API engine.

## Prerequisites

- Docker and Docker Compose
- Optional: .NET 9 SDK for host builds and tests

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

## Development credentials

Keycloak local users include `admin`, `clinician`, and `auditor` with matching development passwords. These are local-only and must not be used in production.

## Architecture

See `docs/architecture.md`. The UI calls the custom API; the API uses `IFhirResourceClient`; all clinical data access goes through FHIR REST APIs. The application database stores only operational data.

## Test commands

```bash
dotnet test
```

In this environment, the .NET SDK may not be installed on the host; Docker builds use SDK images.

## Production warnings

Do not use local secrets, disabled FHIR auth, or development Keycloak passwords in production. Regulatory compliance requires legal, organizational, and operational controls in addition to this software.
