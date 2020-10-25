using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingPlanner.Dto
{
    public class EventData
    {
        [Required] public DateTime Date { get; set; }
        public string HourFrom { get; set; }
        public string HourTo { get; set; }
        public bool WithTime { get; set; } = true;
        public bool Recurring { get; set; } = false;
        public bool Global { get; set; } = true;

        [Required] public string Title { get; set; }

        [Column(TypeName = "text")] public string Description { get; set; }
    }
}
