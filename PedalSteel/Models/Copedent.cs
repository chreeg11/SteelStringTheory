namespace PedalSteel.Models
{
    public class Copedent
    {
        public string Name { get; }
        public IReadOnlyList<GuitarString> Strings { get; }
        public IReadOnlyDictionary<Pedal, IReadOnlyList<StringEffect>> PedalEffects { get; }

        public Copedent(
            string name,
            IReadOnlyList<GuitarString> strings,
            IReadOnlyDictionary<Pedal, IReadOnlyList<StringEffect>> pedalEffects)
        {
            var validNumbers = new HashSet<uint>(strings.Select(s => s.Number));

            foreach (var (pedal, effects) in pedalEffects)
                foreach (var effect in effects)
                    if (!validNumbers.Contains(effect.StringNumber))
                        throw new ArgumentException(
                            $"Pedal '{pedal.Name}' references string {effect.StringNumber}, which does not exist in this copedent.");

            Name = name;
            Strings = strings;
            PedalEffects = pedalEffects;
        }
    }
}