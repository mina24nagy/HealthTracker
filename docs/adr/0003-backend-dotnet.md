# ADR-0003: Backend on ASP.NET Core / .NET 10 LTS

- **Status:** Accepted
- **Date:** 2026-11-18

## Context

The backend needs to serve a web app now and a mobile app later, enforce per-profile authorization on sensitive data, and be cheap to host. The developer is experienced in .NET and open to other stacks.

## Options considered

1. **ASP.NET Core (.NET 10 LTS)** — developer's strongest stack; excellent performance, mature EF Core, first-class OpenAPI, Identity, and testing tools; long-term support until Nov 2028.
2. **Node.js (NestJS / Fastify)** — same language as the frontend, but loses the developer's depth in .NET for the part where correctness matters most.
3. **Backend-as-a-Service (Supabase / Firebase)** — fastest start, but domain logic ends up in the client or in database policies, and vendor lock-in is high.

## Decision

**ASP.NET Core on .NET 10 (LTS)** with Minimal APIs organised per module, OpenAPI for the contract (used to generate the TypeScript client, see ADR-0005), FluentValidation-style validation, structured logging, and xUnit + Testcontainers for tests.

New learning is concentrated in the frontend/mobile stack (React, Expo) and in architecture practice, while the backend stays in a stack where senior-level depth is already available.

## Consequences

- Requires the .NET 10 SDK locally (currently .NET 9 is installed).
- Hosts well on free/cheap tiers (container or App Service).
- Upgrade path: move to the next LTS (.NET 12) when it ships.
