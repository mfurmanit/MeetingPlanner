using System;
using System.ComponentModel.DataAnnotations;

namespace MeetingPlanner.Models
{
    public class Notification
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public NotificationUnit Unit { get; set; }
    }
}
