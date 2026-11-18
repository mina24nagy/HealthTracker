# ADR-0005: React web + Expo mobile in a pnpm monorepo

- **Status:** Accepted
- **Date:** 2026-11-18

## Context

The product starts as a web app, then needs Android (the developer's phone) and iOS. Future features include Android Health Connect and Apple HealthKit sync, which require a native-capable mobile app. The web UI is data-heavy (forms, tables, charts).

## Options considered

1. **React web now + Expo (React Native) later** — best-in-class web ecosystem for tables/charts; Expo has mature Health Connect / HealthKit libraries; TypeScript types, validation and API client are shared. Two UI layers to maintain.
2. **Expo for everything (React Native Web)** — one UI codebase, but the web experience for dense data UIs is weaker.
3. **Flutter** — one codebase for all platforms, but Flutter web is weaker for this kind of app and Dart shares nothing with the backend or web ecosystem.
4. **Blazor + .NET MAUI** — all C#, least new learning, but smaller ecosystem and weaker mobile health integrations; misses the learning goal.

## Decision

**React + TypeScript (Vite) for web**, **Expo** for mobile from M7, in a **pnpm workspace monorepo**:

```
apps/
  api/          ASP.NET Core solution
  web/          React + Vite
  mobile/       Expo (from M7)
packages/
  api-client/   TypeScript client generated from the API's OpenAPI spec
  shared/       shared types, validation schemas, utilities
docs/
```

Specific libraries (UI kit, data fetching, forms, charts) are chosen in M0 and recorded in a follow-up ADR.

## Consequences

- The OpenAPI spec is the contract between backend and all clients; breaking it breaks the generated client at compile time (good).
- Business logic stays in the API; clients share only types, validation and API access.
- Requires Node.js 22+ and pnpm locally.
