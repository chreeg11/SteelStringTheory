# SteelStringTheory

A C# music theory library and pedal steel guitar visualization toolkit, built toward a cross-platform MAUI app for visualizing scales, chords, and intervals on a pedal steel fretboard.

## Projects

| Project | Type | Description |
|---|---|---|
| `MusicTheory` | Library | Instrument-agnostic music theory primitives |
| `MusicTheory.Tests` | xUnit | Tests for MusicTheory |
| `PedalSteel` | Library | Pedal steel domain models and fretboard queries |
| `PedalSteel.Tests` | xUnit | Tests for PedalSteel |
| `SteelStringTheory` | Console App | Demo/validation harness |

## Architecture

```
SteelStringTheory (console)
    └── PedalSteel (instrument domain)
            └── MusicTheory (pure theory)
```

Dependencies flow in one direction only — music theory knows nothing about pedal steel.

## MusicTheory Library

Core music theory primitives, instrument-agnostic and reusable:

- **`Note`** — `(NoteName, Accidental, Octave)` with computed `PitchClass` and `MidiNumber`
- **`Key`** — Key signature with context-aware enharmonic spelling (C# vs Db)
- **`Mode`** — Ionian, Dorian, Phrygian, Lydian, Mixolydian, Aeolian, Locrian + patterns
- **`Scale`** — Generated note array for a given key and mode
- **`ChordType`** — Major, Minor, Dominant7, Major7, Minor7, Diminished, Augmented, and more
- **`Chord`** — Generated note array for a given root and chord type
- **`NoteHelper`** — Transpose, IntervalBetween
- **`ScaleHelper`** — TryGenerateScale
- **`ChordHelper`** — TryGenerateChord (with optional Key context for correct spelling)

## PedalSteel Library

_(In progress — Phase 2)_

Pedal steel-specific domain models:

- **`GuitarString`** — String number and open tuning note
- **`Copedent`** — String tunings + pedal/lever definitions (data-driven, configurable)
- **`Pedal` / `Lever`** — Which strings they affect and by how many semitones
- **`Fretboard`** — Query layer: "What note is string 5 at fret 3 with A+B pedals down?"

### E9 Standard Tuning (high → low)

| String | Open Note |
|---|---|
| 1 | F# |
| 2 | D# |
| 3 | G# |
| 4 | E |
| 5 | B |
| 6 | G# |
| 7 | F# |
| 8 | E |
| 9 | D |
| 10 | B |

### E9 Standard Pedals

| Pedal | Strings Affected | Change |
|---|---|---|
| A | 4, 8 | +2 semitones (E → F#) |
| B | 5, 10 | +1 semitone (B → C) |
| C | 6 | +1 semitone (G# → A) |

## Goals

- Visualize scales, chords, and intervals on a pedal steel fretboard
- Support common scenarios: "Show C Ionian at fret 8", "Show C major chord with A+B pedals down"
- Cross-platform UI via MAUI (Windows, iPad) — future phase

## Tech Stack

- C# / .NET 10
- xUnit for testing
- MAUI for UI (future)

## Built With AI Collaboration

This project was built collaboratively with [GitHub Copilot](https://github.com/features/copilot) (Claude Sonnet / Opus). The architecture, design decisions, and code were developed through an iterative conversation — Copilot as a consultant and pair programmer, the human as the domain expert and decision-maker.
