using System.ComponentModel.DataAnnotations;
using MeetingPlanner.Models;

namespace MeetingPlanner.Dto
{
    public class NotificationRequest
    {
        [Required] public int Quantity { get; set; }

        [Required] public NotificationUnit Unit { get; set; }
    }
}