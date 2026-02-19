# Pedal Steel Guitar MusicTheory Library - Implementation Plan

## Problem Statement
You have a solid start on a MusicTheory library (Note, PitchClass, Interval models with basic transposition logic), but the path from here to visualizing scales, chords, and intervals on a pedal steel fretboard isn't clear. The domain feels complex—lots of moving parts including music theory primitives, pedal steel-specific concepts (copedents, pedals, levers, string tunings), and eventual UI scenarios.

## Approach
Build the library incrementally in layers, validating each layer before moving up:
1. **Core Music Theory** - Scales, chords, intervals (theory agnostic to instruments)
2. **Pedal Steel Domain Models** - Copedents, strings, pedal/lever mechanics
3. **Query/Projection Layer** - "Show me C major at fret 8" logic
4. **Validation** - Console test harness to prove out scenarios
5. **UI Foundation** - MAUI skeleton (later phase)

This keeps the library decoupled from UI concerns and testable via simple console scenarios.

## Architecture Principles
- **Separation of concerns**: Music theory ↔ Pedal steel models ↔ Visualization logic
- **Immutable data structures**: Leverage C# records for Note, Scale, Chord, Copedent
- **Pure functions**: Calculations should be side-effect-free and testable
- **Start simple**: E9 tuning only, then C6, then custom copedents (priority 3)
- **Defer UI decisions**: Build library first, MAUI later once library is validated

## Technology Choices
- **Language**: C# (.NET 8+)
- **UI Framework**: MAUI (cross-platform: Windows, iOS, Android) - committed but implemented later
- **Testing**: xUnit or NUnit for unit tests
- **Target**: Library-first approach, console validation, then UI

---

## Implementation Phases

### Phase 1: Core Music Theory Foundations
**Goal**: Build the fundamental music theory primitives needed for scales, chords, and intervals.

**What exists now**:
- ✅ `PitchClass` enum (C through B with sharps)
- ✅ `Note` record (pitch + octave)
- ✅ `Interval` record (semitones + direction)
- ✅ `NoteHelper.Transpose()` - move notes by semitones
- ✅ `NoteHelper.IntervalBetween()` - semitone distance between notes
- ⚠️ `IntervalHelper.GetInterval()` - stub, needs implementation

