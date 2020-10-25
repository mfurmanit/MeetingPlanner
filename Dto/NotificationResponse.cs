using System;

namespace MeetingPlanner.Dto
{
    public class NotificationResponse : NotificationRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}