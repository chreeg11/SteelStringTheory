using CommunityToolkit.Mvvm.ComponentModel;
using MusicTheory.Models;
using PedalSteel.Data;
using PedalSteel.Functions;
using PedalSteel.Models;

namespace SteelStringTheory.UI.ViewModels;

public partial class FretboardViewModel : ObservableObject
{
    private readonly Copedent copedent = CopedentFactory.CreateE9();

    private static readonly NoteName[] NoteNameMap = Enum.GetValues<NoteName>();
    private static readonly Accidental[] AccidentalMap = [Accidental.Natural, Accidental.Sharp, Accidental.Flat];
    private static readonly Mode[] ModeMap = Enum.GetValues<Mode>();

    public string[] KeyOptions { get; } = Enum.GetNames<NoteName>();
    public string[] AccidentalOptions { get; } = ["Natural", "Sharp", "Flat"];
    public string[] ModeOptions { get; } = Enum.GetNames<Mode>();

    [ObservableProperty]
    public partial int SelectedKeyIndex { get; set; }

    [ObservableProperty]
    public partial int SelectedAccidentalIndex { get; set; }

    [ObservableProperty]
    public partial int SelectedModeIndex { get; set; }

    [ObservableProperty]
    public partial IReadOnlyList<BarPosition> BarPositions { get; set; } = [];

    [ObservableProperty]
    public partial string ScaleLabel { get; set; } = "";

    public Copedent Copedent => copedent;

    public FretboardViewModel()
    {
        UpdatePositions();
    }

    partial void OnSelectedKeyIndexChanged(int value) => UpdatePositions();
    partial void OnSelectedAccidentalIndexChanged(int value) => UpdatePositions();
    partial void OnSelectedModeIndexChanged(int value) => UpdatePositions();

    private void UpdatePositions()
    {
        var key = NoteNameMap[SelectedKeyIndex];
        var accidental = AccidentalMap[SelectedAccidentalIndex];
        var mode = ModeMap[SelectedModeIndex];

        BarPositions = FretboardHelper.GetBarPositions(copedent, key, accidental, mode, 0, 12);

        var accStr = accidental switch
        {
            Accidental.Sharp => "♯",
            Accidental.Flat => "♭",
            _ => ""
        };
        ScaleLabel = $"{key}{accStr} {mode}";
    }
}
