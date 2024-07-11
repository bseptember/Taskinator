using Plugin.Maui.Calendar.Models;
using Taskinator.Models;

namespace Taskinator.Services
{
    public interface IEventService
    {
        public EventCollection Events { get; } // Keep read-only for the interface

        public event EventHandler<EventModel>? EventAdded;
        public event EventHandler<EventModel>? EventRemoved;
        public event EventHandler<EventModel>? EventUpdated;

        public event EventHandler<List<EventModel>>? EventsAdded;
        public event EventHandler<List<EventModel>>? EventsRemoved;

        Task AddEvent(EventModel newEvent);

        Task RemoveEvent(EventModel eventToRemove);

        Task<List<EventModel>> GetAllEvents();

        void RemoveAllEvents();

        void AddManyEvents(List<EventModel> newEvents);

        void UpdateEvent(EventModel newEvent);
    }
}
