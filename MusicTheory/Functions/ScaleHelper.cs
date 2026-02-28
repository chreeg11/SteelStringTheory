namespace MusicTheory.Functions
{
    using MusicTheory.Models;

    public static class ScaleHelper
    {
        public static bool TryGenerateScale(Note root, Mode mode, out Note[] scale)
        {
            if (!ModePatterns.TryGetPattern(mode, out int[]? intervals))
            {
                scale = [];
                return false;
            }

            var key = new Key(root.SpelledName, root.Accidental, mode);

            List<Note> theScale = [root];
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