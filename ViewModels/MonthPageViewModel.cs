using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Calendar.Enums;
using Plugin.Maui.Calendar.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Taskinator.Models;
using Taskinator.Services;
using Taskinator.Views;

namespace Taskinator.ViewModels
{
    public partial class MonthPageViewModel : BaseViewModel
    {
        private readonly IEventService _eventService;
        private readonly IDialogService _dialogService;
        private readonly IDatabaseService _databaseService;
        private readonly IEventParser _eventParser;

        public MonthPageViewModel(IEventService eventService, IDialogService dialogService, IDatabaseService databaseService,IEventParser eventParser)
        {
            _eventService = eventService;
            _dialogService = dialogService;
            _databaseService = databaseService;
            _eventParser = eventParser;

            Events = _eventService.Events;

            _eventService.EventAdded += OnEventAdded;
            _eventService.EventRemoved += OnEventRemoved;

            _eventService.EventsAdded += OnEventsAdded;
            _eventService.EventsRemoved += OnEventsRemoved;
        }

        public ObservableCollection<EventModel> EventList { get; set; } = new ObservableCollection<EventModel>();

        [ObservableProperty]
        public EventCollection events;

        [ObservableProperty]
        private DateTime currentDate = DateTime.Today;

        [ObservableProperty]
        private DateTime? selectedDate = DateTime.Today;

        [ObservableProperty]
        private int day = DateTime.Today.Day;

        [ObservableProperty]
        private int month = DateTime.Today.Month;

        [ObservableProperty]
        private int year = DateTime.Today.Year;

        [ObservableProperty]
        private DateTime shownDate = DateTime.Today;

        [ObservableProperty]
        private WeekLayout calendarLayout = WeekLayout.Month;

        [ObservableProperty]
        private string calendarTypeIcon = "month.png";

        [ObservableProperty]
        private string calendarTypeText = "Month";

        [ObservableProperty]
        private string enteredText = string.Empty;

        public ICommand TodayButtonCommand => new Command(OnTodayButtonClicked);
        public ICommand ChangeCalendarTypeCommand => new Command(() => ChangeCalendarType());
        public ICommand DayTappedCommand => new Command<DateTime>(async (date) => await DayTapped(date));
        public ICommand SwipeLeftCommand => new Command(() => ChangeShownUnit(1));
        public ICommand SwipeRightCommand => new Command(() => ChangeShownUnit(-1));
         public ICommand SearchCommand => new AsyncRelayCommand(Search);
        public ICommand AddCommand => new AsyncRelayCommand(Add);
        public ICommand EnterCommand => new AsyncRelayCommand(EnterText);

        private void OnTodayButtonClicked()
        {
            Year = DateTime.Today.Year;
            Month = DateTime.Today.Month;
            Day = DateTime.Today.Day;

        }

        private async Task DayTapped(DateTime value)
        {
            GetEventForSelectedDate(value);

            string message = string.Empty;

            // Iterate over EventList to get event names
            foreach (EventModel item in EventList)
            {
                message += $"By: {item.User}\nId: {item.Id}\nName: {item.Name}\nStart (UTC):{item.Starting.DateTime:t}\nMinutes: {item.DurationInMinutes}\n\n";
            }

            await _dialogService.DisplayAlert($"Found {EventList.Count} item/s for {value:d MMM yyyy}.", message, "Ok");
        }

        private void GetEventForSelectedDate(DateTime value)
        {
            EventList.Clear();

            // Check if _events contains events for CurrentDate
            if (_eventService.Events.ContainsKey(value.Date))
            {
                // Get the list of events for CurrentDate
                var eventsForDate = _eventService.Events[value.Date];

                // Add each event to EventList
                foreach (EventModel eventModel in eventsForDate)
                {
                    EventList.Add(eventModel);
                }
            }
        }

        private void ChangeCalendarType()
        {
            if (CalendarLayout == WeekLayout.Month)
            {
                CalendarLayout = WeekLayout.Week;
                CalendarTypeIcon = "month.png";
                CalendarTypeText = "Month";
            }
            else if (CalendarLayout == WeekLayout.Week)
            {
                CalendarLayout = WeekLayout.Month;
                CalendarTypeIcon = "week.png";
                CalendarTypeText = "Week";
            }
            else
            {
                // do nothing
            }
        }

        private void ChangeShownUnit(int amountToAdd)
        {
            switch (CalendarLayout)
            {
                case WeekLayout.Week:
                case WeekLayout.TwoWeek:
                    ChangeShownWeek(amountToAdd);
                    break;

                case WeekLayout.Month:
                default:
                    ChangeShownMonth(amountToAdd);
                    break;
            }
        }

        private void ChangeShownMonth(int monthsToAdd)
        {
            ShownDate.AddMonths(monthsToAdd);
        }

        private void ChangeShownWeek(int weeksToAdd)
        {
            ShownDate.AddDays(weeksToAdd * 7);
        }

        private async Task Add()
        {
            await Shell.Current.GoToAsync($"{nameof(AddCustomEventPageView)}?events={_eventService.Events}");
        }

        private void OnEventAdded(object sender, EventModel newEvent)
        {
            EventList.Add(newEvent);
        }

        private void OnEventRemoved(object sender, EventModel newEvent)
        {
            EventList.Remove(newEvent);
        }

        private void OnEventsAdded(object sender, List<EventModel> newEvents)
        {
        }

        private void OnEventsRemoved(object sender, List<EventModel> newEvents)
        {
            EventList.Clear();
        }

        private async Task Search()
        {
            await Shell.Current.GoToAsync(nameof(SearchPageView));
        }

        private async Task EnterText()
        {
            if (!string.IsNullOrWhiteSpace(EnteredText))
            {
                var newEvent = _eventParser.ParseEventFromText(EnteredText);

                // Add locally
                _eventService.AddEvent(newEvent);

                // Add to database storage
                await _databaseService.AddEvent(newEvent);

                await _dialogService.DisplayAlert("Entry saved", $"A new event was added.\n{newEvent.Name}\n{newEvent.Description}", "Ok");

                EnteredText = string.Empty; // Clear the text box
            }
        }
    }
}
