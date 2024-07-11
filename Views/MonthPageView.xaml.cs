using Taskinator.ViewModels;

namespace Taskinator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MonthPageView : ContentPage
    {
        public MonthPageView(MonthPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        void UnloadedHandler(object sender, EventArgs e)
        {
            try
            {
              //  calendar.Dispose();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            (BindingContext as IDisposable)?.Dispose();
        }

    }
}
