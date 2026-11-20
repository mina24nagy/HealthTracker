# Architecture overview

HealthTracker is a **modular monolith** whose modules are organised internally by **vertical slices**, with **ports & adapters only at infrastructure edges**.

> One process, split into modules by business capability. Each module owns its data and exposes only its contracts, organises its code by use case, and puts interfaces only at true infrastructure boundaries. Tests enforce the boundaries, so they hold as the codebase grows.

Decisions behind this: [ADR-0002](../adr/0002-modular-monolith.md) (modular monolith), [ADR-0004](../adr/0004-postgresql-ef-core.md) (schema per module), [ADR-0008](../adr/0008-local-first-cloud-ready.md) (abstractions at infrastructure edges).

---

## Level 1 — Modular monolith (across the app)

One deployable process, split into modules by **business capability** instead of by technical layer.

```
                ┌──────────────────────── HealthTracker.Api (one process) ────────────────────────┐
  HTTP ────────►│  Host (Program.cs): composition, auth, errors, OpenAPI, health, logging          │
                │                                                                                   │
                │   ┌────────────┐  ┌────────────┐  ┌────────────┐  ┌─────────────┐                 │
                │   │  Profiles  │  │ Nutrition  │  │    Labs    │  │ Medications │   …             │
                │   └─────┬──────┘  └─────┬──────┘  └─────┬──────┘  └──────┬──────┘                 │
                │         └───────────────┴── SharedKernel ┴────────────────┘                        │
                └─────────┬─────────────────────────────────────────────────────────────────────────┘
                          ▼
              PostgreSQL — one database, one schema per module (profiles.*, nutrition.*, labs.* …)
```

### Project dependencies

```mermaid
graph LR
  Api[HealthTracker.Api<br/>host] --> Profiles[Modules.Profiles]
  Api --> Nutrition[Modules.Nutrition<br/>M1]
  Api --> SK[SharedKernel]
  Profiles --> SK
  Nutrition --> SK
  Nutrition -. only .Contracts .-> Profiles
  Tests[ArchitectureTests] --> Api
```

### Rules and how they are enforced

| Rule | Enforced by |
|---|---|
| Each module is its own project; everything inside is `internal` | C# compiler |
| A module's public surface is only its `IModule` entry point + a `.Contracts` namespace | Architecture test `Module_public_surface_is_entry_point_and_contracts_only` |
| Modules use other modules only through their Contracts | Architecture test `Modules_only_use_other_modules_through_their_contracts` |
| Modules never depend on the host; SharedKernel depends on nothing project-specific | Architecture tests |
| The boundary detector itself works (so the rules can't pass vacuously) | Architecture test `Boundary_detector_reports_a_known_dependency` |
| Each module owns its tables (own `DbContext` + schema); no cross-module joins | Convention + review (from #4) |
| The host only composes: lists modules and adds cross-cutting concerns | `Program.cs` |

Tests: [`apps/api/tests/HealthTracker.ArchitectureTests`](../../apps/api/tests/HealthTracker.ArchitectureTests/ModuleBoundaryTests.cs).

### Why not the alternatives

- **Microservices** — network calls, distributed data, multiple deployments and observability, for one developer and a few users. A modular monolith keeps the same boundaries at a fraction of the cost; a module can be extracted later along an existing seam.
- **Layered monolith** (`Controllers/`, `Services/`, `Repositories/` for the whole app) — layers cut across every feature, so unrelated capabilities tangle over time and boundaries are only conventions.

---

## Level 2 — Vertical slices (inside a module)

Inside a module, code is grouped by **use case**, not by layer.

```
Modules/Nutrition/HealthTracker.Modules.Nutrition/
  NutritionModule.cs              IModule: registers services, maps endpoints
  Features/
    RecordInBodyScan.cs           endpoint + request/response + validation + handler, together
    GetBodyCompositionTrend.cs
    UploadNutritionPlan.cs
  Domain/                         entities and rules shared by several features
  Persistence/                    DbContext, EF configurations, migrations
  Contracts/                      the only types other modules may use (queries, events)
```

The first slice is [`Features/GetProfile.cs`](../../apps/api/src/Modules/Profiles/HealthTracker.Modules.Profiles/Features/GetProfile.cs): route mapping, response type and handler in one file. Adding a feature mostly means **adding one file**.

### Compared with Clean / Onion architecture

| | Clean / Onion (per app) | Vertical slices (per module) |
|---|---|---|
| Unit of organisation | Technical layer | Use case |
| A new feature touches | Controller, service, interface, repository, DTO, mapper… | Mostly one slice (+ domain if needed) |
| Abstractions | Everywhere (`IRepository<T>`, `IService`) | Only where they pay off |
| Main risk | Ceremony, pass-through layers | Duplication between slices if undisciplined |

The Clean Architecture *principle* — dependencies point inward, the domain does not know about infrastructure — still applies **inside a module whose domain is rich** (e.g. Labs: units, reference ranges, conversions get a real `Domain/` with behaviour). Simple CRUD-like modules don't need it. Structure scales with complexity instead of being imposed uniformly.

---

## Level 3 — Ports & adapters (only at infrastructure edges)

| Port | Adapter now | Adapter later |
|---|---|---|
| `IFileStorage` | Local disk | Cloudflare R2 / Azure Blob |
| `ICurrentUser` | ASP.NET Core Identity claims | Another OIDC provider ([ADR-0009](../adr/0009-authentication.md)) |
| `TimeProvider` (.NET built-in) | System clock | Fake clock in tests |
| Notification sender | Console / log | Email provider |

EF Core's `DbContext` is used directly inside slices — it already is a unit of work + repositories; a generic repository on top would add ceremony without adding swappability.

---

## Communication between modules (later milestones)

1. **Synchronous queries via Contracts** — e.g. Nutrition asks Profiles "may this account access profile X?" through an interface in `Profiles.Contracts`.
2. **In-process domain events for side effects** — e.g. Labs publishes `LabResultRecorded`, Insights reacts. Neither knows the other's internals; if a module is ever extracted, events become messages on a queue.

---

## Request flow (today)

```
GET /api/profiles/{profileId}
  → ExceptionHandler / StatusCodePages middleware        (errors → RFC 9457 ProblemDetails)
  → /api route group (host)  → /profiles group (Profiles module)
  → ProfileId.TryParse                                   (invalid / empty id → 400)
  → GetProfile.Handle                                    (placeholder → 404 until #7)
```

Cross-cutting concerns live in the host (`apps/api/src/HealthTracker.Api/Infrastructure/`): `GlobalExceptionHandler`, `HealthEndpoints` (`/health/live`, `/health/ready`), `OpenApiSetup` (OpenAPI + Scalar in Development), `LoggingSetup` (JSON console outside Development).
