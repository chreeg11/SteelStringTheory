using MusicTheory.Models;
using MusicTheory.Functions;

var db4 = new Note(PitchClass.CSharp, 4, NoteName.D, Accidental.Flat);
var cs4 = new Note(PitchClass.CSharp, 4, NoteName.C, Accidental.Sharp);
Console.WriteLine(db4);
Console.WriteLine(cs4);