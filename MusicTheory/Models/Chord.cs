namespace MusicTheory.Models
{
    public enum ChordType
    {
        // Triads
        Major,
        Minor,
        Dim,
        Aug,

        // Sevenths
        Maj7,
        Min7,
        Dom7,
        Dim7,
        Min7b5,
    }

    public static class ChordPatterns
    {
        private static readonly Dictionary<ChordType, int[]> Patterns = new()
        {
            // Semitone offsets from root (NOT step-by-step)
            { ChordType.Major, [4, 7] },
            { ChordType.Minor, [3, 7] },
            { ChordType.Aug, [4, 8] },
            { ChordType.Dim, [3, 6] },
            { ChordType.Maj7, [4, 7, 11] },
            { ChordType.Min7, [3, 7, 10] },
            { ChordType.Dom7, [4, 7, 10] },
            { ChordType.Dim7, [3, 6, 9] },
            { ChordType.Min7b5, [3, 6, 10] },
        };

        public static int[] GetPattern(ChordType chordType) => Patterns[chordType];

        public static bool TryGetPattern(ChordType chordType, out int[] pattern)
            => Patterns.TryGetValue(chordType, out pattern!);
    }
}