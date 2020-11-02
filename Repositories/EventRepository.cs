using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Extensions;
using MeetingPlanner.Data;
using MeetingPlanner.Models;
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

        public IEnumerable<Event> GetAllGlobal()
        {
            return _context.Events.Where(eventObject => true.Equals(eventObject.Global)).ToList();
        }

        public IEnumerable<Event> GetAllPersonal(string userId)
        {
            return _context.Events.Where(eventObject => false.Equals(eventObject.Global) && userId.Equals(eventObject.User.Id)).ToList();
        }

        public IEnumerable<Event> GetAllWithNotifications()
        {
            return _context.Events.Where(eventObject => false.Equals(eventObject.Global) && eventObject.Notifications.Count > 0)
                .Include(entity => entity.Notifications)
                .Include(entity => entity.User)
                .ToList();
        }

        public Event GetOne(string id)
        {
            return _context.Events.AsNoTracking().FirstOrDefault(entity => id.Equals(entity.Id.ToString()));
        }

        public Event GetOneGlobal(string id)
        {
            return _context.Events
                .FirstOrDefault(entity => id.Equals(entity.Id.ToString()) && true.Equals(entity.Global));
        }

        public Event GetOnePersonal(string id)
        {
            return _context.Events
                .Include(entity => entity.Notifications)
                .AsNoTracking()
                .Include(entity => entity.User)
                .FirstOrDefault(entity => id.Equals(entity.Id.ToString()) && false.Equals(entity.Global));
        }

        public Event GetOneWithNotifications(string id)
        {
            return _context.Events.Include(entity => entity.Notifications)
                .FirstOrDefault(entity => id.Equals(entity.Id.ToString()));
        }

        public Event Update(Event eventObject, bool stateChanged)
        {
            if (eventObject == null)
                throw new ArgumentNullException("Przekazany argument nie może być pusty!");

            RemoveNotifications(eventObject, stateChanged);
            _context.Events.Update(eventObject);
            _context.SaveChanges();

            return eventObject;
        }

        public Event Add(Event eventObject)
        {
            if (eventObject == null)
                throw new ArgumentNullException("Przekazany argument nie może być pusty!");

            _context.Events.Add(eventObject);
            _context.SaveChanges();

            return eventObject;
        }

        public void Delete(Event eventObject)
        {
            if (eventObject == null)
                throw new ArgumentNullException("Przekazany argument nie może być pusty!");

            if (!eventObject.Global && !eventObject.Notifications.IsNullOrEmpty()) 
                _context.Notifications.RemoveRange(eventObject.Notifications);

            _context.Events.Remove(eventObject);
            _context.SaveChanges();
        }

        private void RemoveNotifications(Event eventObject, bool stateChanged)
        {
            if (eventObject.Global && !stateChanged) return;
            
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
