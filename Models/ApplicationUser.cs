using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingPlanner.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Event> Event { get; set; } = new List<Event>();
    }
}
