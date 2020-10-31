using System.Runtime.Serialization;

namespace MeetingPlanner.Models
{
    public enum NotificationUnit
    {
        [EnumMember(Value = "Minutes")]
        Minutes,
        [EnumMember(Value = "Hours")]
        Hours,
        [EnumMember(Value = "Days")]
        Days,
        [EnumMember(Value = "Weeks")]
        Weeks
    }
}
