using IdentityModel.OidcClient;
using MongoDB.Bson;
using Realms;
using Taskinator.Models;

namespace Taskinator.Services
{
    public interface IDatabaseService
    {
        public event EventHandler<LoginResult>? loggedInEvent;

        Task<Realm> GetRealmInstanceAsync();

        void SetLoggedInUser(LoginResult loginResult);

        string GetLoggedInUser();

        Task AddEvent(EventModel newEvent);

        Task RemoveEvent(ObjectId id);

        Task<List<EventModel>> GetAllEvents();

        Task RemoveAllEvents();

        Task AddManyEvents(List<EventModel> newEvents);

        Task UpdateEvent(EventModel newEvent);
    }
}
