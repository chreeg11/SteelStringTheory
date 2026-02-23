namespace MusicTheory.Models
{
    public enum ScaleType 
    { 
        Major = 0,
        Ionian = 1, // Same as major but we want a separate entry
        Dorian = 2,
        Phrygian = 3,
        Lydian = 4,
        MixoLydian = 5,
        Aeolian = 6,
        NaturalMinor = 7, // Same as Aeolian but we want a separate entry
        Locrian = 8,
        HarmonicMinor = 9,
        MelodicMinor = 10, 
    }
}