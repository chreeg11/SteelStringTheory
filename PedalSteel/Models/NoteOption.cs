using MusicTheory.Models;

namespace PedalSteel.Models;

/// <summary>
/// A single playable option: a note produced by a specific pedal state, with its degree label.
/// Only created for target tones (scale/chord notes).
/// </summary>
public readonly record struct NoteOption(
    Note Note,
    PedalState PedalState,
    string DegreeLabel);
