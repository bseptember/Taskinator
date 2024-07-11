using Taskinator.ViewModels;

namespace Taskinator.Views;

public partial class YearPageView : ContentPage
{
	public YearPageView()
	{
		InitializeComponent();
        BindingContext = new YearPageViewModel();
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as IDisposable)?.Dispose();
    }

}