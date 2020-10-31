using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MeetingPlanner.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Event> Event { get; set; } = new List<Event>();
    }
}
