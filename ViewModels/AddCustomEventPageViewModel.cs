using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers.Commands;
using System.Windows.Input;
using Taskinator.Models;
using Taskinator.Services;

namespace Taskinator.ViewModels
{
    public partial class AddCustomEventPageViewModel : BaseViewModel
    {
        private readonly IEventService _eventService;
        private readonly IDialogService _dialogService;
        private readonly IDatabaseService _databaseService;

        public AddCustomEventPageViewModel(IEventService eventService, IDialogService dialogService, IDatabaseService databaseService)
        {
            _eventService = eventService;
            _dialogService = dialogService;
            _databaseService = databaseService;

            AddEventCommand = new AsyncCommand(AddEventAsync);
            RemoveEventCommand = new AsyncCommand<EventModel>(RemoveEventAsync);
        }

        [ObservableProperty]
        private string eventName = string.Empty;

        [ObservableProperty]
        private string eventDescription = string.Empty;

        [ObservableProperty]
        private int eventPriority;

        private DateTime _eventStartDateTime = DateTime.Now;
        public DateTime EventStartDateTime
        {
            get => _eventStartDateTime;
            set
            {
                if (_eventStartDateTime != value)
                {
                    _eventStartDateTime = value.Date + EventStartTime;
                    OnPropertyChanged(); // Ensure this method exists in your BaseViewModel class
                }
            }
        }

        private DateTime _eventEndDateTime = DateTime.Now;
        public DateTime EventEndDateTime
        {
            get => _eventEndDateTime;
            set
            {
                if (_eventEndDateTime != value)
                {
                    _eventEndDateTime = value.Date + EventEndTime;
                    OnPropertyChanged(); // Ensure this method exists in your BaseViewModel class
                }
            }
        }

        private TimeSpan _eventStartTime = DateTime.Now.TimeOfDay;
        public TimeSpan EventStartTime
        {
            get => _eventStartTime;
            set
            {
                if (_eventStartTime != value)
                {
                    _eventStartTime = value;
                    EventStartDateTime = EventStartDateTime.Date + _eventStartTime;
                    OnPropertyChanged(); // Ensure this method exists in your BaseViewModel class
                }
            }
        }

        private TimeSpan _eventEndTime = DateTime.Now.TimeOfDay;
        public TimeSpan EventEndTime
        {
            get => _eventEndTime;
            set
            {
                if (_eventEndTime != value)
                {
                    _eventEndTime = value;
                    EventEndDateTime = EventEndDateTime.Date + _eventEndTime;
                    OnPropertyChanged(); // Ensure this method exists in your BaseViewModel class
                }
            }
        }

        public ICommand AddEventCommand { get; }
        public ICommand RemoveEventCommand { get; }

        private async Task AddEventAsync()
        {
            // Ensure start time is before end time
            if (EventStartDateTime >= EventEndDateTime)
            {
                await _dialogService.DisplayAlert("Error", "End time must be after start time.", "Ok");
                return;
            }

            // Create a new event with user inputs
            var newEvent = new EventModel
            {
                Name = EventName,
                Description = EventDescription,
                Starting = new DateTimeOffset(EventStartDateTime),
                Ending = new DateTimeOffset(EventEndDateTime),
                Priority = EventPriority,
                LastModifiedDate = new DateTimeOffset(DateTime.Now),
                User = _databaseService.GetLoggedInUser()
            };

            // Add locally
            _eventService.AddEvent(newEvent);

            // Add to database storage
            await _databaseService.AddEvent(newEvent);

           // Message alert success
            await _dialogService.DisplayAlert("Entry saved", $"A new event was added.\n{newEvent.Name}\n{newEvent.Description}", "Ok");

            // Optionally clear inputs for the next event
            ClearInputs();

            // Navigate back
            await Shell.Current.Navigation.PopAsync();
        }

        private void ClearInputs()
        {
            EventName = string.Empty;
            EventDescription = string.Empty;
            EventPriority = 0;
            EventStartDateTime = DateTime.Now;
            EventEndDateTime = DateTime.Now;
            EventStartTime = DateTime.Now.TimeOfDay;
            EventEndTime = DateTime.Now.TimeOfDay;
        }

        private async Task RemoveEventAsync(EventModel eventToRemove)
        {
            _eventService.RemoveEvent(eventToRemove);
            await _databaseService.RemoveEvent(eventToRemove.Id);

            await _dialogService.DisplayAlert("Removal cache", $"An event was removed.\n{eventToRemove.Name}", "Ok");
        }  

    }
}