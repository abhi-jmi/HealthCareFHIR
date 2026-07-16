# Testing

The current repository intentionally does not include test projects because the latest implementation request was to avoid adding test projects. Production teams should add automated checks in CI before regulated deployment, including unit tests, API integration tests, FHIR server integration tests, authorization tests, ingestion idempotency tests, and smoke tests for `docker compose up --build`.
