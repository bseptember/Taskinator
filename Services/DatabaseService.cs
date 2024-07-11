using IdentityModel.OidcClient;
using MongoDB.Bson;
using Realms;
using Taskinator.Models;

namespace Taskinator.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly RealmConfiguration _realmConfig;
        private readonly IDialogService _dialogService;
        private readonly object _realmLock = new object();
        private string _user = "user@taskinator.com";

        public event EventHandler<LoginResult>? loggedInEvent;

        public DatabaseService(RealmConfiguration realmConfig, IDialogService dialogService)
        {
            _realmConfig = realmConfig;
            _dialogService = dialogService;
        }
        
        public void SetLoggedInUser(LoginResult loginResult)
        {
            _user = loginResult.User.Claims.ToList().ElementAt(6).Value ?? "user@taskinator.com";
            loggedInEvent?.Invoke(this, loginResult);
        }

        public string GetLoggedInUser()
        {
            return _user;
        }

        public async Task<Realm> GetRealmInstanceAsync()
        {
            var realm = await Realm.GetInstanceAsync(_realmConfig);
            return realm;
        }

        public async Task AddEvent(EventModel newEvent)
        {
            var loggedInUser = GetLoggedInUser();
            if (loggedInUser != null)
            {
                try
                {
                    var realm = await GetRealmInstanceAsync();
                    using (var transaction = realm.BeginWrite())
                    {
                        newEvent.User = loggedInUser; // Set the owner username
                        realm.Add(newEvent);
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions, log or display error messages
                    await _dialogService.DisplayAlert("Error", $"Failed to add event: {ex.Message}", "Ok");
                }
            }
            else
            {
                await _dialogService.DisplayAlert("Please login", $"No user is logged in. Please go to the accounts page to login.", "Ok");
            }
        }

        public async Task RemoveEvent(ObjectId id)
        {
            var loggedInUser = GetLoggedInUser();
            if (loggedInUser != null)
            {
                try
                {
                    var realm = await GetRealmInstanceAsync();
                    var itemToDelete = realm.Find<EventModel>(id);
                    if (itemToDelete != null && itemToDelete.User == loggedInUser)
                    {
                        using (var transaction = realm.BeginWrite())
                        {
                            realm.Remove(itemToDelete);
                            transaction.Commit();
                        }
                    }
                    else
                    {
                        // Handle error: item not found or not owned by the logged-in user
                        await _dialogService.DisplayAlert("Not Found", $"Event not found or not owned by the logged-in user.", "Ok");
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions, log or display error messages
                    await _dialogService.DisplayAlert("Error", $"Failed to remove event: {ex.Message}", "Ok");
                }
            }
            else
            {
                await _dialogService.DisplayAlert("Please login", $"No user is logged in. Please go to the accounts page to login.", "Ok");
            }
        }

        public async Task<List<EventModel>> GetAllEvents()
        {
            var loggedInUser = GetLoggedInUser();
            if (loggedInUser != null)
            {
                try
                {
                    var realm = await GetRealmInstanceAsync();
                    var events = realm.All<EventModel>().Where(ti => ti.User == loggedInUser).ToList();
                    return events;
                }
                catch (Exception ex)
                {
                    // Handle exceptions, log or display error messages
                    await _dialogService.DisplayAlert("Error", $"Failed to get events: {ex.Message}", "Ok");
                    return new List<EventModel>();
                }
            }
            else
            {
                await _dialogService.DisplayAlert("Please login", $"No user is logged in. Please go to the accounts page to login.", "Ok");
                return new List<EventModel>();
            }
        }

        public async Task RemoveAllEvents()
        {
            var loggedInUser = GetLoggedInUser();
            if (loggedInUser != null)
            {
                try
                {
                    var realm = await GetRealmInstanceAsync();
                    using (var transaction = realm.BeginWrite())
                    {
                        var eventsToDelete = realm.All<EventModel>().Where(ti => ti.User == loggedInUser);
                        realm.RemoveRange(eventsToDelete);
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions, log or display error messages
                    await _dialogService.DisplayAlert("Error", $"Failed to remove all events: {ex.Message}", "Ok");
                }
            }
            else
            {
                await _dialogService.DisplayAlert("Please login", $"No user is logged in. Please go to the accounts page to login.", "Ok");
            }
        }

        public async Task AddManyEvents(List<EventModel> newEvents)
        {
            var loggedInUser = GetLoggedInUser();
            if (loggedInUser != null)
            {
                try
                {
                    var realm = await GetRealmInstanceAsync();
                    using (var transaction = realm.BeginWrite())
                    {
                        foreach (var item in newEvents)
                        {
                            item.User = loggedInUser; // Associate each item with the logged-in user
                            realm.Add(item);
                        }
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions, log or display error messages
                    await _dialogService.DisplayAlert("Error", $"Failed to add events: {ex.Message}", "Ok");
                }
            }
            else
            {
                await _dialogService.DisplayAlert("Please login", $"No user is logged in. Please go to the accounts page to login.", "Ok");
            }
        }

        public async Task UpdateEvent(EventModel updatedEvent)
        {
            var loggedInUser = GetLoggedInUser();
            if (loggedInUser != null)
            {
                try
                {
                    var realm = await GetRealmInstanceAsync();
                    using (var transaction = realm.BeginWrite())
                    {
                        // Ensure the item is associated with the logged-in user
                        updatedEvent.User = loggedInUser;
                        realm.Add(updatedEvent, update: true);
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions, log or display error messages
                    await _dialogService.DisplayAlert("Error", $"Failed to update event: {ex.Message}", "Ok");
                }
            }
            else
            {
                await _dialogService.DisplayAlert("Please login", $"No user is logged in. Please go to the accounts page to login.", "Ok");
            }
        }
    }
}