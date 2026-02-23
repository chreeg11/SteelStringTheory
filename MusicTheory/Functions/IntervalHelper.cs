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

        public static string GetIntervalName(int semitones)
        {
            // Handle negatives and wrap to 0-11
            int normalized = ((semitones % 12) + 12) % 12;
            return IntervalNames[normalized];
        }

        public static Interval GetInterval(Note from, Note to)
        {
            int semitones = to.MidiNumber - from.MidiNumber;
            return new Interval(Math.Abs(semitones), IsDescending: semitones < 0);
        }
    }
}
