# Vision

## Problem

Health information for a family is scattered: lab PDFs in Google Drive folders, prescriptions on paper, doctor phone numbers in chats, InBody printouts in a drawer, nutrition plans in WhatsApp. When a doctor asks "when did this value start rising?" or "which medication gave you that reaction?", the answer takes hours to reconstruct — or is lost.

No existing free or paid app covers the whole picture **for multiple family members**, lets the owner customise it freely, and makes the data analysable over years.

## Product statement

**HealthTracker is a personal health record for a household.** It stores every health event — visits, providers, lab results, medications, body measurements, nutrition plans, documents — per family member, on one timeline, and turns it into trends and insights.

## Users

| Persona | Need |
|---|---|
| **Household manager** (primary user) | Records and reviews health data for themself and family members |
| **Family member with own login** (e.g. spouse) | Views/edits their own record, optionally others' |
| **Managed member** (mother, children, deceased relatives) | Has a record but no login; managed by someone else |
| **Future: friends / public users** | Same, for their own households |

## Core capabilities (MVP scope)

1. **Profiles** — one record per person in the household, including deceased relatives (family medical history matters for hereditary risk).
2. **Nutrition** — weekly nutritionist visits, InBody scans as structured metrics, nutrition plans.
3. **Providers & Visits** — doctors, clinics, labs, contacts; what happened at each visit.
4. **Lab Results** — panels and individual values with units and reference ranges, plus the original report file.
5. **Medications** — current and past medications, dosage, reason, prescriber, side-effect journal.
6. **Timeline & Insights** — everything on one timeline; trend charts for any measured value.

## Later

- Import of existing records (e.g. Google Drive folders), with AI extraction of values from lab PDFs and InBody sheets
- Mobile app (Android first, then iOS) and Android Health Connect / Apple HealthKit sync
- Medication reminders, appointment reminders
- Shareable summary / PDF export for a doctor visit
- Public release (web + stores)

## Principles

1. **Privacy first.** Health data is the most sensitive personal data there is. Auth everywhere, least privilege between family members, encryption at rest, full data export. No real data in this public repo.
2. **Data over screens.** A sound domain model (FHIR-inspired) matters more than UI polish; good data makes analytics cheap later.
3. **Start free and local, stay cloud-ready.** Runs on a laptop with Docker; every infrastructure dependency sits behind an abstraction that can be swapped for a paid cloud service.
4. **Every step documented.** Decisions as ADRs, work as issues + PRs, milestones as devlog entries and tagged releases.
5. **Production standards on a side project.** Tests, CI, code review discipline — this project is also for sharpening senior engineering skills.

## Non-goals (for now)

- Being a medical device or giving medical advice/diagnosis
- Integration with hospital systems (but the data model keeps the door open)
- Multi-tenant SaaS billing
