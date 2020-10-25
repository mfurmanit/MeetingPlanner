using System;
using System.Collections.Generic;
using System.Linq;
using MeetingPlanner.Data;

namespace MeetingPlanner.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;
        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Event> GetAll(string userId)
        {
            return _context.Events.Where(eventObject => false.Equals(eventObject.Global) && userId.Equals(eventObject.User.Id)).ToList();
        }

        public IEnumerable<Event> GetAllGlobal()
        {
            return _context.Events.Where(eventObject => true.Equals(eventObject.Global)).ToList();
        }

        public Event GetOneById(string id, bool global)
        {
            return _context.Events.FirstOrDefault(entity => id.Equals(entity.Id.ToString()) && global.Equals(entity.Global));
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
