# Devlog 000 — Project kickoff

**Date:** 2026-11-18 · **Milestone:** M0 (planning)

## The idea

One place for a family's health history: doctors and their contacts, visits, lab results over the years, medications with side-effect notes, and the weekly nutritionist visits with InBody scans and nutrition plans — for me, my spouse, my mother, future children, and my late father's records, which matter as family medical history. Today these live in Google Drive folders, paper and chats.

## Goals of the project

1. Use it in my own life, with features no existing app offers.
2. Sharpen senior software-engineering skills — treat it as a real product.
3. Possibly publish it (web + stores) for friends, maybe as a source of income.
4. Document every feature and milestone on GitHub.
5. Plan and investigate architecture before writing code.

## Decisions made today

| Decision | ADR |
|---|---|
| Record decisions as ADRs | [0001](../adr/0001-record-architecture-decisions.md) |
| Modular monolith, not microservices | [0002](../adr/0002-modular-monolith.md) |
| ASP.NET Core / .NET 10 LTS backend | [0003](../adr/0003-backend-dotnet.md) |
| PostgreSQL + EF Core, schema per module | [0004](../adr/0004-postgresql-ef-core.md) |
| React web now, Expo mobile later, pnpm monorepo | [0005](../adr/0005-frontend-react-expo-monorepo.md) |
| Household → Profiles ownership model (family from day one) | [0006](../adr/0006-household-and-profiles.md) |
| FHIR-inspired model; lab values and InBody metrics are both Observations | [0007](../adr/0007-fhir-inspired-domain-model.md) |
| Local-first with Docker, cloud-ready abstractions | [0008](../adr/0008-local-first-cloud-ready.md) |
| Auth — still open | [0009](../adr/0009-authentication.md) |
| Issues → PRs → milestone releases, conventional commits, devlog | [0010](../adr/0010-development-workflow.md) |

## Why these, in one paragraph

Backend stays in .NET where I have depth, because correctness and authorization over sensitive data matter most there; the new learning goes into React/Expo and into architecture practice. Family support had to be in the data model before the first module — retrofitting it later would touch every table. Treating lab values and body-composition metrics uniformly as Observations is the bet that makes future analytics cheap.

## Next

Finish M0: monorepo scaffolding, API and web skeletons, Docker Compose, CI, auth, and household/profiles. Then M1 — Nutrition & InBody, because new data arrives every week.
