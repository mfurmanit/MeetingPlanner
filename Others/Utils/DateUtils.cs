using System;

namespace MeetingPlanner.Others.Utils
{
    public class DateUtils
    {
        public static DateTime CurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour,
                dateTime.Minute, 0, 0, dateTime.Kind);
        }
    }
}
