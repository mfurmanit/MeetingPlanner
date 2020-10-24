using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingPlanner.Repositories
{
    public class MockEventRepository : IEventRepository
    {
        public IEnumerable<Event> GetAll()
        {
            return new List<Event>
            {
                new Event{Date = DateTime.Now, Global = true, Recurring = false, WithTime = false, Title = "Office Meeting", Description = "Take notebook!"},
                new Event{Date = DateTime.Now, Global = true, Recurring = false, WithTime = true, HourFrom = "08:00", HourTo = "10:00", Title = "Home Meeting", Description = "Meeting on WebEx."}
            };
        }

        public Event GetOneById()
        {
            throw new NotImplementedException();
        }

        public void DeleteOneById()
        {
            throw new NotImplementedException();
        }

        public void Save(Event eventObject)
        {
            throw new NotImplementedException();
        }

        public void Add(Event eventObject)
        {
            throw new NotImplementedException();
        }
    }
}
