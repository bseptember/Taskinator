using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Taskinator.Models;
using Taskinator.Services;
using Taskinator.Views;

namespace Taskinator.ViewModels
{
    public partial class EditEventPageViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IEventService _eventService;
        private readonly IDialogService _dialogService;
        private readonly IDatabaseService _databaseService;

        [ObservableProperty]
        private EventModel _event;

        public EditEventPageViewModel(IEventService eventService, IDialogService dialogService, IDatabaseService databaseService)
        {
            _eventService = eventService;
            _dialogService = dialogService;
            _databaseService = databaseService;
        }

        public AsyncRelayCommand UpdateCommand => new AsyncRelayCommand(OnUpdate);

        private async Task OnUpdate()
        {
            try
            {
                _eventService.UpdateEvent(Event);
                await _databaseService.UpdateEvent(Event);

                await _dialogService.DisplayAlert("Entry updated", $"An event was updated.\n{Event.Name}\n{Event.Description}", "Ok");

                if (Shell.Current.Navigation.NavigationStack.Count > 1)
                {
                    await Shell.Current.Navigation.PopAsync();
                }
                else
                {
                    // Handle the case where there is nothing to pop
                    await Shell.Current.GoToAsync(nameof(EditEventPageView));
                }
            }
            catch (Exception ex)
            {
                // Log the exception or display an error message
                await _dialogService.DisplayAlert("Error", $"An error occurred: {ex.Message}", "Ok");
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("EventModel"))
            {
                Event = query["EventModel"] as EventModel;
            }
        }
    }
}
