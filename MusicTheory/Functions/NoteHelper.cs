namespace MusicTheory.Functions
{
    using MusicTheory.Models;

    public static class NoteHelper
    {
        // Default spelling for each pitch class (0-11), using sharps.
        // Later, a Key can override these to use flats where appropriate.
        private static readonly (NoteName Name, Accidental Accidental)[] DefaultSpelling =
        [
            (NoteName.C, Accidental.Natural),   // 0
            (NoteName.C, Accidental.Sharp),     // 1
            (NoteName.D, Accidental.Natural),   // 2
            (NoteName.D, Accidental.Sharp),     // 3
            (NoteName.E, Accidental.Natural),   // 4
            (NoteName.F, Accidental.Natural),   // 5
            (NoteName.F, Accidental.Sharp),     // 6
            (NoteName.G, Accidental.Natural),   // 7
            (NoteName.G, Accidental.Sharp),     // 8
            (NoteName.A, Accidental.Natural),   // 9
            (NoteName.A, Accidental.Sharp),     // 10
            (NoteName.B, Accidental.Natural),   // 11
        ];

        // Move a note up/down by semitones, using default sharp spelling
        public static Note Transpose(Note note, int semitones)
        {
            // Step 1: Get new absolute position
            int newMidi = note.MidiNumber + semitones;

            // Step 2: Split into pitch class (0-11) and octave
            int pitchClass = ((newMidi % 12) + 12) % 12;
            int octave = (newMidi - pitchClass) / 12 - 1;

            // Step 3: Look up default spelling
            var (name, accidental) = DefaultSpelling[pitchClass];

            return new Note(name, accidental, octave);
        }

        // Return semitone distance between two notes (ignores octave, always ascending 0-11)
        public static int IntervalBetween(Note root, Note target)
        {
            return ((target.PitchClass - root.PitchClass) + 12) % 12;
        }
    }
}
