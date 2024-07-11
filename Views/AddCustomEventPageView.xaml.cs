using Taskinator.ViewModels;

namespace Taskinator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCustomEventPageView : ContentPage
    {
        public AddCustomEventPageView(AddCustomEventPageViewModel viewModel)
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
}
