# Changelog

All notable changes are documented here. Format: [Keep a Changelog](https://keepachangelog.com/en/1.1.0/); versions follow [SemVer](https://semver.org/). Each milestone ends with a tagged release.

## [Unreleased]

### Added
- Project kickoff: vision, roadmap, domain model, initial ADRs (0001–0010), GitHub issue/PR templates.
- Monorepo scaffolding: pnpm workspace, `apps/api` .NET 10 solution with `SharedKernel`, `packages/shared` and `packages/api-client`, shared EditorConfig/Prettier/ESLint, root `build`/`test`/`lint` scripts (#2).
- API skeleton: `HealthTracker.Api` host composing modules via `IModule`, `Profiles` module with a first vertical slice, `ProfileId` route/JSON binding, OpenAPI document + Scalar UI (Development), `/health/live` and `/health/ready`, RFC 9457 ProblemDetails for all errors, JSON console logging outside Development, architecture tests enforcing module boundaries (#3).
