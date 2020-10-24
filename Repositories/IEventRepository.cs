using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingPlanner.Repositories
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAll();
        Event GetOneById();
        void DeleteOneById();
        void Save(Event eventObject);
        void Add(Event eventObject);
    }
}
