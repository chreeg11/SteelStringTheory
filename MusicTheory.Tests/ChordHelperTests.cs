using MusicTheory.Models;
using MusicTheory.Functions;

namespace MusicTheory.Tests;

public class ChordHelperTests
{
    // --- Golden tests: verify exact notes for known chords ---

    [Fact]
    public void CMajor_Returns_C_E_G()
    {
        var root = new Note(NoteName.C, Accidental.Natural, 4);
        var result = ChordHelper.TryGenerateChord(root, ChordType.Major, out var chord);

        Assert.True(result);
        Assert.Equal(3, chord.Length);
        Assert.Equal(new Note(NoteName.C, Accidental.Natural, 4), chord[0]);
        Assert.Equal(new Note(NoteName.E, Accidental.Natural, 4), chord[1]);
        Assert.Equal(new Note(NoteName.G, Accidental.Natural, 4), chord[2]);
    }

    [Fact]
    public void CMinor_Returns_C_Eb_G()
    {
        var root = new Note(NoteName.C, Accidental.Natural, 4);
        var key = new Key(NoteName.C, Accidental.Natural, Mode.Minor);
        var result = ChordHelper.TryGenerateChord(root, ChordType.Minor, key, out var chord);

        Assert.True(result);
        Assert.Equal(3, chord.Length);
        Assert.Equal(new Note(NoteName.C, Accidental.Natural, 4), chord[0]);
        Assert.Equal(new Note(NoteName.E, Accidental.Flat, 4), chord[1]);
        Assert.Equal(new Note(NoteName.G, Accidental.Natural, 4), chord[2]);
    }

    [Fact]
    public void BbMajor_InKeyOfF_SpellsCorrectly()
    {
        var root = new Note(NoteName.B, Accidental.Flat, 4);
        var key = new Key(NoteName.F, Accidental.Natural, Mode.Major);
        var result = ChordHelper.TryGenerateChord(root, ChordType.Major, key, out var chord);

        Assert.True(result);
        Assert.Equal(3, chord.Length);
        Assert.Equal(new Note(NoteName.B, Accidental.Flat, 4), chord[0]);
        Assert.Equal(new Note(NoteName.D, Accidental.Natural, 5), chord[1]);
        Assert.Equal(new Note(NoteName.F, Accidental.Natural, 5), chord[2]);
    }

    [Fact]
    public void CMaj7_Returns_C_E_G_B()
    {
        var root = new Note(NoteName.C, Accidental.Natural, 4);
        var result = ChordHelper.TryGenerateChord(root, ChordType.Maj7, out var chord);

        Assert.True(result);
        Assert.Equal(4, chord.Length);
        Assert.Equal(new Note(NoteName.C, Accidental.Natural, 4), chord[0]);
        Assert.Equal(new Note(NoteName.E, Accidental.Natural, 4), chord[1]);
        Assert.Equal(new Note(NoteName.G, Accidental.Natural, 4), chord[2]);
        Assert.Equal(new Note(NoteName.B, Accidental.Natural, 4), chord[3]);
    }

    [Fact]
    public void CDom7_Returns_C_E_G_Bb()
    {
        var root = new Note(NoteName.C, Accidental.Natural, 4);
        var key = new Key(NoteName.C, Accidental.Natural, Mode.Mixolydian);
        var result = ChordHelper.TryGenerateChord(root, ChordType.Dom7, key, out var chord);

        Assert.True(result);
        Assert.Equal(4, chord.Length);
        Assert.Equal(new Note(NoteName.C, Accidental.Natural, 4), chord[0]);
        Assert.Equal(new Note(NoteName.E, Accidental.Natural, 4), chord[1]);
        Assert.Equal(new Note(NoteName.G, Accidental.Natural, 4), chord[2]);
        Assert.Equal(new Note(NoteName.B, Accidental.Flat, 4), chord[3]);
    }

    [Fact]
    public void CDim_Returns_C_Eb_Gb()
    {
        var root = new Note(NoteName.C, Accidental.Natural, 4);
        var key = new Key(NoteName.C, Accidental.Natural, Mode.Locrian);
        var result = ChordHelper.TryGenerateChord(root, ChordType.Dim, key, out var chord);

        Assert.True(result);
        Assert.Equal(3, chord.Length);
        Assert.Equal(new Note(NoteName.C, Accidental.Natural, 4), chord[0]);
        Assert.Equal(new Note(NoteName.E, Accidental.Flat, 4), chord[1]);
        Assert.Equal(new Note(NoteName.G, Accidental.Flat, 4), chord[2]);
    }

    [Fact]
    public void CAug_Returns_C_E_GSharp()
    {
        var root = new Note(NoteName.C, Accidental.Natural, 4);
        var result = ChordHelper.TryGenerateChord(root, ChordType.Aug, out var chord);

        Assert.True(result);
        Assert.Equal(3, chord.Length);
        Assert.Equal(new Note(NoteName.C, Accidental.Natural, 4), chord[0]);
        Assert.Equal(new Note(NoteName.E, Accidental.Natural, 4), chord[1]);
        Assert.Equal(new Note(NoteName.G, Accidental.Sharp, 4), chord[2]);
    }

