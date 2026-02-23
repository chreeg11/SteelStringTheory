namespace MusicTheory.Models
{
    public enum NoteName { C, D, E, F, G, A, B }

    public enum Accidental
    {
        DoubleFlat = -2,
        Flat = -1,
        Natural = 0,
        Sharp = 1,
        DoubleSharp = 2
    }

    public readonly record struct Note(
        NoteName SpelledName,
        Accidental Accidental,
        int Octave)
    {
        private static readonly Dictionary<NoteName, int> SemitonesFromC = new()
        {
            { NoteName.C, 0 },    // C to C = 0
            { NoteName.D, 2 },    // C to D = 2 (whole step)
            { NoteName.E, 4 },    // C to E = 4 (two whole steps)
            { NoteName.F, 5 },    // C to F = 5 (E to F is a half step!)
            { NoteName.G, 7 },
            { NoteName.A, 9 },
            { NoteName.B, 11 },
        };

        public int Semitone => SemitonesFromC[SpelledName] + (int)Accidental;
        public int PitchClass => ((Semitone % 12) + 12) % 12;
        public int MidiNumber => (Octave + 1) * 12 + Semitone;
    }
}