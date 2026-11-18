# Roadmap

Each milestone is a GitHub Milestone with its own issues and PRs, ends with a [devlog](devlog/) entry, and ships as a tagged release. Each milestone after M0 must deliver something **usable**, not just a technical layer.

Order is driven by personal value: Nutrition first because new InBody data arrives every week.

| # | Milestone | Outcome | Status |
|---|---|---|---|
| M0 | Foundation | Repo, CI, API + web skeletons, local stack, auth, family profiles | 🟡 Planning |
| M1 | Nutrition & InBody | Log weekly nutrition visits, InBody scans and plans; see body-composition trends | ⚪ |
| M2 | Providers & Visits | Doctors, clinics, labs and visit history per profile | ⚪ |
| M3 | Lab Results | Lab panels and values with reference ranges, report files, trend charts | ⚪ |
| M4 | Medications | Current/past medications, side-effect journal | ⚪ |
| M5 | Timeline & Insights | Cross-module timeline, dashboard, compare any metrics | ⚪ |
| M6 | First deploy | Free-tier cloud deployment, beta for family/friends | ⚪ |
| M7 | Android app | Expo app for Android (iOS-ready) | ⚪ |
| M8 | Import & AI extraction | Bulk import of existing files (Google Drive); extract values from lab PDFs / InBody sheets | ⚪ |

**Later / ideas backlog:** Health Connect & HealthKit sync · reminders · doctor-visit summary export · family medical history view · iOS release · public launch.

---

## M0 — Foundation ([milestone](https://github.com/mina24nagy/HealthTracker/milestone/1))

1. [#1](https://github.com/mina24nagy/HealthTracker/issues/1) Repository setup: docs, ADRs, templates, conventions ✅
2. [#2](https://github.com/mina24nagy/HealthTracker/issues/2) Monorepo scaffolding: `pnpm` workspace, `apps/api`, `apps/web`, `packages/api-client`
3. [#3](https://github.com/mina24nagy/HealthTracker/issues/3) API skeleton: .NET 10, modular monolith layout, health checks, OpenAPI, structured logging
4. [#4](https://github.com/mina24nagy/HealthTracker/issues/4) Local stack: Docker Compose with PostgreSQL; EF Core migrations
5. [#5](https://github.com/mina24nagy/HealthTracker/issues/5) Web skeleton: React + TS + Vite, routing, UI kit, API client generated from OpenAPI
6. [#6](https://github.com/mina24nagy/HealthTracker/issues/6) Authentication (see ADR-0009)
7. [#7](https://github.com/mina24nagy/HealthTracker/issues/7) Profiles & household: create household, add profiles (self, spouse, mother, deceased relatives), access roles
8. [#8](https://github.com/mina24nagy/HealthTracker/issues/8) CI: GitHub Actions — build, lint, test for API and web
9. [#9](https://github.com/mina24nagy/HealthTracker/issues/9) Testing baseline: unit + integration (Testcontainers) + one Playwright smoke test

**Suggested order:** #2 → #3 → #4 → #8 → #9 → #5 → #6 → #7 (get CI green early so every later PR is checked).

## M1 — Nutrition & InBody (draft scope, refined when we start)

- Observation core (metric catalogue, units) — shared with M3 later
- Document attachments core (local file storage behind `IFileStorage`)
- Nutritionist visit log (date, provider, notes)
- InBody scan entry (structured metrics + attached scan image/PDF)
- Nutrition plan (attached file + key targets)
- Body-composition trend charts per profile
