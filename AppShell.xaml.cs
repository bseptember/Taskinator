using Taskinator.Views;

namespace Taskinator
{
    public partial class AppShell : Shell
    {
        private bool _isNavigating;

        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            // Additional routes for navigation, i.e. routes not in the flyout menu
            Routing.RegisterRoute(nameof(SearchPageView), typeof(SearchPageView));
            Routing.RegisterRoute(nameof(AddCustomEventPageView), typeof(AddCustomEventPageView));
            Routing.RegisterRoute(nameof(EditEventPageView), typeof(EditEventPageView));

            Routing.RegisterRoute(nameof(AccountPageView), typeof(AccountPageView));
            Routing.RegisterRoute(nameof(DayPageView), typeof(DayPageView));
            Routing.RegisterRoute(nameof(MonthPageView), typeof(MonthPageView));
            Routing.RegisterRoute(nameof(SettingsPageView), typeof(SettingsPageView));
            Routing.RegisterRoute(nameof(YearPageView), typeof(YearPageView));
        }

        // Override OnNavigating method to handle navigation events if needed
        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            // Check if navigation is already in progress
            if (_isNavigating)
            {
                args.Cancel(); // Cancel new navigation request if already navigating
                return;
            }

            _isNavigating = true;

            // Perform custom actions before navigation
            await BeforeNavigation();

            // Allow navigation to proceed
            _isNavigating = false;
        }

        // Override OnNavigated method to handle post-navigation events if needed
        protected override async void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);

            // Perform custom actions after navigation
            await AfterNavigation();
        }

        // Custom method to perform actions before navigation
        private async Task BeforeNavigation()
        {
            // Add any pre-navigation logic here
            await Task.CompletedTask;
        }

        // Custom method to perform actions after navigation
        private async Task AfterNavigation()
        {
            // Add any post-navigation logic here
            await Task.CompletedTask;
        }
    }
}
