# Copilot Instructions — SteelStringTheory

## Project Overview
A C# music theory library and pedal steel guitar toolkit, built toward a cross-platform MAUI app for visualizing scales, chords, and intervals on a pedal steel fretboard.

Read `docs/IMPLEMENTATION-PLAN.md` for the full phased plan and architecture decisions.

## Project Structure
```
MusicTheory/              ← Core library — instrument-agnostic theory primitives
  Models/                 ← Note, Key, Mode, Scale, Chord, Interval
  Functions/              ← Pure helpers: NoteHelper, ScaleHelper, ChordHelper, IntervalHelper
MusicTheory.Tests/        ← xUnit tests for MusicTheory
PedalSteel/               ← Pedal steel domain library (references MusicTheory)
  Models/                 ← GuitarString, Copedent, Pedal, StringEffect
  Data/                   ← CopedentFactory (E9, C6 tuning definitions)
  Functions/              ← FretboardHelper, query helpers
PedalSteel.Tests/         ← xUnit tests for PedalSteel
SteelStringTheory/        ← Console app for validation/demos
docs/                     ← Architecture and planning docs
```

## Architecture — Dependency Direction
```
SteelStringTheory (console)
    └── PedalSteel
            └── MusicTheory
```
**MusicTheory must never reference PedalSteel.** Dependencies flow one direction only.

## Key Design Decisions
- **Note = (NoteName, Accidental, Octave)**: Spelling is the source of truth. PitchClass and MidiNumber are computed properties.
- **`readonly record struct`** for small immutable value types (Note, GuitarString, Pedal, StringEffect). Use `class` for larger aggregates (Copedent).
- **Pure functions**: All calculations are side-effect-free in static helper classes (NoteHelper, ScaleHelper, ChordHelper, FretboardHelper).
- **Mode/ChordType as enums + pattern dictionaries**: Centralized, extensible — add new scale/chord types without changing helper logic.
- **Key for enharmonic spelling**: Key provides context-aware note spelling (C# vs Db). Chord/scale generation requires Key context.
- **Copedent as aggregate root**: Validates that all StringEffect.StringNumber values exist in Strings at construction time.
- **Pedal as open value type**: `Pedal(string Name)` — not an enum, to support custom copedents.

## Current Progress
- ✅ Phase 1: MusicTheory — Note, Key, Mode, Scale, Chord, all helpers, 72+ tests
- 🔄 Phase 2: PedalSteel — GuitarString, Pedal, StringEffect, Copedent, CopedentFactory (E9), 35+ tests
- ⬜ Phase 2 remaining: PedalState, FretboardHelper (GetNoteAtFret)
- ⬜ Phase 3: Fretboard query layer (GetScalePositions, GetChordPositions, GetIntervalPairs)
- ⬜ Phase 4: Console scenario validation
- ⬜ Phase 5: MAUI UI (deferred)

## Pedal Steel Domain
- Standard E9 tuning: 10 strings (F#4, D#4, G#4, E4, B3, G#3, F#3, E3, D3, B2), 24 frets
- A pedal: strings 5 & 10 +2 semitones (B→C#)
- B pedal: strings 3 & 6 +1 semitone (G#→A)
- C pedal: string 4 +2 semitones (E→F#)
- Fret 12 = open note + 1 octave; pattern repeats
- Copedent data is configurable — not hard-coded

---

## C# Coding Standards

- Always use modern C# (currently C# 14 / .NET 10).
- Follow PascalCase for public members and methods; camelCase for private fields and locals.
- Prefer file-scoped namespace declarations.
- Use pattern matching and switch expressions wherever applicable.
- Use `nameof` instead of string literals when referring to member names.
- Use `is null` / `is not null` instead of `== null` / `!= null`.
- Declare variables non-nullable; check for null at entry points only.
- **Comments explain *why*, not what.** Don't comment obvious code.
- Add XML doc comments for public APIs.
- Don't add interfaces/abstractions unless used for external dependencies or testing.
- Least-exposure rule: `private` > `internal` > `protected` > `public`.
- Always use curly braces `{}` for all blocks, even single-line `if`, `foreach`, `for`, `while` statements.
- Use `ArgumentNullException.ThrowIfNull(x)` for null guards.

## DDD & Architecture Principles

- **Ubiquitous language**: Use consistent domain terminology (fret, string, copedent, pedal, open note) across code and tests.
- **Aggregate boundaries**: `Copedent` is an aggregate root — it validates its own invariants at construction.
- **Value objects**: Small immutable types (`Note`, `GuitarString`, `Pedal`, `StringEffect`) as `readonly record struct`.
- **Domain logic in domain layer**: Helpers (`ScaleHelper`, `ChordHelper`, `FretboardHelper`) are the domain service layer — keep business logic out of models.
- **SOLID**: Single responsibility per class/helper, open for extension via enum patterns, depend on abstractions.
- Before implementing: identify which layer is affected, which aggregates change, what invariants apply.

## Testing Standards (xUnit)

- One behavior per test. Use AAA (Arrange / Act / Assert).
- Name tests by behavior: `MethodName_Condition_ExpectedResult`.
- Use `[Theory]` + `[InlineData]` for parameterized cases.
- No branching or conditionals inside tests.
- Test through public APIs only — don't change visibility to enable testing.
- Require tests for every new/changed public API.
- Mirror class structure: `FretboardHelper` → `FretboardHelperTests`.

### TDD Workflow
- **Red**: Write a failing test that describes the desired behavior first.
- **Green**: Write the minimal code to make it pass. No over-engineering.
- **Refactor**: Clean up duplication, improve names, apply patterns — keep tests green.
- Work on one test at a time through the full red/green/refactor cycle.

## Principal Engineering Mindset

- Balance craft excellence with pragmatic delivery — good over perfect, never compromise fundamentals.
- YAGNI: don't build what isn't needed yet.
- KISS: prefer the simplest solution that correctly solves the problem.
- DRY: eliminate duplication, but don't abstract prematurely.
- Anticipate future needs in design (open/closed), but defer implementation.
- Keep diffs small; validate each layer before moving up.
- When identifying technical debt, note it explicitly — don't silently accumulate it.

## MAUI (Phase 5 — Future Reference)
- **Never use** `ListView` (use `CollectionView`), `TableView`, `AndExpand`, `BackgroundColor` (use `Background`), renderers (use handlers).
- **Never place** `ScrollView`/`CollectionView` inside `StackLayout`.
- Always use compiled bindings (`x:DataType`) for performance.
- Use `Border` instead of `Frame`.
- Use Shell navigation.
- Reference images as PNG, not SVG.

---

_Last updated: 2026-03-07_

