# Architecture Decision Records

An ADR records one significant decision: its context, the options considered, what we chose and the consequences. ADRs are immutable once accepted — a changed decision gets a **new** ADR that supersedes the old one.

Statuses: `Proposed` → `Accepted` → (`Deprecated` | `Superseded by ADR-NNNN`)

| # | Title | Status |
|---|---|---|
| [0001](0001-record-architecture-decisions.md) | Record architecture decisions | Accepted |
| [0002](0002-modular-monolith.md) | Modular monolith architecture | Accepted |
| [0003](0003-backend-dotnet.md) | Backend on ASP.NET Core / .NET 10 LTS | Accepted |
| [0004](0004-postgresql-ef-core.md) | PostgreSQL with EF Core | Accepted |
| [0005](0005-frontend-react-expo-monorepo.md) | React web + Expo mobile in a pnpm monorepo | Accepted |
| [0006](0006-household-and-profiles.md) | Household & profiles as the ownership model | Accepted |
| [0007](0007-fhir-inspired-domain-model.md) | FHIR-inspired domain model with generic Observations | Accepted |
| [0008](0008-local-first-cloud-ready.md) | Local-first deployment, cloud-ready abstractions | Accepted |
| [0009](0009-authentication.md) | Authentication approach | Proposed |
| [0010](0010-development-workflow.md) | Development & documentation workflow | Accepted |

New ADRs: copy [template.md](template.md).
