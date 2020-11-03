using System.Collections.Generic;

namespace MeetingPlanner.Dto
{
    public class EventRequest : EventData
    {
        public List<NotificationRequest> Notifications { get; set; } = new List<NotificationRequest>();
    }
}