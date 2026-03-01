# Copilot Instructions — MusicTheory Library

## Project Overview
A C# music theory library for a pedal steel guitar application. The library provides core music theory primitives (notes, scales, chords, intervals) and will eventually model pedal steel-specific concepts (copedents, pedals/levers, fretboard queries). The ultimate goal is a MAUI-based cross-platform app for visualizing scales, chords, and intervals on a pedal steel fretboard.

Read `docs/IMPLEMENTATION-PLAN.md` for the full phased plan and architecture decisions.

## Project Structure
```
MusicTheory/              ← Core library (class library)
  Models/                 ← Domain types: Note, Scale, Interval, etc.
  Functions/              ← Pure helper functions: NoteHelper, ScaleHelper, IntervalHelper
PedalSteelGuitar/         ← Console app for testing/demos
MusicTheory.Tests/        ← xUnit test project
docs/                     ← Architecture and planning docs
```

## Key Design Decisions
- **Note = (NoteName, Accidental, Octave)**: Spelling is the source of truth. PitchClass and MidiNumber are computed properties.
- **No PitchClass enum**: Removed in favor of computed `int PitchClass` (0-11) on Note.
- **Immutable records**: All domain types are `readonly record struct` for value semantics.
- **Pure functions**: All calculations are side-effect-free, in static helper classes.
- **Default sharp spelling**: `NoteHelper.Transpose()` uses sharps by default. A future `Key` model will provide context-aware spelling (C# vs Db).
- **Scale patterns as data**: `ScaleType` enum + dictionary of interval patterns. Generation is a generic function.

## Current Progress (Phase 1: Core Music Theory)
- ✅ Note model with enharmonic support (NoteName + Accidental → computed PitchClass/MidiNumber)
- ✅ NoteHelper: Transpose, IntervalBetween
- ✅ IntervalHelper: GetInterval, GetIntervalName
- ✅ Scale model: ScaleType enum + ScaleHelper.TryGenerateScale
- ⬜ Key model (for context-aware note spelling)
- ⬜ Chord model
- ⬜ Remaining scale patterns (only Major/Ionian implemented so far)

## Upcoming Phases
- **Phase 2**: Pedal steel domain (Copedent, GuitarString, Pedal/Lever, E9 tuning)
- **Phase 3**: Fretboard query layer (GetScalePositions, GetChordPositions, GetIntervalPairs)
- **Phase 4**: Scenario validation via console
- **Phase 5**: MAUI UI (deferred)

## Coding Preferences
- Act as a consultant: help with approaches, review, and explanations.
- Teach as you go — explain design decisions and tradeoffs.
- Write tests alongside every new feature (xUnit, AAA pattern).
- Keep changes small and incremental — validate each layer before moving up.
- Use C# modern syntax (records, collection expressions, pattern matching).
- Target .NET 10.

## Pedal Steel Domain Notes
- Standard E9 tuning: 10 strings, 24 frets
- A pedal: +2 semitones on strings 5 & 10 (B→C#)
- B pedal: +1 semitone on strings 3 & 6 (G#→A)
- C pedal: +2 semitone on string 4 & 5 (E->F#, B->C#)
- Copedent data should be configurable (not hard-coded)
- Each string's fret 12 = fret 0 + one octave; open strings span ~3 octaves
- Priority: E9 → C6 → custom copedents

---

_Last updated: 2026-02-23_
