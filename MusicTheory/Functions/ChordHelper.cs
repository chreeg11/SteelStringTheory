namespace MusicTheory.Functions
{
    using MusicTheory.Models;

    public static class ChordHelper
    {
        public static bool TryGenerateChord(Note root, ChordType chordType, out Note[] chord)
        {
            if (!ChordPatterns.TryGetPattern(chordType, out int[]? intervals))
            {
                chord = [];
                return false;
            }

            var key = new Key(root.NoteName, root.Accidental, Mode.Major);

            List<Note> theChord = [root];
            foreach (int offset in intervals)
            {
                theChord.Add(NoteHelper.Transpose(root, offset, key));
            }
            chord = theChord.ToArray();
            return true;
        }

        /// <summary>
        /// Generate chord with explicit Key for context-aware spelling.
        /// Use when the chord's root differs from the key (e.g. Bb chord in key of F).
        /// </summary>
        public static bool TryGenerateChord(Note root, ChordType chordType, Key key, out Note[] chord)
        {
            if (!ChordPatterns.TryGetPattern(chordType, out int[]? intervals))
            {
                chord = [];
                return false;
            }

            List<Note> theChord = [root];
            foreach (int offset in intervals)
            {
                theChord.Add(NoteHelper.Transpose(root, offset, key));
            }
            chord = theChord.ToArray();
            return true;
        }
    }
}