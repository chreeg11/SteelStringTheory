using PedalSteel.Models;

namespace SteelStringTheory.UI.Controls;

/// <summary>
/// Custom drawable that renders a pedal steel fretboard with scale tone
/// dots, degree labels, and pedal indicators.
/// </summary>
public class FretboardDrawable : IDrawable
{
    private const int StringCount = 10;
    private const float LeftMargin = 70f;
    private const float TopMargin = 40f;
    private const float RightMargin = 20f;
    private const float BottomMargin = 40f;

    private static readonly int[] SingleDotFrets = [3, 5, 7, 9, 15, 17, 19, 21];
    private static readonly int[] DoubleDotFrets = [12, 24];

    public IReadOnlyList<BarPosition>? BarPositions { get; set; }
    public int MinFret { get; set; }
    public int MaxFret { get; set; } = 12;

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (dirtyRect.Width < 200 || dirtyRect.Height < 100)
        {
            return;
        }

        var numColumns = MaxFret - MinFret + 1;
        var fretboardWidth = dirtyRect.Width - LeftMargin - RightMargin;
        var fretboardHeight = dirtyRect.Height - TopMargin - BottomMargin;
        var colWidth = fretboardWidth / numColumns;
        var stringSpacing = fretboardHeight / (StringCount - 1);
        var dotRadius = Math.Min(Math.Min(colWidth * 0.35f, stringSpacing * 0.35f), 18f);

