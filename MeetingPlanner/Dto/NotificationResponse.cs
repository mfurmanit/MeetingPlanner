using System;
using System.ComponentModel.DataAnnotations;
using MeetingPlanner.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MeetingPlanner.Dto
{
    public class NotificationResponse
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required] public int Quantity { get; set; }

        [Required] [JsonConverter(typeof(StringEnumConverter))] public NotificationUnit Unit { get; set; }
    }
}