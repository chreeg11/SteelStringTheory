namespace MusicTheory.Models
{
    /// <summary>
    /// A spelling context derived from a tonic note and scale interval pattern.
    /// Maps each pitch class (0-11) to the correct (NoteName, Accidental) spelling.
    /// Scale tones use key-aware spelling; non-scale tones fall back to sharp default.
    /// </summary>
    public readonly record struct Key
    {
        private static readonly (NoteName Name, Accidental Accidental)[] SharpDefault =
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

        public NoteName Tonic { get; }
        public Accidental TonicAccidental { get; }
        private readonly (NoteName Name, Accidental Accidental)[] _spellings;

        public Key(NoteName tonic, Accidental tonicAccidental, int[] intervalPattern)
        {
            Tonic = tonic;
            TonicAccidental = tonicAccidental;
            _spellings = BuildSpellingTable(tonic, tonicAccidental, intervalPattern);
        }

        public (NoteName Name, Accidental Accidental) SpellPitchClass(int pitchClass)
        {
            int normalized = ((pitchClass % 12) + 12) % 12;
            return _spellings[normalized];
        }

        private static (NoteName, Accidental)[] BuildSpellingTable(
            NoteName tonic, Accidental tonicAccidental, int[] intervalPattern)
        {
            var table = ((NoteName, Accidental)[])SharpDefault.Clone();

            // Key-aware spelling only applies to 7-note (diatonic) scales.
            // Non-diatonic scales (pentatonic, whole tone, etc.) keep sharp defaults.
            if (intervalPattern.Length != 7)
                return table;

            int tonicPc = ((Note.NaturalSemitone(tonic) + (int)tonicAccidental) % 12 + 12) % 12;
            table[tonicPc] = (tonic, tonicAccidental);

            // Walk degrees 2-7 (skip the 7th interval which returns to the octave)
            int cumulativeSemitones = 0;
            for (int i = 0; i < intervalPattern.Length - 1; i++)
            {
                cumulativeSemitones += intervalPattern[i];
                int degreePc = (tonicPc + cumulativeSemitones) % 12;

                // Each degree uses the next letter name in sequence from the tonic
                NoteName letter = (NoteName)(((int)tonic + i + 1) % 7);
                int naturalSemitone = Note.NaturalSemitone(letter);

                // Compute accidental: what adjustment makes this letter hit the target pitch class?
                int diff = ((degreePc - naturalSemitone) % 12 + 12) % 12;
                if (diff > 6) diff -= 12;

                table[degreePc] = (letter, (Accidental)diff);
            }

            return table;
        }
    }
}
