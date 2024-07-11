using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Taskinator.Models;
using Taskinator.Views;
using Taskinator.Services;

namespace Taskinator.ViewModels
{
    public partial class SearchPageViewModel : BaseViewModel
    {
        private readonly IEventService _eventService;
        private readonly IDialogService _dialogService;
        private readonly IDatabaseService _databaseService;

        public SearchPageViewModel(IEventService eventService, IDialogService dialogService, IDatabaseService databaseService)
        {
            _eventService = eventService;
            _dialogService = dialogService;
            _databaseService = databaseService;

            // Subscribe to events being added or removed
            _eventService.EventAdded += (sender, args) => UpdateFilteredEvents();
            _eventService.EventRemoved += (sender, args) => UpdateFilteredEvents();
        }

        private string _searchQuery = "";
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged();
                    UpdateFilteredEvents(); // Call method to update filtered events
                }
            }
        }

        private ObservableCollection<EventModel> _filteredEvents = new ObservableCollection<EventModel>();
        public ObservableCollection<EventModel> FilteredEvents
        {
            get => _filteredEvents;
            set => SetProperty(ref _filteredEvents, value);
        }

        public ICommand EventTappedCommand => new AsyncRelayCommand<EventModel>(OnEventTapped);

        private async Task OnEventTapped(EventModel eventModel)
        {
            var choice = await _dialogService.DisplayPromptDeleteOrEdit();
            switch (choice)
            {
                case "Edit":
                    await ExecuteEditCommand(eventModel);
                    break;
                case "Delete":
                    await ExecuteDeleteCommand(eventModel);
                    break;
                default:
                    // Handle other cases if needed
                    break;
            }
        }

        private async Task ExecuteDeleteCommand(EventModel eventModel)
        {
            bool isConfirmed = await _dialogService.DisplayAlert("Confirmation", "Are you sure you want to delete this event?", "Yes", "No");
            if (isConfirmed)
            {
                await _eventService.RemoveEvent(eventModel);
                await _databaseService.RemoveEvent(eventModel.Id);
                await _dialogService.DisplayAlert("Deleted", $"The event {eventModel.Name} has been removed.", "Ok");
            }
        }

        private async Task ExecuteEditCommand(EventModel eventModel)
        {
            if (eventModel == null) return;

            var navigationParameters = new Dictionary<string, object>
            {
                { "EventModel", eventModel }
            };

            await Shell.Current.GoToAsync(nameof(EditEventPageView), navigationParameters);
            SearchQuery = string.Empty;
        }

        private void UpdateFilteredEvents()
        {
            var filteredEventsList = new List<EventModel>();

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                // Iterate through each date's list of events
                foreach (var eventList in _eventService.Events.Values)
                {
                    // Check if there are events matching the search criteria
                    foreach (EventModel eventModel in eventList)
                    {
                        if (eventModel.Name.ToLower().Contains(SearchQuery.ToLower()))
                        {
                            // Add the event to filteredEventsList
                            filteredEventsList.Add(eventModel);
                        }
                    }
                }
            }

            // Update FilteredEvents with the filtered list
            FilteredEvents = new ObservableCollection<EventModel>(filteredEventsList);
        }
    }
}
