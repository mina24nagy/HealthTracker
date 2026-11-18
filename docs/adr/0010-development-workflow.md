# ADR-0010: Development & documentation workflow

- **Status:** Accepted
- **Date:** 2026-11-18

## Context

A key goal is that the repository tells the story of how the project was built, step by step, and that work follows professional practice.

## Decision

- **GitHub Milestones** mirror the [roadmap](../roadmap.md) (M0, M1, …).
- **Every feature is an Issue** (using the issue templates) assigned to a milestone.
- **One branch + one Pull Request per issue**, branch name `<type>/<issue#>-<slug>` (e.g. `feat/12-inbody-scan-entry`). PRs reference their issue (`Closes #12`) and use the PR template. `main` is protected; merge by squash.
- **Conventional Commits** (`feat:`, `fix:`, `docs:`, `refactor:`, `test:`, `chore:`, `ci:`) — also the squash-commit title.
- **ADRs** for significant decisions (ADR-0001).
- **Devlog entry** in `docs/devlog/` at the end of each milestone: what was built, decisions, lessons, screenshots.
- **Release**: each milestone ends with a SemVer tag (`v0.1.0` = M1, …), a GitHub Release and a `CHANGELOG.md` entry.
- **CI** (GitHub Actions) must pass before merge.

## Consequences

- The issue → PR → release trail documents the build history automatically.
- Slight process overhead for a solo project — accepted, since practising this process is itself a goal.
