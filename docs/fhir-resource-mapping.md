# FHIR Resource Mapping

Clinical resources are not duplicated into application tables. Patient CRUD and CapabilityStatement operations are performed with FHIR REST through `IFhirResourceClient`. The application database stores operational metadata only: extension registry, saved API explorer requests, job state, rule configuration, and audit correlation.
