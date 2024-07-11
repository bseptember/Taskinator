using Taskinator.ViewModels;

namespace Taskinator.Views
{
    public partial class AccountPageView : ContentPage
    {
        public AccountPageView(AccountPageViewModel viewModel)
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
