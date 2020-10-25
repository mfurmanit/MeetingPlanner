using System;
using System.Collections.Generic;

namespace MeetingPlanner.Dto
{
    public class EventResponse : EventData
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<NotificationResponse> Notifications { get; set; } = new List<NotificationResponse>();
    }
}