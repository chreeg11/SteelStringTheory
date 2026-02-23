using MusicTheory.Models;
using MusicTheory.Functions;

namespace MusicTheory.Tests;

public class ScaleHelperTests
{
    [Fact]
    public void Generate_C_Major_Scale()
    {
        var c4 = new Note(NoteName.C, Accidental.Natural, 4);
        var result = ScaleHelper.TryGenerateScale(c4, ScaleType.Major, out var CMajorScale);
        Assert.True(result);
        Assert.Equal(8, CMajorScale.Length);

        Note[] expected = [
            new(NoteName.C, Accidental.Natural, 4),
            new(NoteName.D, Accidental.Natural, 4),
            new(NoteName.E, Accidental.Natural, 4),
            new(NoteName.F, Accidental.Natural, 4),
            new(NoteName.G, Accidental.Natural, 4),
            new(NoteName.A, Accidental.Natural, 4),
            new(NoteName.B, Accidental.Natural, 4),
            new(NoteName.C, Accidental.Natural, 5),
        ];
        Assert.Equal(expected, CMajorScale);
    }

    [Theory]
   [InlineData(NoteName.C, Accidental.Natural)]
   [InlineData(NoteName.D, Accidental.Natural)]
   [InlineData(NoteName.E, Accidental.Natural)]
   [InlineData(NoteName.F, Accidental.Natural)]
   [InlineData(NoteName.G, Accidental.Natural)]
   [InlineData(NoteName.A, Accidental.Natural)]
   [InlineData(NoteName.B, Accidental.Natural)]
   [InlineData(NoteName.C, Accidental.Sharp)]
   [InlineData(NoteName.D, Accidental.Sharp)]
   [InlineData(NoteName.F, Accidental.Sharp)]
   [InlineData(NoteName.G, Accidental.Sharp)]
   [InlineData(NoteName.A, Accidental.Sharp)]
   public void MajorScale_HasCorrectIntervalPattern(NoteName name, Accidental accidental)
   {
       var root = new Note(name, accidental, 4);
       var result = ScaleHelper.TryGenerateScale(root, ScaleType.Major, out var scale);

       Assert.True(result);

       // Verify interval pattern between consecutive notes
       int[] expectedIntervals = [2, 2, 1, 2, 2, 2, 1];
       for (int i = 0; i < expectedIntervals.Length; i++)
       {
           int actual = scale[i + 1].MidiNumber - scale[i].MidiNumber;
           Assert.Equal(expectedIntervals[i], actual);
       }

       // Verify root is first
       Assert.Equal(root.PitchClass, scale[0].PitchClass);

       // Verify 7 unique pitch classes (no duplicates in the scale degrees)
       var uniquePitchClasses = scale.Take(7).Select(n => n.PitchClass).Distinct().Count();       
       Assert.Equal(7, uniquePitchClasses);
   }

   [Theory]
   [InlineData(NoteName.C, Accidental.Natural)]
   [InlineData(NoteName.D, Accidental.Natural)]
   [InlineData(NoteName.E, Accidental.Natural)]
   [InlineData(NoteName.F, Accidental.Natural)]
   [InlineData(NoteName.G, Accidental.Natural)]
   [InlineData(NoteName.A, Accidental.Natural)]
   [InlineData(NoteName.B, Accidental.Natural)]
   [InlineData(NoteName.C, Accidental.Sharp)]
   [InlineData(NoteName.D, Accidental.Sharp)]
   [InlineData(NoteName.F, Accidental.Sharp)]
   [InlineData(NoteName.G, Accidental.Sharp)]
   [InlineData(NoteName.A, Accidental.Sharp)]
   public void IonianScale_HasCorrectIntervalPattern(NoteName name, Accidental accidental)
   {
       var root = new Note(name, accidental, 4);
       var result = ScaleHelper.TryGenerateScale(root, ScaleType.Ionian, out var scale);

       Assert.True(result);

       // Verify interval pattern between consecutive notes
       int[] expectedIntervals = [2, 2, 1, 2, 2, 2, 1];
       for (int i = 0; i < expectedIntervals.Length; i++)
       {
           int actual = scale[i + 1].MidiNumber - scale[i].MidiNumber;
           Assert.Equal(expectedIntervals[i], actual);
       }

       // Verify root is first
       Assert.Equal(root.PitchClass, scale[0].PitchClass);

       // Verify 7 unique pitch classes (no duplicates in the scale degrees)
       var uniquePitchClasses = scale.Take(7).Select(n => n.PitchClass).Distinct().Count();       
       Assert.Equal(7, uniquePitchClasses);
   }
}
