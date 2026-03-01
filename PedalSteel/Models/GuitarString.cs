using MusicTheory.Models;

namespace PedalSteel.Models
{
    public readonly record struct GuitarString(
        uint Number,
        Note OpenNote);
}