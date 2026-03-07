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
        NoteName NoteName,
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

        public static int NaturalSemitone(NoteName name) => SemitonesFromC[name];

        public int Semitone => SemitonesFromC[NoteName] + (int)Accidental;
        public int PitchClass => ((Semitone % 12) + 12) % 12;
        public int MidiNumber => (Octave + 1) * 12 + Semitone;

        private static readonly Dictionary<Accidental, string> AccidentalSymbols = new()
        {
            { Accidental.DoubleFlat,  "bb" },
            { Accidental.Flat,        "b"  },
            { Accidental.Natural,     ""   },
            { Accidental.Sharp,       "#"  },
            { Accidental.DoubleSharp, "##" },
        };

        /// <summary>Returns a human-readable note name, e.g. "F#4", "Bb3", "C4".</summary>
        public override string ToString() => $"{NoteName}{AccidentalSymbols[Accidental]}{Octave}";
    }
}