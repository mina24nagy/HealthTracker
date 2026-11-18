# Domain model

The vocabulary below is the ubiquitous language of the project. Names are borrowed from [HL7 FHIR](https://hl7.org/fhir/) where a concept matches (see [ADR-0007](adr/0007-fhir-inspired-domain-model.md)) — we do **not** implement FHIR, we only align terminology so import/export stays possible later.

## Identity & access

```
Account (login) ──< Membership >── Household ──< Profile
                     role: Owner | Editor | Viewer
```

| Concept | Meaning |
|---|---|
| **Account** | Someone who can log in. |
| **Household** | A group of profiles managed together (a family). |
| **Profile** (≈ FHIR *Patient*) | A person whose health is recorded. May or may not be linked to an Account. Can be marked **deceased** (with date) — kept for family medical history. |
| **Membership / access** | Which Account can see/edit which Profiles, and with what role. |

Rules:
- **Every health record belongs to exactly one Profile.** Authorization is always checked against the Profile.
- An Account sees nothing by default; access is granted per household/profile (least privilege).
- A Profile can later be "claimed" by a family member who creates their own Account (e.g. spouse).

## Health records

| Concept | FHIR analogue | Meaning |
|---|---|---|
| **Provider** | Practitioner / Organization | Doctor, nutritionist, clinic, lab. Contacts, specialty. |
| **Visit** | Encounter | A dated interaction with a provider (incl. weekly nutrition visits). |
| **Observation** | Observation | One measured value at a point in time: metric, value, unit, reference range, source. Used by **both** lab results and InBody scans. |
| **Metric** | (code / LOINC) | Catalogue entry for what is measured (e.g. *Body Fat %*, *HbA1c*), with canonical unit. |
| **Lab Report** | DiagnosticReport | A group of Observations from one lab test, plus the original file. |
| **Body Composition Scan** | DiagnosticReport | A group of Observations from one InBody scan, plus the original sheet. |
| **Nutrition Plan** | NutritionOrder | A plan given by a nutritionist, valid from a date; file + key targets. |
| **Medication** | MedicationStatement | A medication taken over a period: dose, frequency, reason, prescriber. |
| **Side Effect** | AdverseEvent (simplified) | A dated note linked to a medication. |
| **Document** | DocumentReference | Any file (PDF/image) attached to any record. |

## Why "Observation" is the centre

Lab values and InBody metrics are the same shape: *metric + value + unit + time + profile*. Storing them uniformly means any two metrics — e.g. weight (InBody) and fasting glucose (lab) — can be charted together without special code. This is what makes the future **Insights** module cheap.
