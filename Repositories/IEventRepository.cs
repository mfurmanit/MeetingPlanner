using System.Collections.Generic;
using MeetingPlanner.Models;

namespace MeetingPlanner.Repositories
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAllGlobal();
        IEnumerable<Event> GetAllPersonal(string userId);
        IEnumerable<Event> GetAllWithNotifications();
        Event GetOneGlobal(string id);
        Event GetOnePersonal(string id);
        Event Update(Event eventObject);
        Event Add(Event eventObject);
        void DeleteOneById();
    }
}
