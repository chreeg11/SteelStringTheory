using MusicTheory.Models;
using PedalSteel.Models;

namespace PedalSteel.Data
{
    public static class CopedentFactory
    {
        public static Copedent CreateE9()
        {
            // E9 standard tuning, strings 1-10 high to low
            var strings = new List<GuitarString>
            {
                new(1,  new Note(NoteName.F, Accidental.Sharp,   4)),
                new(2,  new Note(NoteName.D, Accidental.Sharp,   4)),
                new(3,  new Note(NoteName.G, Accidental.Sharp,   4)),
                new(4,  new Note(NoteName.E, Accidental.Natural, 4)),
                new(5,  new Note(NoteName.B, Accidental.Natural, 3)),
                new(6,  new Note(NoteName.G, Accidental.Sharp,   3)),
                new(7,  new Note(NoteName.F, Accidental.Sharp,   3)),
                new(8,  new Note(NoteName.E, Accidental.Natural, 3)),
                new(9,  new Note(NoteName.D, Accidental.Natural, 3)),
                new(10, new Note(NoteName.B, Accidental.Natural, 2)),
            };

            // StringNumber references match the string numbers above
            var pedalEffects = new Dictionary<Pedal, IReadOnlyList<StringEffect>>
            {
                // A pedal: strings 5 & 10 (B → C#, +2 semitones)
                [new Pedal("A")] = new List<StringEffect> { new(5, 2), new(10, 2) },

                // B pedal: strings 3 & 6 (G# → A, +1 semitone)
                [new Pedal("B")] = new List<StringEffect> { new(3, 1), new(6, 1) },

                // C pedal: string 4 (E → F#, +2 semitones)
                [new Pedal("C")] = new List<StringEffect> { new(4, 2) },
            };

            return new Copedent("E9 Standard", strings, pedalEffects);
        }
    }
}
