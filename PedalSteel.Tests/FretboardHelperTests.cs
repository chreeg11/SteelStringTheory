using MusicTheory.Models;
using PedalSteel.Data;
using PedalSteel.Functions;
using PedalSteel.Models;

namespace PedalSteel.Tests;

public class FretboardHelperTests
{
    private readonly Copedent _e9 = CopedentFactory.CreateE9();

    // --- Open string (fret 0, no pedals) ---

    [Fact]
    public void GetNoteAtFret_OpenString_NoPedals_ReturnsOpenNote()
    {
        var string5 = _e9.Strings.Single(s => s.Number == 5); // B3

        var result = FretboardHelper.GetNoteAtFret(string5, 0, _e9, PedalState.Open);

        Assert.Equal(NoteName.B, result.NoteName);
        Assert.Equal(Accidental.Natural, result.Accidental);
        Assert.Equal(3, result.Octave);
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
    public void GetNoteAtFret_Fret0_NoPedals_ReturnsOpenTuning(uint stringNumber, NoteName name, Accidental accidental, int octave)
    {
        var gs = _e9.Strings.Single(s => s.Number == stringNumber);

        var result = FretboardHelper.GetNoteAtFret(gs, 0, _e9, PedalState.Open);

        Assert.Equal(name, result.NoteName);
        Assert.Equal(accidental, result.Accidental);
        Assert.Equal(octave, result.Octave);
    }

    // --- Fret climbing, no pedals ---

    [Fact]
    public void GetNoteAtFret_String5_Fret3_NoPedals_ReturnsD4()
    {
        var string5 = _e9.Strings.Single(s => s.Number == 5); // B3 + 3 = D4

        var result = FretboardHelper.GetNoteAtFret(string5, 3, _e9, PedalState.Open);

        Assert.Equal(NoteName.D, result.NoteName);
        Assert.Equal(Accidental.Natural, result.Accidental);
        Assert.Equal(4, result.Octave);
    }

    [Fact]
    public void GetNoteAtFret_Fret12_ReturnsOctaveAboveOpen()
    {
        var string5 = _e9.Strings.Single(s => s.Number == 5); // B3, fret 12 = B4

        var result = FretboardHelper.GetNoteAtFret(string5, 12, _e9, PedalState.Open);

        Assert.Equal(NoteName.B, result.NoteName);
        Assert.Equal(Accidental.Natural, result.Accidental);
        Assert.Equal(4, result.Octave);
    }

    // --- A pedal (+2 semitones on strings 5 & 10) ---

    [Fact]
    public void GetNoteAtFret_String5_Fret0_APedal_ReturnsCSharp4()
    {
        var string5 = _e9.Strings.Single(s => s.Number == 5); // B3 + 2 = C#4
        var pedalState = PedalState.With(new Pedal("A"));

        var result = FretboardHelper.GetNoteAtFret(string5, 0, _e9, pedalState);

        Assert.Equal(NoteName.C, result.NoteName);
        Assert.Equal(Accidental.Sharp, result.Accidental);
        Assert.Equal(4, result.Octave);
    }

    [Fact]
    public void GetNoteAtFret_String10_Fret0_APedal_ReturnsCSharp3()
    {
        var string10 = _e9.Strings.Single(s => s.Number == 10); // B2 + 2 = C#3
        var pedalState = PedalState.With(new Pedal("A"));

        var result = FretboardHelper.GetNoteAtFret(string10, 0, _e9, pedalState);

        Assert.Equal(NoteName.C, result.NoteName);
        Assert.Equal(Accidental.Sharp, result.Accidental);
        Assert.Equal(3, result.Octave);
    }

    [Fact]
    public void GetNoteAtFret_String5_Fret3_APedal_ReturnsE4()
    {
        var string5 = _e9.Strings.Single(s => s.Number == 5); // B3 + 3 frets + 2 pedal = E4
        var pedalState = PedalState.With(new Pedal("A"));

        var result = FretboardHelper.GetNoteAtFret(string5, 3, _e9, pedalState);

        Assert.Equal(NoteName.E, result.NoteName);
        Assert.Equal(Accidental.Natural, result.Accidental);
        Assert.Equal(4, result.Octave);
    }

    // --- B pedal (+1 semitone on strings 3 & 6) ---

    [Fact]
    public void GetNoteAtFret_String3_Fret0_BPedal_ReturnsA4()
    {
        var string3 = _e9.Strings.Single(s => s.Number == 3); // G#4 + 1 = A4
        var pedalState = PedalState.With(new Pedal("B"));

        var result = FretboardHelper.GetNoteAtFret(string3, 0, _e9, pedalState);

        Assert.Equal(NoteName.A, result.NoteName);
        Assert.Equal(Accidental.Natural, result.Accidental);
        Assert.Equal(4, result.Octave);
    }

    [Fact]
    public void GetNoteAtFret_String6_Fret0_BPedal_ReturnsA3()
    {
        var string6 = _e9.Strings.Single(s => s.Number == 6); // G#3 + 1 = A3
        var pedalState = PedalState.With(new Pedal("B"));

        var result = FretboardHelper.GetNoteAtFret(string6, 0, _e9, pedalState);

        Assert.Equal(NoteName.A, result.NoteName);
        Assert.Equal(Accidental.Natural, result.Accidental);
        Assert.Equal(3, result.Octave);
    }

    // --- C pedal (+2 semitones on string 4) ---

    [Fact]
    public void GetNoteAtFret_String4_Fret0_CPedal_ReturnsFSharp4()
    {
        var string4 = _e9.Strings.Single(s => s.Number == 4); // E4 + 2 = F#4
        var pedalState = PedalState.With(new Pedal("C"));

        var result = FretboardHelper.GetNoteAtFret(string4, 0, _e9, pedalState);

        Assert.Equal(NoteName.F, result.NoteName);
        Assert.Equal(Accidental.Sharp, result.Accidental);
        Assert.Equal(4, result.Octave);
    }

    // --- Multiple pedals ---

    [Fact]
    public void GetNoteAtFret_String3_Fret0_ABPedals_OnlyBPedalAffectsString3()
    {
        var string3 = _e9.Strings.Single(s => s.Number == 3); // G#4, A affects 5&10, B affects 3&6
        var pedalState = PedalState.With(new Pedal("A"), new Pedal("B"));

        var result = FretboardHelper.GetNoteAtFret(string3, 0, _e9, pedalState);

        // Only B pedal affects string 3: G#4 + 1 = A4
        Assert.Equal(NoteName.A, result.NoteName);
        Assert.Equal(Accidental.Natural, result.Accidental);
        Assert.Equal(4, result.Octave);
    }

    // --- Pedal doesn't affect unrelated strings ---

    [Fact]
    public void GetNoteAtFret_String1_Fret0_APedal_UnaffectedByPedal()
    {
        var string1 = _e9.Strings.Single(s => s.Number == 1); // F#4, A pedal doesn't affect string 1
        var pedalState = PedalState.With(new Pedal("A"));

        var result = FretboardHelper.GetNoteAtFret(string1, 0, _e9, pedalState);

        Assert.Equal(NoteName.F, result.NoteName);
        Assert.Equal(Accidental.Sharp, result.Accidental);
        Assert.Equal(4, result.Octave);
    }

    // --- Edge cases ---

    [Fact]
    public void GetNoteAtFret_NegativeFret_ThrowsArgumentException()
    {
        var string5 = _e9.Strings.Single(s => s.Number == 5);

        Assert.Throws<ArgumentException>(() =>
            FretboardHelper.GetNoteAtFret(string5, -1, _e9, PedalState.Open));
    }
}
