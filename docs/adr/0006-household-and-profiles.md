# ADR-0006: Household & profiles as the ownership model

- **Status:** Accepted
- **Date:** 2026-11-18

## Context

The app records health data for a whole family: the primary user, a spouse (who may want their own login), a parent managed by the user, future children, and deceased relatives whose history matters for hereditary risk. Retrofitting multi-person support into a single-user data model is very costly, so it must be decided before the first module.

## Options considered

1. **Data owned by the logged-in user** — simplest, but cannot represent family members without logins, and sharing is impossible.
2. **Data owned by a Profile, access granted to Accounts** — separates "who logs in" from "whose health this is".

## Decision

Separate **Account** (can log in) from **Profile** (a person whose health is recorded):

- A **Household** groups Profiles. Accounts join a household with a role: **Owner**, **Editor**, **Viewer**. Per-profile overrides are possible (e.g. a spouse sees only their own profile).
- Every health record has a non-null `ProfileId`. Authorization is enforced centrally (policy + query filter), never ad hoc per endpoint.
- A Profile can be **managed** (no account), **linked** to an Account (claimed later via invitation), and marked **deceased** with a date.
- Least privilege by default: an Account sees nothing it was not granted.

## Consequences

- Every query is profile-scoped — implemented once as a cross-cutting concern and covered by tests.
- Supports the public-release path: each new user simply creates their own household.
- Invitation and account-linking flows are extra work, scheduled after the basics (not required for M1).
