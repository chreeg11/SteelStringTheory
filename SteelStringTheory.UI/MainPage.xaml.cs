using SteelStringTheory.UI.Controls;
using SteelStringTheory.UI.ViewModels;

namespace SteelStringTheory.UI;

public partial class MainPage : ContentPage
{
    private readonly FretboardDrawable drawable = new();

    public MainPage(FretboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        FretboardView.Drawable = drawable;

        drawable.BarPositions = viewModel.BarPositions;

        viewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(FretboardViewModel.BarPositions))
            {
                drawable.BarPositions = viewModel.BarPositions;
                FretboardView.Invalidate();
            }
        };
    }
}
