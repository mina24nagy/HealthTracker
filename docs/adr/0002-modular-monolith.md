# ADR-0002: Modular monolith architecture

- **Status:** Accepted
- **Date:** 2026-11-18

## Context

The app has several distinct health modules (Nutrition, Providers & Visits, Labs, Medications, Documents, Insights) and a single developer. It must run for free on one machine at first, but should be able to grow.

## Options considered

1. **Layered monolith** (Controllers / Services / Repositories across everything) — simple, but modules blur together and coupling grows invisibly.
2. **Microservices** — strong boundaries, but heavy operational cost (deployments, networking, distributed data) with no matching benefit for one developer and a handful of users.
3. **Modular monolith** — one deployable, internally split into modules with explicit boundaries.

## Decision

A **modular monolith**. Each module owns its domain, persistence (own EF Core `DbContext` / schema) and endpoints, organised as vertical slices. Modules talk to each other only through public contracts (interfaces or in-process events), never by reaching into another module's tables. A shared kernel holds cross-cutting primitives (profile id, current user, clock, file storage abstraction). Architecture tests enforce the boundaries.

## Consequences

- One process to build, test, deploy and debug; cheap to host.
- Boundaries are real but cheap to adjust while the domain is still being discovered.
- A module could be extracted into a service later if it ever needs to scale independently.
- Requires discipline: boundary rules must be enforced by tests, not by convention.
