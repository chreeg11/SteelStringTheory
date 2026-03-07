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
}
