using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingPlanner.Repositories
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAllGlobal();
        IEnumerable<Event> GetAll(string userId);
        Event GetOneById(string id, bool global);
        void DeleteOneById();
        void Save(Event eventObject);
        void Add(Event eventObject);
    }
}
