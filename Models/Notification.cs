using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingPlanner.Models
{
    public class Notification
    {
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public NotificationUnit Unit { get; set; }
    }
}
