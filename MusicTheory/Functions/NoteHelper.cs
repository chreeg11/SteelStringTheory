namespace MusicTheory.Functions
{
    using MusicTheory.Models;

    public static class NoteHelper
    {
        // Move a note up/down by semitones, wrapping around 12
        public static Note Transpose(Note note, int semitones)
        {
            int pitchValue = (int)note.PitchClass;
            int totalSemitones = pitchValue + semitones;

            int newPitchValue;
            int octaveShift;

            if (totalSemitones >= 0)
            {
                newPitchValue = totalSemitones % 12;
                octaveShift = totalSemitones / 12;
            }
            else
            {
                // Calculate how many full octaves down we go
                int octavesDown = (Math.Abs(totalSemitones) + 11) / 12;
                newPitchValue = (totalSemitones + (octavesDown * 12)) % 12;
                octaveShift = -octavesDown;
            }

            var newPitchClass = (PitchClass)newPitchValue;
            var newOctave = note.Octave + octaveShift;

            // Transposition doesn't preserve spelling - would need key context for that
            return new Note(newPitchClass, newOctave, SpelledName: null, Accidental: null);

        }

        // Return semitone distance between two notes (ignores octave)
        public static int IntervalBetween(Note root, Note target)
        {
            int rootVal = (int)root.PitchClass;
            int targetVal = (int)target.PitchClass;
            int interval = (targetVal - rootVal + 12) % 12;
            return interval;
        }
    }
}
