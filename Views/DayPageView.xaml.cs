using Taskinator.ViewModels;

namespace Taskinator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DayPageView : ContentPage
    {

        public DayPageView(DayPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            // Subscribe to the SizeChanged event to scroll to the current time when layout changes
            SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            // Scroll to the current time position
            ScrollToCurrentTime();
        }

        private void ScrollToCurrentTime()
        {
            // Calculate the position to scroll to based on the current time
            DateTime currentTime = DateTime.Now;
            double scrollToY = CalculateScrollPosition(currentTime);

            // Scroll the ScrollView to the calculated position
            MainScrollView.ScrollToAsync(0, scrollToY, true);
        }

        private double CalculateScrollPosition(DateTime currentTime)
        {
            // Calculate the Y-offset where the current time should be visible
            // This calculation will depend on your specific layout and item heights

            // For example, if you have hourly sections, you can calculate something like:
            double sectionHeight = 59.19/* Height of each section (e.g., each hour) */;
            int currentHour = currentTime.Hour;
            double scrollY = currentHour * sectionHeight; // Adjust as per your actual layout

            return scrollY;
        }

        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            var viewModel = BindingContext as DayPageViewModel;

            if (viewModel == null)
                return;

            System.Diagnostics.Debug.WriteLine($"Swiped {e.Direction}");

            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    viewModel?.SwipeLeftCommand.Execute(null);
                    break;
                case SwipeDirection.Right:
                    viewModel?.SwipeRightCommand.Execute(null);
                    break;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            (BindingContext as IDisposable)?.Dispose();
        }
    }
}