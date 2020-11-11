using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using IdentityServer4.Extensions;
using MeetingPlanner.Data;
using MeetingPlanner.Models;
using MeetingPlanner.Others.Utils;
using MeetingPlanner.Repositories;
using Microsoft.Extensions.Logging;

namespace MeetingPlanner.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventRepository _eventRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ApplicationDbContext context, IEventRepository eventRepository,
            IEmailService emailService, ILogger<NotificationService> logger)
        {
            _context = context;
            _eventRepository = eventRepository;
            _emailService = emailService;
            _logger = logger;
        }

        public void ResolveAndSendNotifications()
        {
            var events = _eventRepository.GetAllWithNotifications().ToList();
            var notificationsToRemove = new List<Notification>();
            events.ForEach(eventObject => eventObject.Notifications.ForEach(notification =>
                CheckAndPrepareNotification(eventObject, notification, notificationsToRemove)));
            RemoveNotifications(notificationsToRemove);
        }

        private void CheckAndPrepareNotification(Event eventObject, Notification notification, List<Notification> notificationsToRemove)
        {
            var eventDate = eventObject.Date;
            var withHourFrom = !eventObject.HourFrom.IsNullOrEmpty();
            var daysUnit = notification.Unit == NotificationUnit.Days;
            var weeksUnit = notification.Unit == NotificationUnit.Weeks;
            var daysOrWeeks = daysUnit || weeksUnit;

            if (!withHourFrom && daysOrWeeks)
            {
                var quantity = daysUnit ? notification.Quantity : (notification.Quantity * 7);
                var notificationDate = eventDate.AddDays(-quantity);
                CheckShortDateAndResolve(eventObject, notification, notificationDate, notificationsToRemove);
            }
            else if (withHourFrom)
            {
                var time = eventObject.HourFrom.Split(":");
                var hours = Convert.ToDouble(time[0]);
                var minutes = Convert.ToDouble(time[1]);
                var eventDateAndTime = eventDate.AddHours(hours).AddMinutes(minutes);
                var notificationDate = ResolveNotificationDate(notification, eventDateAndTime);
                CheckFullDateAndResolve(eventObject, notification, notificationDate, notificationsToRemove);
            }
        }

        private void CheckShortDateAndResolve(Event eventObject, Notification notification,
            DateTime notificationDate, List<Notification> toRemove)
        {
            if (notificationDate.ToShortDateString() == DateTime.Today.ToShortDateString())
            {
                _logger.LogInformation($"Sending notification for event '{eventObject.Title} - {eventObject.Id}.");
                toRemove.Add(notification);
                _emailService.SendNotification(eventObject);
                _logger.LogInformation("E-mail should be sent");
            }
        }

        private void CheckFullDateAndResolve(Event eventObject, Notification notification,
            DateTime notificationDate, List<Notification> toRemove)
        {
            if (notificationDate.Equals(DateUtils.CurrentDate()))
            {
                _logger.LogInformation($"Sending notification for event '{eventObject.Title} - {eventObject.Id}.");
                toRemove.Add(notification);
                _emailService.SendNotification(eventObject);
                _logger.LogInformation("E-mail should be sent.");
            }
        }

        private void RemoveNotifications(List<Notification> toRemove)
        {
            if (!toRemove.IsNullOrEmpty())
            {
                _context.Notifications.RemoveRange(toRemove);
                _context.SaveChanges();
            }
        }

        private DateTime ResolveNotificationDate(Notification notification, DateTime dateTime)
        {
            return notification.Unit switch
            {
                NotificationUnit.Minutes => dateTime.AddMinutes(-notification.Quantity),
                NotificationUnit.Hours => dateTime.AddHours(-notification.Quantity),
                NotificationUnit.Days => dateTime.AddDays(-notification.Quantity),
                NotificationUnit.Weeks => dateTime.AddDays(-(notification.Quantity * 7)),
                _ => throw new InvalidEnumArgumentException("Wskazana jednostka powiadomienia nie istnieje!")
            };
        }
    }
}