**What's needed**:
- **Key/Tonality model** - Represents musical key context (e.g., "Key of Ab major") to drive spelling decisions
- **Enharmonic handling** - Pitch identity (semitone value) separate from spelling (C# vs Db)
  - Design options: 
    - Add `NoteName` (A-G) + `Accidental` (-1, 0, +1) alongside `PitchClass`
    - Or: `Key` provides `SpellPitch(int semitone)` method for context-aware spelling
  - **Critical**: Cannot defer—scales and chords require correct spelling from day one
- Scale model + scale definitions (Ionian, Dorian, etc.)
- Chord model + chord definitions (major, minor, 7th, etc.)
- Scale generation: "Given Key of C major, generate [C, D, E, F, G, A, B]" with correct spelling
- Chord generation: "Given C major, generate [C, E, G]" with correct spelling
- Interval naming improvements (handle compound intervals beyond octave, quality + size)

**Validation**:
- Console tests: Generate C major scale, show notes
- Console tests: Generate C major chord, show notes
- Console tests: Generate all notes in A minor pentatonic

---

### Phase 2: Pedal Steel Domain Models
**Goal**: Model the physical/mechanical aspects of a pedal steel guitar.

**What's needed**:
- `Copedent` model (string tunings + pedal/lever changes as data, not hard-coded)
- `GuitarString` model (string number, open tuning note with octave)
- `Pedal` / `Lever` models (which strings they affect, semitone delta as configurable value)
- **`PedalState` as first-class type** - Composite of engaged pedals, hashable/queryable
  - Example: `PedalState.Open`, `PedalState.Create(PedalId.A, PedalId.B)`
  - Handles pedal combinations cleanly, usable as dictionary keys for caching
- E9 standard copedent definition (with accurate pedal values, but configurable)
- Fret calculation logic: "What note is string 3 at fret 5 with A+B pedals down?"
- Pedal conflict resolution rules (if same string affected by multiple pedals/levers)

**E9 Standard Tuning** (10 strings, high to low):
```
String 1: F# (high)
String 2: D#
String 3: G#
String 4: E
String 5: B
String 6: G#
String 7: F#
String 8: E
String 9: D
String 10: B (low)
```

**E9 Standard Pedals/Levers** (simplified, most common):
- A pedal: Raises strings 4 & 8 by 2 semitones (E→F#, E→F#)
- B pedal: Raises strings 5 & 10 by 1 semitone (B→C, B→C)
- C pedal: Raises strings 6 by 1 semitone (G#→A)
- Common knee levers: Various combinations, defer until A/B/C are working
- **Note**: Copedent data should be configurable; these values represent standard E9 but may need tweaking

**Fretboard Range**:
- Standard pedal steel: 24 frets (0-24)
- Each string: fret 12 is one octave higher than fret 0 (12 semitones = 1 octave)
- Pattern repeats: fret 13 = fret 1 + octave, fret 14 = fret 2 + octave, etc.
- Note: Open strings (across string 1-10) span multiple octaves, not just one
- Most practical playing happens in frets 0-12, but full range should be modeled

**Validation**:
- Console tests: Show E9 open string tunings
- Console tests: Show E9 strings with A pedal engaged
- Console tests: Calculate note at string 5, fret 3, B pedal down

---

### Phase 3: Fretboard Query Layer
**Goal**: Answer questions like "Show me all C major scale notes on the fretboard" or "Show me C major chord positions."

**What's needed**:
- Fretboard model (strings + fret range, 0-24 standard, current pedal state)
- **Rich query return types** - Not just `(string, fret)` tuples, but structured objects:
  - `FretPosition` record: string number, fret, resulting note, scale degree (if applicable)
  - `FretboardProjection` record: collection of positions + context (scale, pedal state, fret range)
- Query: "Given a scale and pedal state, return all matching fret positions"
- Query: "Given a chord and pedal state, return all chord tone positions"
- Query: "Given an interval (e.g., major 3rd), return all matching pairs"
- Filter by fret range (e.g., "show only frets 7-10")
- Handle "no matching positions" gracefully

**Example queries to support**:
- `GetScalePositions(scale: C Ionian, pedalState: A+B down, fretRange: 0-24)`
- `GetChordPositions(chord: C major, pedalState: Open, fretRange: All)`
- `GetIntervalPairs(interval: Major 3rd, pedalState: Open)`

**Validation**:
- Console tests: Show all C major scale notes on open E9 strings
- Console tests: Show C major scale at fret 8 only
- Console tests: Show C, E, G chord tones with A+B pedals down

---

### Phase 4: Scenario Validation (Console Test Harness)
**Goal**: Validate the library handles your real-world scenarios before building UI.

**Test scenarios** (from your request):
1. C Ionian scale on E9 fretboard (all frets, open strings)
2. C Ionian scale only at fret 8
3. C Ionian scale at fret 3 with A+B pedals down
4. C major chord positions (all C, E, G notes)
5. Major 3rd intervals (C+E pairs)
6. Major 6th intervals (C+A pairs or E+C pairs depending on interpretation)

**Validation**:
- Each scenario runs via console
- Output is clear and correct
- Edge cases handled (no notes found, out of range, etc.)

---

### Phase 5: UI Foundation (Future)
**Goal**: Build minimal MAUI app to visualize fretboard.

**Deferred until library is solid**. Will include:
- MAUI project setup
- Fretboard visualization component
- Key/scale selector dropdown
- Pedal state toggles (A, B, C checkboxes)
- Fret range filter
- Display modes (scale, chord, intervals)

---

## Current State Assessment
**What's working**:
- Clean, minimal models (Note, PitchClass, Interval)
- Basic transposition logic
- Good use of records and enums

**What feels unclear** (your concerns):
- "Feels complex" → Yes, because you're mixing theory + instrument + UI in your head
- "Forest through the trees" → Need clearer layer boundaries
- "Not clear what's needed" → Missing scales, chords, copedent, query layer

**The solution**:
- Build in layers, validate each layer before moving up
- Keep library pure and testable
- Use console scenarios to validate before investing in UI

---

## Success Criteria
- Library can generate scales and chords from music theory primitives
- Library models E9 copedent accurately (strings + pedals)
- Library can answer "what notes are at these fret positions given this pedal state?"
- Library can answer "where are all the scale/chord notes on the fretboard?"
- All scenarios from Phase 4 run correctly via console
- Codebase feels organized, testable, and extensible

---

## Risks & Considerations
- **Enharmonic complexity**: C# vs Db matters for both display AND calculation (scale/chord spelling). **Cannot defer—must be solved in Phase 1** with Key/spelling context. Retrofitting later would break all consumers.
- **Copedent data accuracy**: Pedal semitone values must be verified against authoritative reference (Emmons E9 standard). Wrong data = wrong query results. Model copedents as configurable data, not hard-coded constants, to allow tweaking.
- **Copedent variations**: Players customize heavily. Defer custom copedents until E9 + C6 are solid, but design `Copedent` to be data-driven from day one.
- **Fretboard range**: Standard 24 frets. Each string's fret 12 is one octave above its fret 0 (12 semitones). Library should support full 0-24 range, but UI can offer focused ranges.
- **Octave arithmetic**: Fret calculation must properly track octave changes per-string. E.g., string 5 open = B3, fret 12 = B4, fret 24 = B5. Open strings span ~3 octaves (B2 to F#5 roughly).
- **Pedal conflicts**: Multiple pedals/levers can affect the same string. Define resolution rules early (additive? error? last-wins?).
- **Edge cases**: Double sharps/flats (Fbb in dim7 chords), compound intervals (9th, 11th, 13th), half-pedaling (out of scope but don't make impossible).
- **Performance**: Not a concern for this domain—queries will be sub-millisecond even for 24 frets × 10 strings.
- **UI complexity**: MAUI is the right call, but defer until library proves itself.
- **Two-repo workflow**: This repo is for planning; public repo gets final code. Consider where MusicTheory library lives—avoid building in Scratch/ then migrating later.

---

## Notes
- C# is an excellent choice—strong typing, records, pattern matching, MAUI support.
- MAUI is Microsoft's official cross-platform framework with good iOS/Android/Windows support.
- This is absolutely do-able going slow. The library layer insulates you from UI complexity.
- Focus on making the library crystal clear before worrying about the UI.
- You can validate everything via simple console scenarios first.

---

## Next Steps (After Plan Approval)
1. Review this plan, suggest edits if needed
2. Start Phase 1: Flesh out Scale and Chord models
3. Implement scale/chord generation functions
4. Write console tests to validate Phase 1
5. Move to Phase 2 once Phase 1 feels solid
