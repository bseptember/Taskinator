namespace Taskinator.Models
{
    public class LogoutEvent
    {
        public DateTime Timestamp { get; private set; }

        public LogoutEvent(DateTime timestamp)
        {
            Timestamp = timestamp;
        }
    }
}
