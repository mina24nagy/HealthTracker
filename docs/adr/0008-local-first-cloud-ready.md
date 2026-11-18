# ADR-0008: Local-first deployment, cloud-ready abstractions

- **Status:** Accepted
- **Date:** 2026-11-18

## Context

Budget is zero at the start; the app may later be hosted for friends or published. Moving from local to cloud should be configuration, not a rewrite.

## Decision

- **Local:** the whole stack runs with `docker compose up` (API, PostgreSQL, web dev server). Files are stored on local disk.
- **Abstractions at every infrastructure edge:** `IFileStorage` (local disk → S3-compatible e.g. Cloudflare R2 / Azure Blob), email/notification sender, clock, current user. Configuration via environment variables / options pattern; no hard-coded hosts.
- **Containers as the deploy unit:** the API ships as a container image so it runs on any host (Azure Container Apps, Fly.io, a VPS…).
- **Free-tier target for M6** (decided in its own ADR then): e.g. static web on Cloudflare Pages / Vercel, API container on a free tier, PostgreSQL on Neon, files on Cloudflare R2.

## Consequences

- Zero cost until M6; no vendor lock-in.
- Slight upfront cost of abstractions — kept thin, only at true infrastructure boundaries.
- Backups are the owner's responsibility while local: a backup/export script is part of M0/M1.
