using MusicTheory.Models;
using MusicTheory.Functions;

// Enharmonic equivalents — same sound, different spelling
var db4 = new Note(NoteName.D, Accidental.Flat, 4);
var cs4 = new Note(NoteName.C, Accidental.Sharp, 4);
Console.WriteLine($"Db4: PitchClass={db4.PitchClass}, Midi={db4.MidiNumber}");
Console.WriteLine($"C#4: PitchClass={cs4.PitchClass}, Midi={cs4.MidiNumber}");
Console.WriteLine($"Same pitch? {db4.PitchClass == cs4.PitchClass}");

// Transpose C4 up a major 3rd (4 semitones)
var c4 = new Note(NoteName.C, Accidental.Natural, 4);
var up4 = NoteHelper.Transpose(c4, 4);
Console.WriteLine($"\nC4 + 4 semitones = {up4.SpelledName}{up4.Accidental} {up4.Octave}");

// Interval between C and G
var g4 = new Note(NoteName.G, Accidental.Natural, 4);
var interval = IntervalHelper.GetInterval(c4, g4);
Console.WriteLine($"\nC4 to G4 = {interval.Semitones} semitones ({IntervalHelper.GetIntervalName(interval.Semitones)})");
