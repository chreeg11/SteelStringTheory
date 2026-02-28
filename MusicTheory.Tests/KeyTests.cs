using MusicTheory.Models;

namespace MusicTheory.Tests;

public class KeyTests
{
    [Fact]
    public void CMajor_AllNaturals()
    {
        var key = new Key(NoteName.C, Accidental.Natural, Mode.Major);

        // All 7 scale tones should be natural
        Assert.Equal((NoteName.C, Accidental.Natural), key.SpellPitchClass(0));
        Assert.Equal((NoteName.D, Accidental.Natural), key.SpellPitchClass(2));
        Assert.Equal((NoteName.E, Accidental.Natural), key.SpellPitchClass(4));
        Assert.Equal((NoteName.F, Accidental.Natural), key.SpellPitchClass(5));
        Assert.Equal((NoteName.G, Accidental.Natural), key.SpellPitchClass(7));
        Assert.Equal((NoteName.A, Accidental.Natural), key.SpellPitchClass(9));
        Assert.Equal((NoteName.B, Accidental.Natural), key.SpellPitchClass(11));
    }

    [Fact]
    public void CDorian_SpellsEbAndBb()
    {
        var key = new Key(NoteName.C, Accidental.Natural, Mode.Dorian);

        Assert.Equal((NoteName.C, Accidental.Natural), key.SpellPitchClass(0));
        Assert.Equal((NoteName.D, Accidental.Natural), key.SpellPitchClass(2));
        Assert.Equal((NoteName.E, Accidental.Flat), key.SpellPitchClass(3));
        Assert.Equal((NoteName.F, Accidental.Natural), key.SpellPitchClass(5));
        Assert.Equal((NoteName.G, Accidental.Natural), key.SpellPitchClass(7));
        Assert.Equal((NoteName.A, Accidental.Natural), key.SpellPitchClass(9));
        Assert.Equal((NoteName.B, Accidental.Flat), key.SpellPitchClass(10));
    }

    [Fact]
    public void FMajor_SpellsBb()
    {
        var key = new Key(NoteName.F, Accidental.Natural, Mode.Major);

        Assert.Equal((NoteName.F, Accidental.Natural), key.SpellPitchClass(5));
        Assert.Equal((NoteName.G, Accidental.Natural), key.SpellPitchClass(7));
        Assert.Equal((NoteName.A, Accidental.Natural), key.SpellPitchClass(9));
        Assert.Equal((NoteName.B, Accidental.Flat), key.SpellPitchClass(10));
        Assert.Equal((NoteName.C, Accidental.Natural), key.SpellPitchClass(0));
        Assert.Equal((NoteName.D, Accidental.Natural), key.SpellPitchClass(2));
        Assert.Equal((NoteName.E, Accidental.Natural), key.SpellPitchClass(4));
    }

    [Fact]
    public void BbMajor_SpellsBbAndEb()
    {
        var key = new Key(NoteName.B, Accidental.Flat, Mode.Major);

        Assert.Equal((NoteName.B, Accidental.Flat), key.SpellPitchClass(10));
        Assert.Equal((NoteName.C, Accidental.Natural), key.SpellPitchClass(0));
        Assert.Equal((NoteName.D, Accidental.Natural), key.SpellPitchClass(2));
        Assert.Equal((NoteName.E, Accidental.Flat), key.SpellPitchClass(3));
        Assert.Equal((NoteName.F, Accidental.Natural), key.SpellPitchClass(5));
        Assert.Equal((NoteName.G, Accidental.Natural), key.SpellPitchClass(7));
        Assert.Equal((NoteName.A, Accidental.Natural), key.SpellPitchClass(9));
    }

    [Fact]
    public void DMajor_SpellsFSharpAndCSharp()
    {
        var key = new Key(NoteName.D, Accidental.Natural, Mode.Major);

        Assert.Equal((NoteName.D, Accidental.Natural), key.SpellPitchClass(2));
        Assert.Equal((NoteName.E, Accidental.Natural), key.SpellPitchClass(4));
        Assert.Equal((NoteName.F, Accidental.Sharp), key.SpellPitchClass(6));
        Assert.Equal((NoteName.G, Accidental.Natural), key.SpellPitchClass(7));
        Assert.Equal((NoteName.A, Accidental.Natural), key.SpellPitchClass(9));
        Assert.Equal((NoteName.B, Accidental.Natural), key.SpellPitchClass(11));
        Assert.Equal((NoteName.C, Accidental.Sharp), key.SpellPitchClass(1));
    }

    [Fact]
    public void NonScaleTones_FallBackToSharpDefault()
    {
        // C Major: pitch classes 1, 3, 6, 8, 10 are non-scale tones
        var key = new Key(NoteName.C, Accidental.Natural, Mode.Major);

        Assert.Equal((NoteName.C, Accidental.Sharp), key.SpellPitchClass(1));
        Assert.Equal((NoteName.D, Accidental.Sharp), key.SpellPitchClass(3));
        Assert.Equal((NoteName.F, Accidental.Sharp), key.SpellPitchClass(6));
        Assert.Equal((NoteName.G, Accidental.Sharp), key.SpellPitchClass(8));
        Assert.Equal((NoteName.A, Accidental.Sharp), key.SpellPitchClass(10));
    }

    [Fact]
    public void AbMajor_SpellsFourFlats()
    {
        // Ab major: Ab, Bb, C, Db, Eb, F, G
        var key = new Key(NoteName.A, Accidental.Flat, Mode.Major);

        Assert.Equal((NoteName.A, Accidental.Flat), key.SpellPitchClass(8));
        Assert.Equal((NoteName.B, Accidental.Flat), key.SpellPitchClass(10));
        Assert.Equal((NoteName.C, Accidental.Natural), key.SpellPitchClass(0));
        Assert.Equal((NoteName.D, Accidental.Flat), key.SpellPitchClass(1));
        Assert.Equal((NoteName.E, Accidental.Flat), key.SpellPitchClass(3));
        Assert.Equal((NoteName.F, Accidental.Natural), key.SpellPitchClass(5));
        Assert.Equal((NoteName.G, Accidental.Natural), key.SpellPitchClass(7));
    }
}
