namespace MusicTheory.Functions
{
    using MusicTheory.Models;

    public static class ScaleHelper
    {
        private static readonly Dictionary<ScaleType, int[]> ScalePatterns = new()
        {
            { ScaleType.Major, [2, 2, 1, 2, 2, 2, 1] },
            { ScaleType.Ionian, [2, 2, 1, 2, 2, 2, 1] },
        };

        public static bool TryGenerateScale(Note root, ScaleType scaleType, out Note[] scale)
        {
            if (!ScalePatterns.TryGetValue(scaleType, out int[]? intervals))
            {
                scale = [];
                return false;
            }

            List<Note> theScale = new();
            theScale.Add(root);
            int intervalCount = 0;
            foreach (int i in intervals)
            {
                intervalCount += i;
                Note scaleNote = NoteHelper.Transpose(root, intervalCount);
                theScale.Add(scaleNote);
            }
            scale = theScale.ToArray();
            return true;
        }
    }
}