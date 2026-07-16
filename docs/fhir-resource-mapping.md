# FHIR Resource Mapping

Clinical resources are not duplicated into application tables. Patient CRUD and CapabilityStatement operations are performed with FHIR REST through `IFhirResourceClient`. The application database stores operational metadata only: extension registry, saved API explorer requests, job state, rule configuration, and audit correlation.

## Level 1 custom extension enforcement

Standard HL7 extension URLs under `http://hl7.org/fhir/StructureDefinition/` are allowed as standards-based extensions. Custom extension URLs must be active in the application extension registry for the applicable resource type before validation or business workflows accept them. The application stores only registry metadata; extension values remain inside the canonical FHIR resources persisted by the FHIR server.

## Level 1 completion scope

Level 1 now includes typed display components for Coding, CodeableConcept, Identifier, HumanName, Address, ContactPoint, Quantity, Period, Attachment, Reference, Narrative, Meta, Extension, Bundle, OperationOutcome, and raw JSON/XML. Custom extension enforcement recursively inspects resource, datatype, backbone-element, nested extension, and modifier-extension locations before delegating to the FHIR server `$validate` operation.

## Levels 3-5 implementation coverage

Administration, clinical, diagnostics, medications, workflow, financial, and clinical reasoning resources are accessed through the generic `/api/resources/{resourceType}` API and the matching Blazor resource pages. The custom platform stores only operational metadata; canonical clinical and administrative resources remain in Microsoft FHIR Server and are accessed exclusively through FHIR REST APIs.

FHIR Bundle ingestion is exposed at `POST /api/ingestion/fhir-bundle`. It validates bundle type, basic Patient/Observation structure, UCUM-system presence for Quantity observations, records idempotent ingestion job state, appends Provenance, and persists through FHIR transaction semantics.

Clinical reasoning is exposed at `/api/clinical-reasoning`. Rules are versioned JSON configurations in the operational database. The included HbA1c review rule emits only a configured review message in a GuidanceResponse, records input/output references, and explicitly avoids diagnosis, medication recommendations, and generative clinical decision-making.
