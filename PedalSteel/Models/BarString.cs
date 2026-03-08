using MusicTheory.Models;

namespace PedalSteel.Models;

/// <summary>
/// All playable options for one physical string at a specific bar position.
/// Always shows the open (no pedals) note, plus any pedal states that produce target tones.
/// Sorted by pedal count ascending (simplest first).
/// </summary>
public class BarString
{
    public uint StringNumber { get; }

    /// <summary>
    /// The note this string produces at this fret with no pedals engaged.
    /// </summary>
    public Note OpenNote { get; }

    /// <summary>
    /// All pedal states that produce a target tone on this string, sorted by pedal count.
    /// Empty if no pedal state produces a target tone. May include PedalState.Open.
    /// </summary>
    public IReadOnlyList<NoteOption> TargetOptions { get; }

    /// <summary>
    /// True if at least one pedal state produces a target tone on this string.
    /// </summary>
    public bool HasTarget => TargetOptions.Count > 0;

    public BarString(uint stringNumber, Note openNote, IReadOnlyList<NoteOption> targetOptions)
    {
        StringNumber = stringNumber;
        OpenNote = openNote;
        TargetOptions = targetOptions;
    }
}
