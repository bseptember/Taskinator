using MongoDB.Bson;
using Realms;

namespace Taskinator.Models
{
    public class EventModel : RealmObject
    {
        #region Properties

        [PrimaryKey]
        public ObjectId Id { get; private set; } = ObjectId.GenerateNewId();

        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset Starting { get; set; }

        public DateTimeOffset Ending { get; set; }

        public int Priority { get; set; } = 0;

        public bool IsComplete { get; set; } = false;

        public DateTimeOffset LastModifiedDate { get; set; }

        public IList<DateTimeOffset> SuggestedDate { get; } = new List<DateTimeOffset>();

        // Reference to who owns this Event
        public string? User { get; set; }

        #endregion

        #region Computed Properties

        public bool NotComplete => !IsComplete;

        public Color Color => Options.PriorityColors[Priority];

        public bool IsValidDueDate => Ending > DateTimeOffset.Now;

        public bool IsValidStartDate => Starting < Ending;

        public int StartMinute => (int)(Starting.Minute + Starting.Hour * 60);

        public int DurationInMinutes => (int)((Ending - Starting).TotalMinutes);

        #endregion

    }
}
