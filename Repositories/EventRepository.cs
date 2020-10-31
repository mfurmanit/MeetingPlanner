using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Extensions;
using MeetingPlanner.Data;
using MeetingPlanner.Models;
using MeetingPlanner.Others;
using Microsoft.EntityFrameworkCore;

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
            return _context.Events
                .Include(entity => entity.Notifications)
                .FirstOrDefault(entity => id.Equals(entity.Id.ToString()) && global.Equals(entity.Global));
        }

        public Event GetOnePersonal(string id)
        {
            return _context.Events
                .Include(entity => entity.Notifications)
                .AsNoTracking()
                .Include(entity => entity.User)
                .FirstOrDefault(entity => id.Equals(entity.Id.ToString()) && false.Equals(entity.Global));
        }

        public void DeleteOneById()
        {
            throw new NotImplementedException();
        }

        public Event Update(Event eventObject)
        {
            if (eventObject == null)
            {
                throw new ArgumentNullException("Przekazany argument nie może być pusty.");
            }

            RemoveNotifications(eventObject);
            _context.Events.Update(eventObject);
            _context.SaveChanges();

            return eventObject;
        }

        public Event Add(Event eventObject)
        {
            if (eventObject == null)
            {
                throw new ArgumentNullException("Przekazany argument nie może być pusty.");
            }

            _context.Events.Add(eventObject);
            _context.SaveChanges();

            return eventObject;
        }

        private void RemoveNotifications(Event eventObject)
        {
            if (eventObject.Global) return;
            
            var notificationsToRemove = 
                (from notification in _context.Notifications
                    where (notification.EventId == eventObject.Id
                           && !eventObject.Notifications.Contains(notification))
                    select notification);

            if (!notificationsToRemove.IsNullOrEmpty())
                _context.Notifications.RemoveRange(notificationsToRemove);
        }
    }
}
