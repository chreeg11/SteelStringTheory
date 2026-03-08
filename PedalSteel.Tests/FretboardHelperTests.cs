using MusicTheory.Functions;
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

    // --- E lever (-1 semitone on strings 4 & 8) ---

    [Fact]
    public void GetNoteAtFret_String4_Fret0_ELever_ReturnsDSharp4()
    {
        var string4 = _e9.Strings.Single(s => s.Number == 4); // E4 - 1 = D#4
        var pedalState = PedalState.With(new Pedal("E"));

        var result = FretboardHelper.GetNoteAtFret(string4, 0, _e9, pedalState);

        Assert.Equal(NoteName.D, result.NoteName);
        Assert.Equal(Accidental.Sharp, result.Accidental);
        Assert.Equal(4, result.Octave);
    }

    [Fact]
    public void GetNoteAtFret_String8_Fret0_ELever_ReturnsDSharp3()
    {
        var string8 = _e9.Strings.Single(s => s.Number == 8); // E3 - 1 = D#3
        var pedalState = PedalState.With(new Pedal("E"));

        var result = FretboardHelper.GetNoteAtFret(string8, 0, _e9, pedalState);

        Assert.Equal(NoteName.D, result.NoteName);
        Assert.Equal(Accidental.Sharp, result.Accidental);
        Assert.Equal(3, result.Octave);
    }

    // --- F lever (+1 semitone on strings 4 & 8) ---

    [Fact]
    public void GetNoteAtFret_String4_Fret0_FLever_ReturnsF4()
    {
        var string4 = _e9.Strings.Single(s => s.Number == 4); // E4 + 1 = F4
        var pedalState = PedalState.With(new Pedal("F"));

        var result = FretboardHelper.GetNoteAtFret(string4, 0, _e9, pedalState);

        Assert.Equal(NoteName.F, result.NoteName);
        Assert.Equal(Accidental.Natural, result.Accidental);
        Assert.Equal(4, result.Octave);
    }

    [Fact]
    public void GetNoteAtFret_String8_Fret0_FLever_ReturnsF3()
    {
        var string8 = _e9.Strings.Single(s => s.Number == 8); // E3 + 1 = F3
        var pedalState = PedalState.With(new Pedal("F"));

        var result = FretboardHelper.GetNoteAtFret(string8, 0, _e9, pedalState);

        Assert.Equal(NoteName.F, result.NoteName);
        Assert.Equal(Accidental.Natural, result.Accidental);
        Assert.Equal(3, result.Octave);
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

    // ==========================================================
    // GetScalePositions (fixed pedal state)
    // ==========================================================

    [Fact]
    public void GetScalePositions_CMajor_Open_AllResultsAreScaleTones()
    {
        var cMajorPitchClasses = new HashSet<int> { 0, 2, 4, 5, 7, 9, 11 };
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var positions = FretboardHelper.GetScalePositions(_e9, PedalState.Open, scale).ToList();

        Assert.All(positions, p => Assert.Contains(p.Note.PitchClass, cMajorPitchClasses));
    }

    [Fact]
    public void GetScalePositions_CMajor_Open_CoversAllStrings()
    {
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var positions = FretboardHelper.GetScalePositions(_e9, PedalState.Open, scale).ToList();
        var stringsHit = positions.Select(p => p.StringNumber).Distinct();

        // Every string has at least some C Major tones across 25 frets
        Assert.Equal(10, stringsHit.Count());
    }

    [Fact]
    public void GetScalePositions_CMajor_Open_Fret0_MatchesManualCalculation()
    {
        // Open E9 pitch classes at fret 0: F#(6), D#(3), G#(8), E(4), B(11), G#(8), F#(6), E(4), D(2), B(11)
        // C Major pitch classes: {0, 2, 4, 5, 7, 9, 11}
        // Matches at fret 0: E(4) on strings 4,8; B(11) on strings 5,10; D(2) on string 9
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var positions = FretboardHelper.GetScalePositions(_e9, PedalState.Open, scale, minFret: 0, maxFret: 0).ToList();

        Assert.Equal(5, positions.Count);
        Assert.Contains(positions, p => p.StringNumber == 4 && p.Fret == 0);  // E4
        Assert.Contains(positions, p => p.StringNumber == 5 && p.Fret == 0);  // B3
        Assert.Contains(positions, p => p.StringNumber == 8 && p.Fret == 0);  // E3
        Assert.Contains(positions, p => p.StringNumber == 9 && p.Fret == 0);  // D3
        Assert.Contains(positions, p => p.StringNumber == 10 && p.Fret == 0); // B2
    }

    [Fact]
    public void GetScalePositions_ConstrainedFretRange_OnlyReturnsFretInRange()
    {
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var positions = FretboardHelper.GetScalePositions(_e9, PedalState.Open, scale, minFret: 5, maxFret: 7).ToList();

        Assert.All(positions, p =>
        {
            Assert.InRange(p.Fret, 5, 7);
        });
        Assert.NotEmpty(positions);
    }

    [Fact]
    public void GetScalePositions_EachPositionHasCorrectPedalState()
    {
        var pedalState = PedalState.With(new Pedal("A"));
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var positions = FretboardHelper.GetScalePositions(_e9, pedalState, scale).ToList();

        Assert.All(positions, p => Assert.Equal(pedalState, p.PedalState));
    }

    [Fact]
    public void GetScalePositions_NoteMatchesGetNoteAtFret()
    {
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var positions = FretboardHelper.GetScalePositions(_e9, PedalState.Open, scale, maxFret: 12).ToList();

        Assert.All(positions, p =>
        {
            var gs = _e9.Strings.Single(s => s.Number == p.StringNumber);
            var expected = FretboardHelper.GetNoteAtFret(gs, p.Fret, _e9, PedalState.Open);
            Assert.Equal(expected, p.Note);
        });
    }

    // ==========================================================
    // GetScalePositions (convenience overload: root + mode)
    // ==========================================================

    [Fact]
    public void GetScalePositions_ConvenienceOverload_MatchesExplicitScale()
    {
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var fromScale = FretboardHelper.GetScalePositions(_e9, PedalState.Open, scale).ToList();
        var fromOverload = FretboardHelper.GetScalePositions(
            _e9, PedalState.Open, NoteName.C, Accidental.Natural, Mode.Major).ToList();

        Assert.Equal(fromScale.Count, fromOverload.Count);
        Assert.Equal(fromScale, fromOverload);
    }

    // ==========================================================
    // GetScalePositionsAllPedalStates
    // ==========================================================

    [Fact]
    public void GetScalePositionsAllPedalStates_FindsMoreThanFixedOpen()
    {
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var openOnly = FretboardHelper.GetScalePositions(_e9, PedalState.Open, scale).ToList();
        var allStates = FretboardHelper.GetScalePositionsAllPedalStates(_e9, scale).ToList();

        Assert.True(allStates.Count > openOnly.Count);
    }

    [Fact]
    public void GetScalePositionsAllPedalStates_AllResultsAreScaleTones()
    {
        var cMajorPitchClasses = new HashSet<int> { 0, 2, 4, 5, 7, 9, 11 };
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var positions = FretboardHelper.GetScalePositionsAllPedalStates(_e9, scale).ToList();

        Assert.All(positions, p => Assert.Contains(p.Note.PitchClass, cMajorPitchClasses));
    }

    [Fact]
    public void GetScalePositionsAllPedalStates_IncludesOpenPedalState()
    {
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var positions = FretboardHelper.GetScalePositionsAllPedalStates(_e9, scale).ToList();

        Assert.Contains(positions, p => p.PedalState == PedalState.Open);
    }

    [Fact]
    public void GetScalePositionsAllPedalStates_IncludesNonOpenPedalStates()
    {
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var positions = FretboardHelper.GetScalePositionsAllPedalStates(_e9, scale).ToList();
        var pedalA = new Pedal("A");

        Assert.Contains(positions, p => p.PedalState.IsEngaged(pedalA));
    }

    [Fact]
    public void GetScalePositionsAllPedalStates_E9Has32PedalCombinations()
    {
        // E9 has 5 pedals/levers (A, B, C, E, F) → 2⁵ = 32 pedal states (including open)
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var positions = FretboardHelper.GetScalePositionsAllPedalStates(_e9, scale).ToList();

        var distinctStates = positions
            .Select(p => string.Join(",", p.PedalState.ActivePedals.Select(x => x.Name).OrderBy(n => n)))
            .Distinct()
            .ToList();

        Assert.Equal(32, distinctStates.Count);
    }

    [Fact]
    public void GetScalePositionsAllPedalStates_ConvenienceOverload_MatchesExplicitScale()
    {
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var fromScale = FretboardHelper.GetScalePositionsAllPedalStates(_e9, scale).ToList();
        var fromOverload = FretboardHelper.GetScalePositionsAllPedalStates(
            _e9, NoteName.C, Accidental.Natural, Mode.Major).ToList();

        Assert.Equal(fromScale.Count, fromOverload.Count);

        // Compare by string/fret/note since PedalState uses HashSet (reference equality)
        var scaleKeys = fromScale.Select(p => (p.StringNumber, p.Fret, p.Note)).ToList();
        var overloadKeys = fromOverload.Select(p => (p.StringNumber, p.Fret, p.Note)).ToList();
        Assert.Equal(scaleKeys, overloadKeys);
    }

    [Fact]
    public void GetScalePositionsAllPedalStates_NoteMatchesGetNoteAtFret()
    {
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var positions = FretboardHelper.GetScalePositionsAllPedalStates(_e9, scale, maxFret: 12).ToList();

        Assert.All(positions, p =>
        {
            var gs = _e9.Strings.Single(s => s.Number == p.StringNumber);
            var expected = FretboardHelper.GetNoteAtFret(gs, p.Fret, _e9, p.PedalState);
            Assert.Equal(expected, p.Note);
        });
    }

    // ==========================================================
    // Domain truth: C Major IS achievable at a single fret
    // by engaging different pedal combinations while barring
    // ==========================================================

    [Theory]
    [InlineData(3)]
    [InlineData(8)]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(20)]
    [InlineData(22)]
    public void GetScalePositionsAllPedalStates_CMajor_FullScaleAtSingleFret(int fret)
    {
        // On E9 pedal steel you can achieve all 7 C Major pitch classes at certain frets
        // by engaging different pedals while barring the same fret.
        // e.g. Fret 8 open gives C,D,E,G,B; A pedal adds A; B pedal adds F.
        var cMajorPitchClasses = new HashSet<int> { 0, 2, 4, 5, 7, 9, 11 };
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var allPositions = FretboardHelper.GetScalePositionsAllPedalStates(_e9, scale).ToList();
        var atThisFret = allPositions.Where(p => p.Fret == fret).ToList();
        var pitchClassesFound = new HashSet<int>(atThisFret.Select(p => p.Note.PitchClass));

        Assert.Superset(cMajorPitchClasses, pitchClassesFound);
    }

    [Fact]
    public void GetScalePositionsAllPedalStates_CMajor_Fret8_OpenGivesFiveScaleTones()
    {
        // Verify the specific pedal story at fret 8:
        // Open strings at fret 8 produce C(0), D(2), E(4), G(7), B(11) — 5 of 7
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var openAtFret8 = FretboardHelper.GetScalePositions(_e9, PedalState.Open, scale, minFret: 8, maxFret: 8).ToList();
        var openPcs = new HashSet<int>(openAtFret8.Select(p => p.Note.PitchClass));

        Assert.Contains(0, openPcs);  // C
        Assert.Contains(2, openPcs);  // D
        Assert.Contains(4, openPcs);  // E
        Assert.Contains(7, openPcs);  // G
        Assert.Contains(11, openPcs); // B
    }

    [Fact]
    public void GetScalePositionsAllPedalStates_CMajor_Fret8_APedalAddsA()
    {
        // A pedal at fret 8: B strings (5&10) shift +2 → A (pc 9)
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var pedalA = PedalState.With(new Pedal("A"));
        var withAAtFret8 = FretboardHelper.GetScalePositions(_e9, pedalA, scale, minFret: 8, maxFret: 8).ToList();
        var aPcs = new HashSet<int>(withAAtFret8.Select(p => p.Note.PitchClass));

        Assert.Contains(9, aPcs); // A — contributed by A pedal on B strings
    }

    [Fact]
    public void GetScalePositionsAllPedalStates_CMajor_Fret8_BPedalAddsF()
    {
        // B pedal at fret 8: G# strings (3&6) shift +1 → F (pc 5)
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);

        var pedalB = PedalState.With(new Pedal("B"));
        var withBAtFret8 = FretboardHelper.GetScalePositions(_e9, pedalB, scale, minFret: 8, maxFret: 8).ToList();
        var bPcs = new HashSet<int>(withBAtFret8.Select(p => p.Note.PitchClass));

        Assert.Contains(5, bPcs); // F — contributed by B pedal on G# strings
    }

    // ==========================================================
    // FretPosition value equality
    // ==========================================================

    [Fact]
    public void FretPosition_ValueEquality_SameValues_AreEqual()
    {
        var note = new Note(NoteName.B, Accidental.Natural, 3);
        var a = new FretPosition(5, 0, note, PedalState.Open);
        var b = new FretPosition(5, 0, note, PedalState.Open);

        Assert.Equal(a, b);
    }

    [Fact]
    public void FretPosition_ValueEquality_DifferentFret_NotEqual()
    {
        var note = new Note(NoteName.B, Accidental.Natural, 3);
        var a = new FretPosition(5, 0, note, PedalState.Open);
        var b = new FretPosition(5, 1, note, PedalState.Open);

        Assert.NotEqual(a, b);
    }

    // ==========================================================
    // GetBarPosition — single fret, grouped by string
    // ==========================================================

    [Fact]
    public void GetBarPosition_Fret8_CMajor_HasTenStrings()
    {
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);
        var root = new Note(NoteName.C, Accidental.Natural, 4);

        var bar = FretboardHelper.GetBarPosition(_e9, 8, scale, root);

        Assert.Equal(10, bar.Strings.Count);
    }

    [Fact]
    public void GetBarPosition_Fret8_CMajor_AllSevenDegreesCovered()
    {
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);
        var root = new Note(NoteName.C, Accidental.Natural, 4);

        var bar = FretboardHelper.GetBarPosition(_e9, 8, scale, root);

        // C Ionian has 7 degrees: R, 2, 3, 4, 5, 6, 7
        Assert.Contains("R", bar.CoveredDegrees);
        Assert.Contains("2", bar.CoveredDegrees);
        Assert.Contains("3", bar.CoveredDegrees);
        Assert.Contains("4", bar.CoveredDegrees);
        Assert.Contains("5", bar.CoveredDegrees);
        Assert.Contains("6", bar.CoveredDegrees);
        Assert.Contains("7", bar.CoveredDegrees);
        Assert.Equal(7, bar.CoveredDegrees.Count);
    }

    [Fact]
    public void GetBarPosition_Fret8_CMajor_NonTargetStringsHaveNoOptions()
    {
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);
        var root = new Note(NoteName.C, Accidental.Natural, 4);

        var bar = FretboardHelper.GetBarPosition(_e9, 8, scale, root);
        var nonTargets = bar.Strings.Where(s => !s.HasTarget).ToList();

        Assert.All(nonTargets, s => Assert.Empty(s.TargetOptions));
    }

    [Fact]
    public void GetBarPosition_Fret8_CMajor_String4_SimplestOptionIsRoot()
    {
        // String 4 at fret 8: open → C (root), C pedal → D (2nd), E lever → B (7th)
        // First option (simplest) should be open → Root
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);
        var root = new Note(NoteName.C, Accidental.Natural, 4);

        var bar = FretboardHelper.GetBarPosition(_e9, 8, scale, root);
        var string4 = bar.Strings.Single(s => s.StringNumber == 4);

        Assert.True(string4.HasTarget);
        Assert.Equal("R", string4.TargetOptions[0].DegreeLabel);
        Assert.Equal(PedalState.Open, string4.TargetOptions[0].PedalState);
    }

    [Fact]
    public void GetBarPosition_Fret8_CMajor_String4_HasMultipleOptions()
    {
        // String 4 at fret 8: open → C (R), C pedal → D (2), E lever → B (7)
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);
        var root = new Note(NoteName.C, Accidental.Natural, 4);

        var bar = FretboardHelper.GetBarPosition(_e9, 8, scale, root);
        var string4 = bar.Strings.Single(s => s.StringNumber == 4);
        var labels = string4.TargetOptions.Select(o => o.DegreeLabel).ToHashSet();

        Assert.Contains("R", labels);  // open → C
        Assert.Contains("2", labels);  // C pedal → D
        Assert.Contains("7", labels);  // E lever → B
    }

    [Fact]
    public void GetBarPosition_Fret8_CMajor_String3_BPedalGives4th()
    {
        // String 3 (G#4) at fret 8: open → E (3), B pedal → F (4)
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);
        var root = new Note(NoteName.C, Accidental.Natural, 4);

        var bar = FretboardHelper.GetBarPosition(_e9, 8, scale, root);
        var string3 = bar.Strings.Single(s => s.StringNumber == 3);
        var labels = string3.TargetOptions.Select(o => o.DegreeLabel).ToHashSet();

        Assert.Contains("3", labels);  // open → E
        Assert.Contains("4", labels);  // B pedal → F
    }

    [Fact]
    public void GetBarPosition_Fret8_CMajor_String5_APedalGives6th()
    {
        // String 5 (B3) at fret 8: open → G (5), A pedal → A (6)
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);
        var root = new Note(NoteName.C, Accidental.Natural, 4);

        var bar = FretboardHelper.GetBarPosition(_e9, 8, scale, root);
        var string5 = bar.Strings.Single(s => s.StringNumber == 5);
        var labels = string5.TargetOptions.Select(o => o.DegreeLabel).ToHashSet();

        Assert.Contains("5", labels);  // open → G
        Assert.Contains("6", labels);  // A pedal → A
    }

    [Fact]
    public void GetBarPosition_Fret8_CMajor_TargetStringCountIsCorrect()
    {
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);
        var root = new Note(NoteName.C, Accidental.Natural, 4);

        var bar = FretboardHelper.GetBarPosition(_e9, 8, scale, root);

        Assert.Equal(bar.Strings.Count(s => s.HasTarget), bar.TargetStringCount);
        Assert.True(bar.TargetStringCount >= 7);
    }

    [Fact]
    public void GetBarPosition_Fret8_CMajor_OpenNoteMatchesGetNoteAtFret()
    {
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);
        var root = new Note(NoteName.C, Accidental.Natural, 4);

        var bar = FretboardHelper.GetBarPosition(_e9, 8, scale, root);

        Assert.All(bar.Strings, s =>
        {
            var gs = _e9.Strings.Single(g => g.Number == s.StringNumber);
            var expected = FretboardHelper.GetNoteAtFret(gs, 8, _e9, PedalState.Open);
            Assert.Equal(expected, s.OpenNote);
        });
    }

    // ==========================================================
    // GetBarPositions — all frets in range
    // ==========================================================

    [Fact]
    public void GetBarPositions_CMajor_DefaultRange_Returns25Positions()
    {
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);
        var root = new Note(NoteName.C, Accidental.Natural, 4);

        var bars = FretboardHelper.GetBarPositions(_e9, scale, root);

        Assert.Equal(25, bars.Count); // frets 0-24
    }

    [Fact]
    public void GetBarPositions_CMajor_ConstrainedRange_ReturnsCorrectCount()
    {
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);
        var root = new Note(NoteName.C, Accidental.Natural, 4);

        var bars = FretboardHelper.GetBarPositions(_e9, scale, root, minFret: 5, maxFret: 10);

        Assert.Equal(6, bars.Count); // frets 5-10
        Assert.All(bars, b => Assert.InRange(b.Fret, 5, 10));
    }

    [Fact]
    public void GetBarPositions_ConvenienceOverload_MatchesExplicit()
    {
        ScaleHelper.TryGenerateScale(
            new Note(NoteName.C, Accidental.Natural, 4), Mode.Major, out var scale);
        var root = new Note(NoteName.C, Accidental.Natural, 4);

        var fromExplicit = FretboardHelper.GetBarPositions(_e9, scale, root, 0, 12);
        var fromOverload = FretboardHelper.GetBarPositions(
            _e9, NoteName.C, Accidental.Natural, Mode.Major, 0, 12);

        Assert.Equal(fromExplicit.Count, fromOverload.Count);
        for (int i = 0; i < fromExplicit.Count; i++)
        {
            Assert.Equal(fromExplicit[i].Fret, fromOverload[i].Fret);
            Assert.Equal(fromExplicit[i].TargetStringCount, fromOverload[i].TargetStringCount);
        }
    }

    // ==========================================================
    // NoteOption value equality
    // ==========================================================

    [Fact]
    public void NoteOption_SameValues_AreEqual()
    {
        var note = new Note(NoteName.C, Accidental.Natural, 4);
        var a = new NoteOption(note, PedalState.Open, "R");
        var b = new NoteOption(note, PedalState.Open, "R");

        Assert.Equal(a, b);
    }
}
