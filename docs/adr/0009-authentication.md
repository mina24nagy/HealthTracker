# ADR-0009: Authentication approach

- **Status:** Proposed — to be decided during M0
- **Date:** 2026-11-18

## Context

Needed from M0: login for the web app, later for the mobile app, invitation of family members (ADR-0006), and eventually public sign-up. Must be free and work fully offline/local at first.

## Options considered

1. **ASP.NET Core Identity (Identity API endpoints)** — built in, free, no external service, works locally; issues bearer tokens usable by web and mobile. Tokens are ASP.NET-specific (not standard OIDC); social logins and MFA need extra work.
2. **ASP.NET Core Identity + OpenIddict** — full standards-based OIDC server in our own app; most learning, most complexity.
3. **Self-hosted Keycloak** — complete OIDC IdP in a container; heavy to run and operate on a free tier.
4. **Managed IdP** (Auth0, Clerk, Microsoft Entra External ID, Supabase Auth) — social login, MFA and security handled for us; free tiers exist; external dependency even for local development.

## Leaning

Option 1 for M0, with application code depending only on an `ICurrentUser` abstraction and claims, so moving to a standards-based OIDC provider (option 2 or 4) before public release is a contained change. Final decision when M0 issue #6 starts.
