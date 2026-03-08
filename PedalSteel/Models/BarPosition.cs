namespace PedalSteel.Models;

/// <summary>
/// A fret-grouped view of what's playable at a single bar position.
/// Contains one BarString per physical string, each showing the open note
/// and all pedal states that produce target tones (scale/chord notes).
/// </summary>
public class BarPosition
{
    public int Fret { get; }

    /// <summary>
    /// One entry per physical string, always in string-number order.
    /// </summary>
    public IReadOnlyList<BarString> Strings { get; }

    /// <summary>
    /// How many strings have at least one target tone option.
    /// </summary>
    public int TargetStringCount { get; }

    /// <summary>
    /// The set of unique degree labels achievable at this fret across all strings and pedal states.
    /// </summary>
    public IReadOnlySet<string> CoveredDegrees { get; }

    public BarPosition(int fret, IReadOnlyList<BarString> strings)
    {
        Fret = fret;
        Strings = strings;
        TargetStringCount = strings.Count(s => s.HasTarget);
        CoveredDegrees = new HashSet<string>(
            strings.SelectMany(s => s.TargetOptions.Select(o => o.DegreeLabel)));
    }
}