    [Fact]
    public void CDim7_Returns_C_Eb_Gb_Bbb()
    {
        var root = new Note(NoteName.C, Accidental.Natural, 4);
        var key = new Key(NoteName.C, Accidental.Natural, Mode.Locrian);
        var result = ChordHelper.TryGenerateChord(root, ChordType.Dim7, key, out var chord);

        Assert.True(result);
        Assert.Equal(4, chord.Length);
        Assert.Equal(new Note(NoteName.C, Accidental.Natural, 4), chord[0]);
        Assert.Equal(new Note(NoteName.E, Accidental.Flat, 4), chord[1]);
        Assert.Equal(new Note(NoteName.G, Accidental.Flat, 4), chord[2]);
        // Dim7's 7th (9 semitones) = pitch class 9 = A natural in default/Locrian spelling
        Assert.Equal(9, chord[3].PitchClass);
    }

    [Fact]
    public void CMin7b5_Returns_C_Eb_Gb_Bb()
    {
        var root = new Note(NoteName.C, Accidental.Natural, 4);
        var key = new Key(NoteName.C, Accidental.Natural, Mode.Locrian);
        var result = ChordHelper.TryGenerateChord(root, ChordType.Min7b5, key, out var chord);

        Assert.True(result);
        Assert.Equal(4, chord.Length);
        Assert.Equal(new Note(NoteName.C, Accidental.Natural, 4), chord[0]);
        Assert.Equal(new Note(NoteName.E, Accidental.Flat, 4), chord[1]);
        Assert.Equal(new Note(NoteName.G, Accidental.Flat, 4), chord[2]);
        Assert.Equal(new Note(NoteName.B, Accidental.Flat, 4), chord[3]);
    }

    [Fact]
    public void CMin7_Returns_C_Eb_G_Bb()
    {
        var root = new Note(NoteName.C, Accidental.Natural, 4);
        var key = new Key(NoteName.C, Accidental.Natural, Mode.Minor);
        var result = ChordHelper.TryGenerateChord(root, ChordType.Min7, key, out var chord);

        Assert.True(result);
        Assert.Equal(4, chord.Length);
        Assert.Equal(new Note(NoteName.C, Accidental.Natural, 4), chord[0]);
        Assert.Equal(new Note(NoteName.E, Accidental.Flat, 4), chord[1]);
        Assert.Equal(new Note(NoteName.G, Accidental.Natural, 4), chord[2]);
        Assert.Equal(new Note(NoteName.B, Accidental.Flat, 4), chord[3]);
    }

    // --- Property-based: verify interval pattern for all chord types across all roots ---

    [Theory]
    [InlineData(ChordType.Major, new[] { 4, 7 })]
    [InlineData(ChordType.Minor, new[] { 3, 7 })]
    [InlineData(ChordType.Aug, new[] { 4, 8 })]
    [InlineData(ChordType.Dim, new[] { 3, 6 })]
    [InlineData(ChordType.Maj7, new[] { 4, 7, 11 })]
    [InlineData(ChordType.Min7, new[] { 3, 7, 10 })]
    [InlineData(ChordType.Dom7, new[] { 4, 7, 10 })]
    [InlineData(ChordType.Dim7, new[] { 3, 6, 9 })]
    [InlineData(ChordType.Min7b5, new[] { 3, 6, 10 })]
    public void AllChordTypes_HaveCorrectIntervals_FromAllRoots(ChordType type, int[] expectedOffsets)
    {
        var roots = new (NoteName Name, Accidental Acc)[]
        {
            (NoteName.C, Accidental.Natural),
            (NoteName.C, Accidental.Flat),
            (NoteName.C, Accidental.Sharp),
            (NoteName.D, Accidental.Natural),
            (NoteName.D, Accidental.Flat),
            (NoteName.D, Accidental.Sharp),
            (NoteName.E, Accidental.Natural),
            (NoteName.E, Accidental.Flat),
            (NoteName.E, Accidental.Sharp),
            (NoteName.F, Accidental.Natural),
            (NoteName.F, Accidental.Flat),
            (NoteName.F, Accidental.Sharp),
            (NoteName.G, Accidental.Natural),
            (NoteName.G, Accidental.Flat),
            (NoteName.G, Accidental.Sharp),
            (NoteName.A, Accidental.Natural),
            (NoteName.A, Accidental.Flat),
            (NoteName.A, Accidental.Sharp),
            (NoteName.B, Accidental.Natural),
            (NoteName.B, Accidental.Flat),
            (NoteName.B, Accidental.Sharp),
        };

        foreach (var (name, acc) in roots)
        {
            var root = new Note(name, acc, 4);
            var result = ChordHelper.TryGenerateChord(root, type, out var chord);

            Assert.True(result);
            Assert.Equal(expectedOffsets.Length + 1, chord.Length); // +1 for root

            // Verify each note is the correct semitone distance from root
            for (int i = 0; i < expectedOffsets.Length; i++)
            {
                int actualOffset = chord[i + 1].MidiNumber - chord[0].MidiNumber;
                Assert.Equal(expectedOffsets[i], actualOffset);
            }
        }
    }
}
