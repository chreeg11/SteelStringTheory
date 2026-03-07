using MusicTheory.Functions;
using MusicTheory.Models;
using PedalSteel.Models;

namespace PedalSteel.Functions;

/// <summary>
/// Pure functions for calculating notes at fret positions on a pedal steel fretboard.
/// </summary>
public static class FretboardHelper
{
    /// <summary>
    /// Returns the note sounding at the given fret on a string, with the specified pedals engaged.
    /// </summary>
    /// <param name="guitarString">The string being played.</param>
    /// <param name="fret">Fret number (0 = open string).</param>
    /// <param name="copedent">The copedent defining pedal effects.</param>
    /// <param name="pedalState">Which pedals are currently engaged.</param>
    public static Note GetNoteAtFret(
        GuitarString guitarString,
        int fret,
        Copedent copedent,
        PedalState pedalState)
    {
        if (fret < 0)
        {
            throw new ArgumentException("Fret number cannot be negative.", nameof(fret));
        }

        // Sum semitone deltas from all engaged pedals that affect this string
        int pedalSemitones = 0;
        foreach (var pedal in pedalState.ActivePedals)
        {
            if (copedent.PedalEffects.TryGetValue(pedal, out var effects))
            {
                foreach (var effect in effects)
                {
                    if (effect.StringNumber == guitarString.Number)
                    {
                        pedalSemitones += effect.SemitonesDelta;
                    }
                }
            }
        }

        return NoteHelper.Transpose(guitarString.OpenNote, fret + pedalSemitones);
    }

    /// <summary>
    /// Returns all fret positions where any note in the given scale appears,
    /// using a fixed pedal state.
    /// </summary>
    public static IEnumerable<FretPosition> GetScalePositions(
        Copedent copedent,
        PedalState pedalState,
        Note[] scaleNotes,
        int minFret = 0,
        int maxFret = 24)
    {
        var pitchClasses = new HashSet<int>(scaleNotes.Select(n => n.PitchClass));
        var positions = new List<FretPosition>();

        foreach (var guitarString in copedent.Strings)
        {
            for (int fret = minFret; fret <= maxFret; fret++)
            {
                var note = GetNoteAtFret(guitarString, fret, copedent, pedalState);
                if (pitchClasses.Contains(note.PitchClass))
                {
                    positions.Add(new FretPosition(guitarString.Number, fret, note, pedalState));
                }
            }
        }

        return positions;
    }

    /// <summary>
    /// Convenience overload — builds the scale from root + mode and returns matching fret positions
    /// using a fixed pedal state.
    /// </summary>
    public static IEnumerable<FretPosition> GetScalePositions(
        Copedent copedent,
        PedalState pedalState,
        NoteName root,
        Accidental accidental,
        Mode mode,
        int minFret = 0,
        int maxFret = 24)
    {
        if (!ScaleHelper.TryGenerateScale(new Note(root, accidental, 4), mode, out var scaleNotes))
        {
            return Enumerable.Empty<FretPosition>();
        }

        return GetScalePositions(copedent, pedalState, scaleNotes, minFret, maxFret);
    }

    /// <summary>
    /// Returns all fret positions where scale notes appear, trying every possible
    /// pedal/lever combination per string and fret. Each result includes which
    /// pedal state produces that note.
    /// </summary>
    public static IEnumerable<FretPosition> GetScalePositionsAllPedalStates(
        Copedent copedent,
        Note[] scaleNotes,
        int minFret = 0,
        int maxFret = 24)
    {
        var pitchClasses = new HashSet<int>(scaleNotes.Select(n => n.PitchClass));
        var allStates = GenerateAllPedalStates(copedent);
        var positions = new List<FretPosition>();

        foreach (var guitarString in copedent.Strings)
        {
            for (int fret = minFret; fret <= maxFret; fret++)
            {
                foreach (var pedalState in allStates)
                {
                    var note = GetNoteAtFret(guitarString, fret, copedent, pedalState);
                    if (pitchClasses.Contains(note.PitchClass))
                    {
                        positions.Add(new FretPosition(guitarString.Number, fret, note, pedalState));
                    }
                }
            }
        }

        return positions;
    }

    /// <summary>
    /// Convenience overload — builds the scale and tries all pedal combinations.
    /// </summary>
    public static IEnumerable<FretPosition> GetScalePositionsAllPedalStates(
        Copedent copedent,
        NoteName root,
        Accidental accidental,
        Mode mode,
        int minFret = 0,
        int maxFret = 24)
    {
        if (!ScaleHelper.TryGenerateScale(new Note(root, accidental, 4), mode, out var scaleNotes))
        {
            return Enumerable.Empty<FretPosition>();
        }

        return GetScalePositionsAllPedalStates(copedent, scaleNotes, minFret, maxFret);
    }

    /// <summary>
    /// Generates all possible pedal states (power set of pedals in the copedent).
    /// For 5 pedals/levers this is 2^5 = 32 combinations — very manageable.
    /// </summary>
    private static List<PedalState> GenerateAllPedalStates(Copedent copedent)
    {
        var pedals = copedent.PedalEffects.Keys.ToList();
        var states = new List<PedalState> { PedalState.Open };

        foreach (var pedal in pedals)
        {
            var newStates = new List<PedalState>();
            foreach (var existing in states)
            {
                // Add a new state that includes this pedal
                var combined = new HashSet<Pedal>(existing.ActivePedals) { pedal };
                newStates.Add(new PedalState(combined));
            }
            states.AddRange(newStates);
        }

        return states;
    }
}
