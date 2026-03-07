namespace PedalSteel.Models;

/// <summary>
/// Represents the set of pedals currently engaged on the instrument.
/// Use <see cref="Open"/> for no pedals engaged.
/// </summary>
public readonly record struct PedalState(IReadOnlySet<Pedal> ActivePedals)
{
    /// <summary>No pedals engaged — open string tuning.</summary>
    public static readonly PedalState Open = new(new HashSet<Pedal>());

    /// <summary>Creates a PedalState with one or more pedals engaged.</summary>
    public static PedalState With(params Pedal[] pedals) => new(new HashSet<Pedal>(pedals));

    public bool IsEngaged(Pedal pedal) => ActivePedals.Contains(pedal);
}
