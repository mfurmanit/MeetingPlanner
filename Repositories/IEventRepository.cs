using System.Collections.Generic;
using MeetingPlanner.Models;

namespace MeetingPlanner.Repositories
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAllGlobal();
        IEnumerable<Event> GetAll(string userId);
        Event GetOneById(string id, bool global);
        void DeleteOneById();
        Event Update(Event eventObject);
        Event Add(Event eventObject);
    }
}
