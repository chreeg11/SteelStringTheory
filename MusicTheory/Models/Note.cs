namespace MusicTheory.Models
{
    public enum PitchClass
    {
        C = 0,
        CSharp = 1,
        D = 2,
        DSharp = 3,
        E = 4,
        F = 5,
        FSharp = 6,
        G = 7,
        GSharp = 8,
        A = 9,
        ASharp = 10,
        B = 11
    }

    public enum NoteName { C, D, E, F, G, A, B }

    public enum Accidental
    {
        DoubleFlag = -2,
        Flat = -1,
        Natural = 0,
        Sharp = 1,
        DoubleSharp = 2
    }

    public readonly record struct Note(
        PitchClass PitchClass,
        int Octave,
        NoteName? SpelledName,
        Accidental? Accidental);
}