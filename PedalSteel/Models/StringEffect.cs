namespace PedalSteel.Models
{
    // Note, alternative name could've been StringChange but
    // that feels more like a verb, hence going with effect.
    // i.e. "A pedal has these StringEffects: raise string 4 by 2,
    // raise string 8 by 2."
    public readonly record struct StringEffect(uint StringNumber, int SemitonesDelta);
}