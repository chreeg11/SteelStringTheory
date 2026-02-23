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
            int newMidi = note.MidiNumber + semitones;
            int pitchClass = ((newMidi % 12) + 12) % 12;
            var (name, accidental) = DefaultSpelling[pitchClass];
            int octave = (newMidi - pitchClass) / 12 - 1;

            return new Note(name, accidental, octave);
        }

        // Move a note up/down by semitones, using Key for context-aware spelling
        public static Note Transpose(Note note, int semitones, Key key)
        {
            int newMidi = note.MidiNumber + semitones;
            int pitchClass = ((newMidi % 12) + 12) % 12;
            var (name, accidental) = key.SpellPitchClass(pitchClass);
            int semitone = Note.NaturalSemitone(name) + (int)accidental;
            int octave = (newMidi - semitone) / 12 - 1;

            return new Note(name, accidental, octave);
        }

        // Return semitone distance between two notes (ignores octave, always ascending 0-11)
        public static int IntervalBetween(Note root, Note target)
        {
            return ((target.PitchClass - root.PitchClass) + 12) % 12;
        }
    }
}
