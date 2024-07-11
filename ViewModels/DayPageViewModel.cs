using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Taskinator.Services;
using Taskinator.Models;
using CommunityToolkit.Mvvm.Input;
using Taskinator.Views;
using Realms;
using IdentityModel.OidcClient;

namespace Taskinator.ViewModels
{
    public partial class DayPageViewModel : BaseViewModel
    {
        private readonly IEventService _eventService;
        private readonly IDialogService _dialogService;
        private readonly IDatabaseService _databaseService;
        private readonly IEventParser _eventParser;

        public DayPageViewModel(IEventService eventService, IDialogService dialogService, IDatabaseService databaseService, IEventParser eventParser)
        {
            _eventService = eventService;
            _dialogService = dialogService;
            _databaseService = databaseService;
            _eventParser = eventParser;

            _eventService.EventAdded += OnEventAdded;
            _eventService.EventRemoved += OnEventRemoved;
            _eventService.EventUpdated += OnEventUpdated;

            _eventService.EventsAdded += OnEventsAdded;
            _eventService.EventsRemoved += OnEventsRemoved;

            _databaseService.loggedInEvent += OnLoggedInEvent;

            Lines = new List<HourItem>();
            for (int hour = 0; hour < 24; hour++)
            {
                Lines.Add(new HourItem($"{hour:00}:00"));
            }

            InitializeEventListAsync();
        }

        public ObservableCollection<EventModel> EventList { get; set; } = new ObservableCollection<EventModel>();

        [ObservableProperty]
        public List<HourItem> lines;

        [ObservableProperty]
        private DateTime currentDate = DateTime.Today;

        [ObservableProperty]
        private string pageDay = $"{DateTime.Today.Day} {DateTime.Today:dddd}, {DateTime.Today.ToString("MMM").ToUpper()}";

        [ObservableProperty]
        private string enteredText = string.Empty;

        public ICommand TodayButtonCommand => new Command(OnTodayButtonClicked);
        public ICommand EventTappedCommand => new AsyncRelayCommand<EventModel>(OnEventTapped);
        public ICommand SwipeLeftCommand => new Command(() => CurrentDate = CurrentDate.AddDays(1));
        public ICommand SwipeRightCommand => new Command(() => CurrentDate = CurrentDate.AddDays(-1));
        public ICommand SearchCommand => new AsyncRelayCommand(Search);
        public ICommand AddCommand => new AsyncRelayCommand(Add);
        public ICommand EnterCommand => new AsyncRelayCommand(EnterText);

        private async void InitializeEventListAsync()
        {
            try
            {
                EventList.Clear();

                var realm = await _databaseService.GetRealmInstanceAsync();

                // Load events from db for specific user
                var events = await _databaseService.GetAllEvents();

                // Service update
                _eventService.AddManyEvents(events);

                foreach (var eventModel in events)
                {
                    // UI update
                    EventList.Add(eventModel);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in InitializeEventListAsync: {ex}");
            }
        }

        private void OnTodayButtonClicked()
        {
            CurrentDate = DateTime.Today;
        }

        private async Task OnEventTapped(EventModel eventModel)
        {
            var message = $"Name: {eventModel.Name}\n" +
                $"Description: {eventModel.Description}\n\n" +
                $"Identifier: {eventModel.Id}\n" +
                $"Modified (UTC): {eventModel.LastModifiedDate:g}";
            await _dialogService.DisplayAlert("Event Information", message, "Ok");
        }

        partial void OnCurrentDateChanged(DateTime value)
        {
            PageDay = $"{value.Day} {value:dddd}, {value.ToString("MMM").ToUpper()}";

            GetEventForSelectedDate(value);
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

        private async Task Search()
        {
            await Shell.Current.GoToAsync(nameof(SearchPageView));
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

        private void OnEventUpdated(object sender, EventModel newEvents)
        {
        }

        private void OnEventsAdded(object sender, List<EventModel> newEvents)
        {
        }

        private void OnEventsRemoved(object sender, List<EventModel> newEvents)
        {
            EventList.Clear();
        }

        private void OnLoggedInEvent(object sender, LoginResult result)
        {
            InitializeEventListAsync();
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
