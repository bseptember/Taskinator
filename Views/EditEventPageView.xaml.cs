using Taskinator.ViewModels;
namespace Taskinator.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EditEventPageView : ContentPage
{
	public EditEventPageView(EditEventPageViewModel viewModel)
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