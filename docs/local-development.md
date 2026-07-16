# Local Development

Prerequisites: Docker, Docker Compose, and optionally the .NET 9 SDK for host builds.

Run the stack:

```bash
docker compose up --build
```

Local URLs:

- Web UI: http://localhost:5001
- Custom API Swagger: http://localhost:5000/swagger
- API health: http://localhost:5000/health
- FHIR server metadata: http://localhost:8080/metadata
- Keycloak: http://localhost:8081

Development passwords in `docker-compose.yml` are local-only defaults and must be overridden for shared or production environments.

## Phase 1 API routes

- `GET /api/patients` searches Patient resources through FHIR REST.
- `GET /api/patients/{id}` reads a Patient by logical ID.
- `POST /api/patients` creates a Patient through FHIR REST.
- `PUT /api/patients/{id}` updates a Patient through FHIR REST.
- `GET /api/conformance/dashboard` reads the FHIR server CapabilityStatement.
- `POST /api/fhir/validation` validates FHIR JSON or XML and returns an OperationOutcome projection.

## Level 1 UI routes

- `/validation` validates pasted FHIR JSON/XML through `POST /api/fhir/validation`, displays OperationOutcome issues, and exposes formatted JSON/XML download links.
- `/administration/extensions` manages the custom extension registry used to reject unknown custom extensions during validation and business workflows.

## Level 2 API routes

- `GET /api/conformance/dashboard/details` returns CapabilityStatement-derived resource, interaction, search-parameter, operation and expected-resource gap details.
- `GET /api/terminology/code-systems`, `/value-sets`, and `/concept-maps` search terminology resources through FHIR REST.
- `POST /api/terminology/value-set/$expand`, `/value-set/$validate-code`, and `/concept-map/$translate` delegate terminology operations to the configured FHIR server.
- `POST /api/fhir-explorer/execute` executes constrained read/search requests against an allow-list of FHIR resource types.
- `POST /api/audit/events` writes AuditEvent resources through FHIR REST.

## Inspecting the FHIR server SQL schema

`docker compose up --build` now runs `fhir-sql-init` to create the `FHIR` database before the Microsoft FHIR Server starts. Microsoft FHIR Server then creates and migrates its own SQL schema. To view those server-owned tables, run the verification SQL in `deploy/sql/002_verify_fhir_server_schema.sql` against the `fhir-sql` container after the FHIR server is healthy.

The application database is initialized separately by `application-sql-init` using `deploy/sql/001_app_init.sql`; it intentionally contains only operational platform tables, not duplicated FHIR resource tables.
