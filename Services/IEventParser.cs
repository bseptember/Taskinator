using Taskinator.Models;

namespace Taskinator.Services
{
    public interface IEventParser
    {
        EventModel ParseEventFromText(string text);
    }
}
