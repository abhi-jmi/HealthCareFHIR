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
