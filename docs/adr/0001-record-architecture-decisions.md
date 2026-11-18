# ADR-0001: Record architecture decisions

- **Status:** Accepted
- **Date:** 2026-11-18

## Context

This project is built incrementally as a side project, partly to practise senior-level engineering, and its build history should be readable by others. Decisions made in chat or in someone's head are lost.

## Decision

Record every significant technical decision as a Markdown ADR in `docs/adr/`, numbered sequentially, using [template.md](template.md). Accepted ADRs are not edited; changes are made by a superseding ADR.

## Consequences

- Reasons behind the architecture stay discoverable next to the code.
- Small overhead per decision; "significant" = hard to reverse, or affects several modules.
