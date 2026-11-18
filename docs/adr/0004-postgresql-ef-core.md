# ADR-0004: PostgreSQL with EF Core

- **Status:** Accepted
- **Date:** 2026-11-18

## Context

The data is relational (profiles, providers, visits, medications) with a large time-series-like part (observations) that must support analytics. Must be free locally and have free cloud tiers.

## Options considered

1. **SQLite** — zero setup, but weak for concurrent multi-user access and painful to move to the cloud later.
2. **SQL Server** — familiar to .NET developers, but free cloud tiers are limited and hosting cost is higher.
3. **PostgreSQL** — free, runs in Docker, generous free cloud tiers (Neon, Supabase, Azure Flexible Server trial), strong analytics (window functions, JSONB, optional TimescaleDB later).
4. **Document DB (MongoDB / Firestore)** — flexible, but the domain is clearly relational and analytics across records is harder.

## Decision

**PostgreSQL** via **EF Core** (Npgsql provider), one database with **one schema per module** (e.g. `nutrition`, `labs`) to make module ownership visible. Migrations are per module.

## Consequences

- Docker is required for local development.
- Schema-per-module keeps the option to split a module's data out later.
- Analytics queries over observations can use plain SQL when EF Core is not a good fit.
