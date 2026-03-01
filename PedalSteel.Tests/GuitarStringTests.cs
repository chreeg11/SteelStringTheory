using MusicTheory.Models;
using PedalSteel.Models;

namespace PedalSteel.Tests;

public class GuitarStringTests
{
    // E9 standard tuning (strings 1-10, high to low)
    // 1:F#4, 2:D#4, 3:G#4, 4:E4, 5:B3, 6:G#3, 7:F#3, 8:E3, 9:D3, 10:B2

    [Fact]
    public void GuitarString_StoresNumberAndOpenNote()
    {
        var lowB = new GuitarString(10, new Note(NoteName.B, Accidental.Natural, 2));
        Assert.Equal(10u, lowB.Number);
        Assert.Equal(NoteName.B, lowB.OpenNote.NoteName);
        Assert.Equal(Accidental.Natural, lowB.OpenNote.Accidental);
        Assert.Equal(2, lowB.OpenNote.Octave);
    }

    [Fact]
    public void GuitarString_HighFString_IsString1()
    {
        var highF = new GuitarString(1, new Note(NoteName.F, Accidental.Sharp, 4));
        Assert.Equal(1u, highF.Number);
        Assert.Equal(NoteName.F, highF.OpenNote.NoteName);
        Assert.Equal(Accidental.Sharp, highF.OpenNote.Accidental);
        Assert.Equal(4, highF.OpenNote.Octave);
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
    public void E9_StandardTuning_AllStrings(uint number, NoteName name, Accidental accidental, int octave)
    {
        var gs = new GuitarString(number, new Note(name, accidental, octave));
        Assert.Equal(number, gs.Number);
        Assert.Equal(name, gs.OpenNote.NoteName);
        Assert.Equal(accidental, gs.OpenNote.Accidental);
        Assert.Equal(octave, gs.OpenNote.Octave);
    }

    [Fact]
    public void GuitarString_ValueEquality_SameStringAndNote()
    {
        var a = new GuitarString(10, new Note(NoteName.B, Accidental.Natural, 2));
        var b = new GuitarString(10, new Note(NoteName.B, Accidental.Natural, 2));
        Assert.Equal(a, b);
    }

    [Fact]
    public void GuitarString_ValueEquality_DifferentNumber_NotEqual()
    {
        var a = new GuitarString(9,  new Note(NoteName.B, Accidental.Natural, 2));
        var b = new GuitarString(10, new Note(NoteName.B, Accidental.Natural, 2));
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void GuitarString_OpenNote_HasCorrectMidiNumber()
    {
        // B2 = MIDI 47
        var lowB = new GuitarString(10, new Note(NoteName.B, Accidental.Natural, 2));
        Assert.Equal(47, lowB.OpenNote.MidiNumber);
    }

    [Fact]
    public void GuitarString_OpenNote_HasCorrectPitchClass()
    {
        // B = pitch class 11
        var lowB = new GuitarString(10, new Note(NoteName.B, Accidental.Natural, 2));
        Assert.Equal(11, lowB.OpenNote.PitchClass);
    }
}
