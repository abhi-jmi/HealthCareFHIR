# FHIR Resource Mapping

Clinical resources are not duplicated into application tables. Patient CRUD and CapabilityStatement operations are performed with FHIR REST through `IFhirResourceClient`. The application database stores operational metadata only: extension registry, saved API explorer requests, job state, rule configuration, and audit correlation.

## Level 1 custom extension enforcement

Standard HL7 extension URLs under `http://hl7.org/fhir/StructureDefinition/` are allowed as standards-based extensions. Custom extension URLs must be active in the application extension registry for the applicable resource type before validation or business workflows accept them. The application stores only registry metadata; extension values remain inside the canonical FHIR resources persisted by the FHIR server.
