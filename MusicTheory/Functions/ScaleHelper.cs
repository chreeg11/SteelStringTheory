namespace MusicTheory.Functions
{
    using MusicTheory.Models;

    public static class ScaleHelper
    {
        private static readonly Dictionary<ScaleType, int[]> ScalePatterns = new()
        {
            { ScaleType.Major, [2, 2, 1, 2, 2, 2, 1] },
            { ScaleType.Ionian, [2, 2, 1, 2, 2, 2, 1] },
            { ScaleType.Dorian, [2, 1, 2, 2, 2, 1, 2] },
            { ScaleType.Phrygian, [1, 2, 2, 2, 1, 2, 2] },
            { ScaleType.Lydian, [2, 2, 2, 1, 2, 2, 1] },
            { ScaleType.Mixolydian, [2, 2, 1, 2, 2, 1, 2] },
            { ScaleType.Aeolian, [2, 1, 2, 2, 1, 2, 2] },
            { ScaleType.NaturalMinor, [2, 1, 2, 2, 1, 2, 2] },
            { ScaleType.HarmonicMinor, [2, 1, 2, 2, 1, 3, 1] },
        };

        public static bool TryGenerateScale(Note root, ScaleType scaleType, out Note[] scale)
        {
            if (!ScalePatterns.TryGetValue(scaleType, out int[]? intervals))
            {
                scale = [];
                return false;
            }

            var key = new Key(root.SpelledName, root.Accidental, intervals);

            List<Note> theScale = new();
            theScale.Add(root);
            int intervalCount = 0;
            foreach (int i in intervals)
            {
                intervalCount += i;
                theScale.Add(NoteHelper.Transpose(root, intervalCount, key));
            }
            scale = theScale.ToArray();
            return true;
        }
    }
}