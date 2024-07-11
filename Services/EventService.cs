using Plugin.Maui.Calendar.Models;
using Taskinator.Models;

namespace Taskinator.Services
{
    public class EventService : IEventService
    {
        private readonly EventCollection _events = new EventCollection();
        private readonly IDialogService _dialogService;

        public EventCollection Events => _events;

        public event EventHandler<EventModel>? EventAdded;
        public event EventHandler<EventModel>? EventRemoved;
        public event EventHandler<EventModel>? EventUpdated;

        public event EventHandler<List<EventModel>>? EventsAdded;
        public event EventHandler<List<EventModel>>? EventsRemoved;

        public EventService(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public async Task AddEvent(EventModel newEvent)
        {
            // Check if Events already contains events for EventStartDateTime.Date
            if (_events.ContainsKey(newEvent.Starting.DateTime))
            {
                // Get the list of events for that date
                var eventList = (List<EventModel>)_events[newEvent.Starting.DateTime];

                // Add the new event to the list for that date
                eventList.Add(newEvent);
            }
            else
            {
                // Create a new list for events on this date and add the new event
                Events.Add(newEvent.Starting.DateTime, new List<EventModel> { newEvent });
            }
            // Raise event to notify listeners that an event was added
            EventAdded?.Invoke(this, newEvent);

        }

        public async Task RemoveEvent(EventModel eventToRemove)
        {
            if (eventToRemove == null)
                return;

            //Note: Convert from DateTimeOffset to DateTime
            if (_events.ContainsKey(eventToRemove.Starting.DateTime))
            {
                var eventList = (List<EventModel>)_events[eventToRemove.Starting.DateTime];

                if (eventList == null)
                    throw new Exception("No event list found");

                eventList.Remove(eventToRemove);

                if (eventList.Count == 0)
                    _events.Remove(eventToRemove.Starting.DateTime);

                // Raise event to notify listeners that an event was removed
                EventRemoved?.Invoke(this, eventToRemove);
            }
        }

        public Task<List<EventModel>> GetAllEvents()
        {
            // Collect all events from the EventCollection
            var allEvents = new List<EventModel>();
            foreach (var eventList in _events.Values)
            {
                allEvents.AddRange((List<EventModel>)eventList);
            }
            EventsAdded?.Invoke(this, allEvents);

            return Task.FromResult(allEvents);
        }

        public void RemoveAllEvents()
        {
            _events.Clear();
            Events.Clear();
            EventsRemoved?.Invoke(this, new List<EventModel>());
        }

        public void AddManyEvents(List<EventModel> newEvents)
        {
            foreach (var newEvent in newEvents)
            {
                if (_events.ContainsKey(newEvent.Starting.DateTime))
                {
                    var eventList = (List<EventModel>)_events[newEvent.Starting.DateTime];
                    eventList.Add(newEvent);
                }
                else
                {
                    Events.Add(newEvent.Starting.DateTime, new List<EventModel> { newEvent });
                }
            }
            EventsAdded?.Invoke(this, newEvents);
        }

        public void UpdateEvent(EventModel updatedEvent)
        {
            if (updatedEvent == null)
                return;

            if (_events.ContainsKey(updatedEvent.Starting.DateTime))
            {
                var eventList = (List<EventModel>)_events[updatedEvent.Starting.DateTime];
                var existingEvent = eventList.FirstOrDefault(e => e.Id == updatedEvent.Id);

                if (existingEvent != null)
                {
                    eventList.Remove(existingEvent);
                    eventList.Add(updatedEvent);

                    EventUpdated?.Invoke(this, updatedEvent);
                }
            }
            else
            {
                Events.Add(updatedEvent.Starting.DateTime, new List<EventModel> { updatedEvent });
                EventUpdated?.Invoke(this, updatedEvent);
            }
        }
    }

}
