using MusicTheory.Models;
using PedalSteel.Data;
using PedalSteel.Models;

namespace PedalSteel.Tests;

public class CopedentTests
{
    // --- CopedentFactory.CreateE9() ---

    [Fact]
    public void CreateE9_HasCorrectName()
    {
        var e9 = CopedentFactory.CreateE9();
        Assert.Equal("E9 Standard", e9.Name);
    }

    [Fact]
    public void CreateE9_HasTenStrings()
    {
        var e9 = CopedentFactory.CreateE9();
        Assert.Equal(10, e9.Strings.Count);
    }

    [Theory]
    [InlineData(1,  NoteName.F, Accidental.Sharp,   4)]
    [InlineData(2,  NoteName.D, Accidental.Sharp,   4)]
    [InlineData(3,  NoteName.G, Accidental.Sharp,   4)]
    [InlineData(4,  NoteName.E, Accidental.Natural, 4)]
    [InlineData(5,  NoteName.B, Accidental.Natural, 3)]
    [InlineData(6,  NoteName.G, Accidental.Sharp,   3)]
    [InlineData(7,  NoteName.F, Accidental.Sharp,   3)]
    [InlineData(8,  NoteName.E, Accidental.Natural, 3)]
    [InlineData(9,  NoteName.D, Accidental.Natural, 3)]
    [InlineData(10, NoteName.B, Accidental.Natural, 2)]
    public void CreateE9_StringTunings_AreCorrect(uint number, NoteName name, Accidental accidental, int octave)
    {
        var e9 = CopedentFactory.CreateE9();
        var str = e9.Strings.Single(s => s.Number == number);
        Assert.Equal(name, str.OpenNote.NoteName);
        Assert.Equal(accidental, str.OpenNote.Accidental);
        Assert.Equal(octave, str.OpenNote.Octave);
    }

    [Fact]
    public void CreateE9_HasThreePedals()
    {
        var e9 = CopedentFactory.CreateE9();
        Assert.Equal(3, e9.PedalEffects.Count);
    }

    [Fact]
    public void CreateE9_APedal_RaisesStrings5And10_By2Semitones()
    {
        var e9 = CopedentFactory.CreateE9();
        var effects = e9.PedalEffects[new Pedal("A")];
        Assert.Equal(2, effects.Count);
        Assert.Contains(effects, e => e.StringNumber == 5 && e.SemitonesDelta == 2);
        Assert.Contains(effects, e => e.StringNumber == 10 && e.SemitonesDelta == 2);
    }

    [Fact]
    public void CreateE9_BPedal_RaisesStrings3And6_By1Semitone()
    {
        var e9 = CopedentFactory.CreateE9();
        var effects = e9.PedalEffects[new Pedal("B")];
        Assert.Equal(2, effects.Count);
        Assert.Contains(effects, e => e.StringNumber == 3 && e.SemitonesDelta == 1);
        Assert.Contains(effects, e => e.StringNumber == 6 && e.SemitonesDelta == 1);
    }

    [Fact]
    public void CreateE9_CPedal_RaisesString4_By2Semitones()
    {
        var e9 = CopedentFactory.CreateE9();
        var effects = e9.PedalEffects[new Pedal("C")];
        Assert.Single(effects);
        Assert.Contains(effects, e => e.StringNumber == 4 && e.SemitonesDelta == 2);
    }

    // --- Copedent constructor validation ---

    [Fact]
    public void Copedent_InvalidStringNumber_ThrowsArgumentException()
    {
        var strings = new List<GuitarString>
        {
            new(1, new Note(NoteName.B, Accidental.Natural, 3))
        };

        var pedalEffects = new Dictionary<Pedal, IReadOnlyList<StringEffect>>
        {
            [new Pedal("A")] = new List<StringEffect> { new(99, 2) } // string 99 doesn't exist
        };

        var ex = Assert.Throws<ArgumentException>(() =>
            new Copedent("Test", strings, pedalEffects));

        Assert.Contains("99", ex.Message);
        Assert.Contains("A", ex.Message);
    }

    [Fact]
    public void Copedent_ValidStringNumbers_DoesNotThrow()
    {
        var strings = new List<GuitarString>
        {
            new(1, new Note(NoteName.B, Accidental.Natural, 3)),
            new(2, new Note(NoteName.E, Accidental.Natural, 4))
        };

        var pedalEffects = new Dictionary<Pedal, IReadOnlyList<StringEffect>>
        {
            [new Pedal("A")] = new List<StringEffect> { new(1, 2), new(2, 1) }
        };

        var copedent = new Copedent("Test", strings, pedalEffects);
        Assert.NotNull(copedent);
    }

    [Fact]
    public void Copedent_NoPedals_IsValid()
    {
        var strings = new List<GuitarString>
        {
            new(1, new Note(NoteName.B, Accidental.Natural, 3))
        };

        var copedent = new Copedent("Open Only", strings, new Dictionary<Pedal, IReadOnlyList<StringEffect>>());
        Assert.Empty(copedent.PedalEffects);
    }
}
