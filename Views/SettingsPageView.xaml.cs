using SkiaSharp.Extended.UI.Controls;
using Taskinator.ViewModels;

namespace Taskinator.Views;

public partial class SettingsPageView : ContentPage
{
	public SettingsPageView(SettingsPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (LottieView != null)
        {
            LottieView.WidthRequest = width;
            LottieView.HeightRequest = height;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as IDisposable)?.Dispose();
    }

}