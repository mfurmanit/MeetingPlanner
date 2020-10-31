using System;
using System.ComponentModel.DataAnnotations;
using MeetingPlanner.Models;

namespace MeetingPlanner.Dto
{
    public class NotificationRequest
    {
        public Guid? Id { get; set; } = Guid.NewGuid();
        [Required] public int Quantity { get; set; }

        [Required] public NotificationUnit Unit { get; set; }
    }
}