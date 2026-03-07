using MusicTheory.Models;

namespace PedalSteel.Models;

/// <summary>
/// A note at a specific position on the fretboard, including which pedals are engaged.
/// </summary>
public readonly record struct FretPosition(uint StringNumber, int Fret, Note Note, PedalState PedalState);