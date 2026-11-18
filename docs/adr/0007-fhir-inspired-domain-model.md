# ADR-0007: FHIR-inspired domain model with generic Observations

- **Status:** Accepted
- **Date:** 2026-11-18

## Context

Lab results and InBody scans are both sets of measured values over time, and the long-term goal is to analyse any metric against any other. HL7 FHIR is the global standard for health data exchange; full FHIR compliance is far too heavy for this project.

## Options considered

1. **Module-specific tables** (`InBodyScan.BodyFatPercent`, `LabResult.Hba1c`, …) — easy at first, but every new metric is a schema change and cross-module analytics needs custom code each time.
2. **Full FHIR server** (e.g. HAPI FHIR, Azure FHIR) — maximal interoperability, but enormous complexity and poor developer ergonomics for a personal app.
3. **FHIR-inspired model** — own simple schema, using FHIR concepts and names where they fit.

## Decision

Use an own domain model **inspired by FHIR** (Patient→Profile, Practitioner, Encounter, Observation, DiagnosticReport, MedicationStatement, DocumentReference — see [domain-model.md](../domain-model.md)).

Measured values are stored as **Observations** (profile, metric, value, unit, reference range, effective time, source) grouped under reports (lab report, body-composition scan). Metrics come from a **catalogue** with canonical units; codes (e.g. LOINC) can be attached later.

## Consequences

- Adding a metric is data, not a schema change.
- Any two metrics can be charted together — the foundation of the Insights module.
- Future import/export to FHIR is a mapping exercise, not a redesign.
- Unit conversion (e.g. mg/dL ↔ mmol/L) must be handled explicitly in the metric catalogue.
