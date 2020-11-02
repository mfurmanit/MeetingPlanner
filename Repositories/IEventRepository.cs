using System.Collections.Generic;
using MeetingPlanner.Dto;
using MeetingPlanner.Models;

namespace MeetingPlanner.Repositories
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAllGlobal(DateRange dateRange);
        IEnumerable<Event> GetAllPersonal(DateRange dateRange, string userId);
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
