namespace MusicTheory.Models
{
    public enum Mode
    {
        Major,
        Minor,
        Ionian,
        Dorian,
        Phrygian,
        Lydian,
        Mixolydian,
        Aeolian,
        Locrian,
        HarmonicMinor,
        MelodicMinor,
    }

    /// <summary>
    /// Maps each Mode to its interval pattern (sequence of semitone steps).
    /// </summary>
    public static class ModePatterns
    {
        private static readonly Dictionary<Mode, int[]> Patterns = new()
        {
            { Mode.Major,         [2, 2, 1, 2, 2, 2, 1] },
            { Mode.Ionian,        [2, 2, 1, 2, 2, 2, 1] },
            { Mode.Minor,         [2, 1, 2, 2, 1, 2, 2] },
            { Mode.Aeolian,       [2, 1, 2, 2, 1, 2, 2] },
            { Mode.Dorian,        [2, 1, 2, 2, 2, 1, 2] },
            { Mode.Phrygian,      [1, 2, 2, 2, 1, 2, 2] },
            { Mode.Lydian,        [2, 2, 2, 1, 2, 2, 1] },
            { Mode.Mixolydian,    [2, 2, 1, 2, 2, 1, 2] },
            { Mode.Locrian,       [1, 2, 2, 1, 2, 2, 2] },
            { Mode.HarmonicMinor, [2, 1, 2, 2, 1, 3, 1] },
            { Mode.MelodicMinor,  [2, 1, 2, 2, 2, 2, 1] },
        };

        public static int[] GetPattern(Mode mode) => Patterns[mode];

        public static bool TryGetPattern(Mode mode, out int[] pattern)
            => Patterns.TryGetValue(mode, out pattern!);
    }
}
