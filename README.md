# HealthTracker

A personal & family **health record** app: doctors, visits, lab results, medications, nutrition / InBody scans — all in one place, with analytics over time.

> Status: **Planning (M0 – Foundation)**. See the [roadmap](docs/roadmap.md).

## Why

Existing apps track pieces of health (steps, calories, pills) but not the whole picture across a family: which doctor said what, how lab values evolved over years, which medication caused which side effect, and how body composition changes under a nutrition plan. HealthTracker is built to hold all of it, for every member of a household, and make it analysable.

This is also a deliberately documented side project: every decision is recorded as an [ADR](docs/adr/README.md), every milestone has a [devlog entry](docs/devlog/), and every feature lands as an issue + pull request.

## Tech stack (planned)

| Layer          | Choice                                          | Decision                                                                                   |
| -------------- | ----------------------------------------------- | ------------------------------------------------------------------------------------------ |
| Backend        | ASP.NET Core on .NET 10 (LTS), modular monolith | [ADR-0002](docs/adr/0002-modular-monolith.md), [ADR-0003](docs/adr/0003-backend-dotnet.md) |
| Database       | PostgreSQL + EF Core                            | [ADR-0004](docs/adr/0004-postgresql-ef-core.md)                                            |
| Web            | React + TypeScript (Vite)                       | [ADR-0005](docs/adr/0005-frontend-react-expo-monorepo.md)                                  |
| Mobile (later) | Expo (React Native)                             | [ADR-0005](docs/adr/0005-frontend-react-expo-monorepo.md)                                  |
| Hosting        | Local-first (Docker Compose), cloud-ready       | [ADR-0008](docs/adr/0008-local-first-cloud-ready.md)                                       |

## Documentation

- [Vision](docs/vision.md) — the problem, users and principles
- [Roadmap](docs/roadmap.md) — milestones M0…M8
- [Domain model](docs/domain-model.md) — the core concepts and vocabulary
- [Architecture overview](docs/architecture/overview.md) — modular monolith, vertical slices, enforced boundaries
- [Architecture Decision Records](docs/adr/README.md)
- [Devlog](docs/devlog/) — the build story, milestone by milestone
- [Changelog](CHANGELOG.md)

## Privacy rule for this repository

This repo is public. **No real health data is ever committed** — only synthetic fixtures. Real data lives in the running app's database and file storage, which are git-ignored.

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (pinned in `global.json`)
- [Node.js](https://nodejs.org/) 22+
- [pnpm](https://pnpm.io/) (version pinned in `package.json` → `packageManager`)
- [Docker](https://www.docker.com/) (Docker Desktop on Windows/macOS)

### Setup

```bash
pnpm install
dotnet tool restore   # dotnet-ef, pinned in dotnet-tools.json
pnpm build            # TypeScript packages + .NET solution
pnpm test
pnpm lint             # ESLint + Prettier + dotnet format
```

### Run locally

```bash
docker compose up -d --build    # API (http://localhost:5080) + PostgreSQL
docker compose up -d postgres   # or: only the database, then run the API from your IDE / `dotnet run`
docker compose down             # stop (data is kept in the postgres-data volume)
```

In Development the API applies every module's EF Core migrations at startup. Useful URLs: `/health/ready`, `/scalar` (API reference), `/openapi/v1.json`.

The defaults in `compose.yaml` are local-only development credentials (ports bound to `127.0.0.1`); override them in a git-ignored `.env` (see [.env.example](.env.example)).

### Database migrations (one schema and migration set per module)

```bash
dotnet ef migrations add <Name> \
  --project apps/api/src/Modules/Profiles/HealthTracker.Modules.Profiles \
  --startup-project apps/api/src/HealthTracker.Api \
  --context ProfilesDbContext --output-dir Persistence/Migrations
```

### Backup and restore (local data)

```powershell
./scripts/db-backup.ps1                                   # → backups/healthtracker-<timestamp>.dump (git-ignored)
./scripts/db-restore.ps1 -Path ./backups/<file>.dump      # destructive; asks for confirmation
```

### Repository layout

```
apps/
  api/            ASP.NET Core solution (HealthTracker.slnx)
  web/            React + Vite web app        (#5)
  mobile/         Expo app                    (M7)
packages/
  api-client/     TS client generated from OpenAPI (#5)
  shared/         Types and helpers shared by the clients
docs/             Vision, roadmap, ADRs, devlog, architecture
scripts/          Local database backup / restore
compose.yaml      Local stack: API + PostgreSQL
```

## License

Copyright © 2026 Mina Nagy. All rights reserved — the source is public for reference only. See [LICENSE](LICENSE).
