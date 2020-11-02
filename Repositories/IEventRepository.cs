using System.Collections.Generic;
using MeetingPlanner.Models;

namespace MeetingPlanner.Repositories
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAllGlobal();
        IEnumerable<Event> GetAllPersonal(string userId);
        IEnumerable<Event> GetAllWithNotifications();
        Event GetOne(string id);
        Event GetOneWithNotifications(string id);
        Event GetOneGlobal(string id);
        Event GetOnePersonal(string id);
        Event Update(Event eventObject, bool stateChanged);
        Event Add(Event eventObject);
        void Delete(Event eventObject);
    }
}
