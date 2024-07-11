using Taskinator.ViewModels;

namespace Taskinator.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SearchPageView : ContentPage
{
	public SearchPageView(SearchPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as IDisposable)?.Dispose();
    }

}