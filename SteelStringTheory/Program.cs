using MusicTheory.Models;
using MusicTheory.Functions;
using PedalSteel.Models;
using PedalSteel.Data;
using PedalSteel.Functions;

var e9 = CopedentFactory.CreateE9();

// ── Show the E9 open tuning ──────────────────────────────────
Console.WriteLine("═══ E9 Standard Tuning ═══");
Console.WriteLine();
foreach (var s in e9.Strings)
{
    Console.WriteLine($"  String {s.Number,2}: {s.OpenNote}");
}

// ── C Major scale at fret 8 ─────────────────────────────────
Console.WriteLine();
Console.WriteLine("═══ C Major (Ionian) at Fret 8 ═══");
Console.WriteLine();

var bar8 = FretboardHelper.GetBarPositions(
    e9, NoteName.C, Accidental.Natural, Mode.Ionian, minFret: 8, maxFret: 8)[0];

PrintBarPosition(bar8);

// ── C Major scale at fret 3 (A+B pedals territory) ──────────
Console.WriteLine();
Console.WriteLine("═══ C Major (Ionian) at Fret 3 ═══");
Console.WriteLine();

var bar3 = FretboardHelper.GetBarPositions(
    e9, NoteName.C, Accidental.Natural, Mode.Ionian, minFret: 3, maxFret: 3)[0];

PrintBarPosition(bar3);

// ── Best frets for C Major (ranked by coverage) ─────────────
Console.WriteLine();
Console.WriteLine("═══ Best Frets for C Major (all 7 degrees) ═══");
Console.WriteLine();

var allBars = FretboardHelper.GetBarPositions(
    e9, NoteName.C, Accidental.Natural, Mode.Ionian);

foreach (var bar in allBars.Where(b => b.CoveredDegrees.Count == 7).OrderBy(b => b.Fret))
{
    Console.WriteLine($"  Fret {bar.Fret,2}: {bar.TargetStringCount} strings hit, all 7 degrees covered");
}

// ── What note is string 5 at fret 3 with A pedal? ───────────
Console.WriteLine();
Console.WriteLine("═══ Quick Note Lookup ═══");
Console.WriteLine();

var string5 = e9.Strings.Single(s => s.Number == 5);
var open = FretboardHelper.GetNoteAtFret(string5, 3, e9, PedalState.Open);
var withA = FretboardHelper.GetNoteAtFret(string5, 3, e9, PedalState.With(new Pedal("A")));
Console.WriteLine($"  String 5, Fret 3, Open:    {open}");
Console.WriteLine($"  String 5, Fret 3, A pedal: {withA}");

// ══════════════════════════════════════════════════════════════

void PrintBarPosition(BarPosition bar)
{
    Console.WriteLine($"  Fret {bar.Fret} — {bar.CoveredDegrees.Count} of 7 degrees covered");
    Console.WriteLine($"  {"Str",3}  {"Open",6}  {"Targets",-40}");
    Console.WriteLine($"  {"───",3}  {"──────",6}  {"────────────────────────────────────────",-40}");

    foreach (var s in bar.Strings)
    {
        var targets = s.HasTarget
            ? string.Join(" | ", s.TargetOptions.Select(FormatOption))
            : "—";

        Console.WriteLine($"  {s.StringNumber,3}  {s.OpenNote,6}  {targets}");
    }
}

string FormatOption(NoteOption opt)
{
    var pedals = opt.PedalState.ActivePedals.Count == 0
        ? "open"
        : string.Join("+", opt.PedalState.ActivePedals.Select(p => p.Name).OrderBy(n => n));

    return $"{opt.DegreeLabel}={opt.Note} ({pedals})";
}
