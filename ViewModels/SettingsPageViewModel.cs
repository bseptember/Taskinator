using CommunityToolkit.Mvvm.Input;
using Taskinator.Services;

namespace Taskinator.ViewModels
{
    public partial class SettingsPageViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;
        private readonly IEventService _eventService;
        private readonly IDialogService _dialogService;

        public SettingsPageViewModel(IDatabaseService databaseService, IEventService eventService, IDialogService dialogService)
        {
            _databaseService = databaseService;
            _eventService = eventService;
            _dialogService = dialogService;

            ClearCacheCommand = new AsyncRelayCommand(ClearCache);
            DeleteDatabaseCommand = new AsyncRelayCommand(DeleteDatabase);
        }

        public IAsyncRelayCommand ClearCacheCommand { get; }
        public IAsyncRelayCommand DeleteDatabaseCommand { get; }

        private async Task ClearCache()
        {
            bool isConfirmed = await _dialogService.DisplayAlert("Confirmation", "Are you sure you want to clear the cache?", "Yes", "No");
            if (isConfirmed)
            {
                _eventService.RemoveAllEvents();
                await _dialogService.DisplayAlert("Delete cache", "All local events have been cleared.", "Ok");
            }
        }

        private async Task DeleteDatabase()
        {
            bool isConfirmed = await _dialogService.DisplayAlert("Confirmation", "Are you sure you want to delete the database?", "Yes", "No");
            if (isConfirmed)
            {
                _eventService.RemoveAllEvents();
                await _databaseService.RemoveAllEvents();
                await _dialogService.DisplayAlert("Database deleted", "All events have been deleted from the device.", "OK");
            }
        }
    }
}
