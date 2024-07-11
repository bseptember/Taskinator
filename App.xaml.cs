using Taskinator.Services;

namespace Taskinator
{
    public partial class App : Application
    {
        private IDialogService _dialogService;
        public App()
        {
            InitializeComponent();

            _dialogService = new DialogService();

            // Force dark theme
            UserAppTheme = AppTheme.Dark;

            MainPage = new AppShell();
        }

#if (IOS || ANDROID)
        protected override async void OnStart()
        {
            await _dialogService.DisplayAlert("Let's get started!", "Welcome to the Taskinator.", "Ok");
        }

        protected override async void OnResume()
        {
            await _dialogService.DisplayAlert("Welcome back!", "App is resuming.", "Ok");
        }
#endif

    }
}