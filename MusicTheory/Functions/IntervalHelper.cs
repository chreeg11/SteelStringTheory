namespace MusicTheory.Functions
{
    using MusicTheory.Models;

    public static class IntervalHelper
    {
        private static readonly string[] IntervalNames =
        [
            "Unison",        // 0
            "Minor 2nd",     // 1
            "Major 2nd",     // 2
            "Minor 3rd",     // 3
            "Major 3rd",     // 4
            "Perfect 4th",   // 5
            "Tritone",       // 6
            "Perfect 5th",   // 7
            "Minor 6th",     // 8
            "Major 6th",     // 9
            "Minor 7th",     // 10
            "Major 7th",     // 11
        ];

        /// <summary>
        /// Shorthand scale-degree labels used on fretboard diagrams.
        /// Index = semitones from root. Standard notation: R, b2, 2, b3, 3, 4, b5, 5, b6, 6, b7, 7.
        /// </summary>
        private static readonly string[] DegreeLabels =
        [
            "R",   // 0  — Root / Unison
            "b2",  // 1  — Minor 2nd
            "2",   // 2  — Major 2nd
            "b3",  // 3  — Minor 3rd
            "3",   // 4  — Major 3rd
            "4",   // 5  — Perfect 4th
            "b5",  // 6  — Tritone (b5 by default; Lydian may prefer #4)
            "5",   // 7  — Perfect 5th
            "b6",  // 8  — Minor 6th
            "6",   // 9  — Major 6th
            "b7",  // 10 — Minor 7th
            "7",   // 11 — Major 7th
        ];

        public static string GetIntervalName(int semitones)
        {
            int normalized = ((semitones % 12) + 12) % 12;
            return IntervalNames[normalized];
        }

        /// <summary>
        /// Returns the shorthand scale-degree label for a given number of semitones from the root.
        /// Examples: 0 → "R", 4 → "3", 7 → "5", 10 → "b7".
        /// </summary>
        public static string GetDegreeLabel(int semitones)
        {
            int normalized = ((semitones % 12) + 12) % 12;
            return DegreeLabels[normalized];
        }

        /// <summary>
        /// Returns the scale-degree label for a note relative to a root note.
        /// Uses pitch class comparison so octave doesn't matter.
        /// </summary>
        public static string GetDegreeLabel(Note root, Note note)
        {
            int semitones = ((note.PitchClass - root.PitchClass) + 12) % 12;
            return DegreeLabels[semitones];
        }

        public static Interval GetInterval(Note from, Note to)
        {
            int semitones = to.MidiNumber - from.MidiNumber;
            return new Interval(Math.Abs(semitones), IsDescending: semitones < 0);
        }
    }
}
