using MusicTheory.Models;
using MusicTheory.Functions;

namespace MusicTheory.Tests;

public class NoteHelperTests
{
    // --- Transpose Tests ---

    [Fact]
    public void Transpose_C4_Up4Semitones_ReturnsE4()
    {
        var c4 = new Note(NoteName.C, Accidental.Natural, 4);

        var result = NoteHelper.Transpose(c4, 4);

        Assert.Equal(4, result.PitchClass);  // E = pitch class 4
        Assert.Equal(4, result.Octave);
    }

    [Fact]
    public void Transpose_C4_Up12Semitones_ReturnsC5()
    {
        var c4 = new Note(NoteName.C, Accidental.Natural, 4);

        var result = NoteHelper.Transpose(c4, 12);

        Assert.Equal(0, result.PitchClass);  // C = pitch class 0
        Assert.Equal(5, result.Octave);
    }

    [Fact]
    public void Transpose_C4_DownOneSemitone_ReturnsB3()
    {
        var c4 = new Note(NoteName.C, Accidental.Natural, 4);

        var result = NoteHelper.Transpose(c4, -1);

        Assert.Equal(11, result.PitchClass);  // B = pitch class 11
        Assert.Equal(3, result.Octave);
    }

    [Fact]
    public void Transpose_PreservesMidiMath()
    {
        // Db4 and C#4 are the same pitch — transposing either up 2
        // should give the same MidiNumber
        var db4 = new Note(NoteName.D, Accidental.Flat, 4);
        var cs4 = new Note(NoteName.C, Accidental.Sharp, 4);

        var fromDb = NoteHelper.Transpose(db4, 2);
        var fromCs = NoteHelper.Transpose(cs4, 2);

        Assert.Equal(fromDb.MidiNumber, fromCs.MidiNumber);
    }

    // --- IntervalBetween Tests ---

    [Theory]
    [InlineData(NoteName.C, Accidental.Natural, NoteName.E, Accidental.Natural, 4)]  // C to E = major 3rd
    [InlineData(NoteName.C, Accidental.Natural, NoteName.G, Accidental.Natural, 7)]  // C to G = perfect 5th
    [InlineData(NoteName.E, Accidental.Natural, NoteName.C, Accidental.Natural, 8)]  // E to C = minor 6th (ascending)
    [InlineData(NoteName.C, Accidental.Natural, NoteName.C, Accidental.Natural, 0)]  // Same note = unison
    public void IntervalBetween_ReturnsCorrectSemitones(
        NoteName rootName, Accidental rootAcc,
        NoteName targetName, Accidental targetAcc,
        int expectedSemitones)
    {
        var root = new Note(rootName, rootAcc, 4);
        var target = new Note(targetName, targetAcc, 4);

        var result = NoteHelper.IntervalBetween(root, target);

        Assert.Equal(expectedSemitones, result);
    }

    [Fact]
    public void IntervalBetween_EnharmonicNotes_ReturnZero()
    {
        // C# and Db are the same pitch class — interval should be 0
        var cs = new Note(NoteName.C, Accidental.Sharp, 4);
        var db = new Note(NoteName.D, Accidental.Flat, 4);

        Assert.Equal(0, NoteHelper.IntervalBetween(cs, db));
    }
}

public class IntervalHelperTests
{
    [Theory]
    [InlineData(0, "Unison")]
    [InlineData(4, "Major 3rd")]
    [InlineData(7, "Perfect 5th")]
    [InlineData(12, "Unison")]      // Octave wraps to 0
    [InlineData(-1, "Major 7th")]   // Negative wraps correctly
    public void GetIntervalName_ReturnsCorrectName(int semitones, string expected)
    {
        Assert.Equal(expected, IntervalHelper.GetIntervalName(semitones));
    }

    [Fact]
    public void GetInterval_C4ToE4_Returns4SemitonesAscending()
    {
        var c4 = new Note(NoteName.C, Accidental.Natural, 4);
        var e4 = new Note(NoteName.E, Accidental.Natural, 4);

        var interval = IntervalHelper.GetInterval(c4, e4);

        Assert.Equal(4, interval.Semitones);
        Assert.False(interval.IsDescending);
    }

    [Fact]
    public void GetInterval_E4ToC4_Returns4SemitonesDescending()
    {
        var e4 = new Note(NoteName.E, Accidental.Natural, 4);
        var c4 = new Note(NoteName.C, Accidental.Natural, 4);

        var interval = IntervalHelper.GetInterval(e4, c4);

        Assert.Equal(4, interval.Semitones);
        Assert.True(interval.IsDescending);
    }
}