        DrawFretboardBackground(canvas, fretboardWidth, fretboardHeight);
        DrawFretMarkers(canvas, colWidth, stringSpacing);
        DrawFretWires(canvas, numColumns, colWidth, fretboardHeight);
        DrawStrings(canvas, fretboardWidth, stringSpacing);
        DrawFretLabels(canvas, numColumns, colWidth);
        DrawStringLabels(canvas, stringSpacing);
        DrawScaleTones(canvas, colWidth, stringSpacing, dotRadius);
    }

    private static void DrawFretboardBackground(ICanvas canvas, float fretboardWidth, float fretboardHeight)
    {
        canvas.FillColor = Color.FromArgb("#F5E6D0");
        canvas.FillRectangle(LeftMargin, TopMargin, fretboardWidth, fretboardHeight);
    }

    private void DrawFretMarkers(ICanvas canvas, float colWidth, float stringSpacing)
    {
        canvas.FillColor = Color.FromArgb("#E0D5C5");
        var midY = TopMargin + (StringCount - 1) * stringSpacing / 2f;

        foreach (var fret in SingleDotFrets)
        {
            if (fret < MinFret || fret > MaxFret)
            {
                continue;
            }

            var x = LeftMargin + (fret - MinFret + 0.5f) * colWidth;
            canvas.FillCircle(x, midY, 5f);
        }

        foreach (var fret in DoubleDotFrets)
        {
            if (fret < MinFret || fret > MaxFret)
            {
                continue;
            }

            var x = LeftMargin + (fret - MinFret + 0.5f) * colWidth;
            var offset = stringSpacing * 1.5f;
            canvas.FillCircle(x, midY - offset, 5f);
            canvas.FillCircle(x, midY + offset, 5f);
        }
    }

    private void DrawFretWires(ICanvas canvas, int numColumns, float colWidth, float fretboardHeight)
    {
        for (var i = 0; i <= numColumns; i++)
        {
            var x = LeftMargin + i * colWidth;
            var fretNum = MinFret + i;
            var isNut = fretNum == 0;
            var isBoundary = i == 0 || i == numColumns;

            canvas.StrokeColor = isNut ? Color.FromArgb("#222222") : Color.FromArgb("#999999");
            canvas.StrokeSize = isNut ? 4f : (isBoundary ? 2f : 1.5f);
            canvas.DrawLine(x, TopMargin, x, TopMargin + fretboardHeight);
        }
    }

    private static void DrawStrings(ICanvas canvas, float fretboardWidth, float stringSpacing)
    {
        for (var i = 0; i < StringCount; i++)
        {
            var y = TopMargin + i * stringSpacing;
            canvas.StrokeColor = Color.FromArgb("#666666");
            canvas.StrokeSize = 1f + i * 0.15f;
            canvas.DrawLine(LeftMargin, y, LeftMargin + fretboardWidth, y);
        }
    }

    private void DrawFretLabels(ICanvas canvas, int numColumns, float colWidth)
    {
        canvas.FontColor = Color.FromArgb("#444444");
        canvas.FontSize = 12f;

        for (var i = 0; i < numColumns; i++)
        {
            var fretNum = MinFret + i;
            var x = LeftMargin + (i + 0.5f) * colWidth;
            var label = fretNum == 0 ? "Open" : fretNum.ToString();
            canvas.DrawString(label, x - 20, 4, 40, TopMargin - 8,
                HorizontalAlignment.Center, VerticalAlignment.Bottom);
        }
    }

    private void DrawStringLabels(ICanvas canvas, float stringSpacing)
    {
        if (BarPositions is null || BarPositions.Count == 0)
        {
            return;
        }

        canvas.FontColor = Color.FromArgb("#444444");
        canvas.FontSize = 10f;

        var bp = BarPositions[0];
        for (var i = 0; i < bp.Strings.Count && i < StringCount; i++)
        {
            var bs = bp.Strings[i];
            var y = TopMargin + i * stringSpacing;
            var noteLabel = FormatNoteName(bs.OpenNote);
            canvas.DrawString($"{bs.StringNumber} {noteLabel}", 2, y - 8, LeftMargin - 8, 16,
                HorizontalAlignment.Right, VerticalAlignment.Center);
        }
    }

    private void DrawScaleTones(ICanvas canvas, float colWidth, float stringSpacing, float dotRadius)
    {
        if (BarPositions is null)
        {
            return;
        }

        foreach (var bp in BarPositions)
        {
            var col = bp.Fret - MinFret;
            var cx = LeftMargin + (col + 0.5f) * colWidth;

            for (var si = 0; si < bp.Strings.Count && si < StringCount; si++)
            {
                var bs = bp.Strings[si];
                if (!bs.HasTarget)
                {
                    continue;
                }

                var option = bs.TargetOptions[0];
                var isRoot = option.DegreeLabel == "R";
                var hasPedals = option.PedalState.ActivePedals.Count > 0;
                var cy = TopMargin + si * stringSpacing;

                var fill = (isRoot, hasPedals) switch
                {
                    (true, false) => Color.FromArgb("#D32F2F"),
                    (true, true) => Color.FromArgb("#EF9A9A"),
                    (false, false) => Color.FromArgb("#1565C0"),
                    (false, true) => Color.FromArgb("#90CAF9"),
                };

                canvas.FillColor = fill;
                canvas.FillCircle(cx, cy, dotRadius);

                // Degree label inside the dot
                canvas.FontColor = hasPedals ? Color.FromArgb("#333333") : Colors.White;
                canvas.FontSize = dotRadius * 0.75f;
                canvas.DrawString(option.DegreeLabel,
                    cx - dotRadius, cy - dotRadius,
                    dotRadius * 2, dotRadius * 2,
                    HorizontalAlignment.Center, VerticalAlignment.Center);

                // Pedal indicator below the dot
                if (hasPedals)
                {
                    var pedalText = string.Join("",
                        option.PedalState.ActivePedals
                            .OrderBy(p => p.Name)
                            .Select(p => p.Name));
                    canvas.FontColor = Color.FromArgb("#777777");
                    canvas.FontSize = dotRadius * 0.55f;
                    canvas.DrawString(pedalText,
                        cx - dotRadius, cy + dotRadius + 1,
                        dotRadius * 2, dotRadius * 0.8f,
                        HorizontalAlignment.Center, VerticalAlignment.Top);
                }
            }
        }
    }

    private static string FormatNoteName(MusicTheory.Models.Note note)
    {
        var acc = note.Accidental switch
        {
            MusicTheory.Models.Accidental.Sharp => "♯",
            MusicTheory.Models.Accidental.Flat => "♭",
            _ => ""
        };
        return $"{note.NoteName}{acc}";
    }
}
