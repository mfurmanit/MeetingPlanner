using System;
using MeetingPlanner.Dto;

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

        public static DateTime StartOfPreviousMonth(DateTime dateTime)
        {
            var month = dateTime.Month;
            var year = dateTime.Year;

            if (month == 1)
            {
                month = 12;
                year -= 1;
            }
            else
            {
                month -= 1;
            }

            return new DateTime(year, month, 1, 0,
                0, 0, 0, dateTime.Kind);
        }

        public static DateTime EndOfNextMonth(DateTime dateTime)
        {
            var month = dateTime.Month;
            var year = dateTime.Year;

            if (month == 12)
            {
                month = 1;
                year += 1;
            }
            else
            {
                month += 1;
            }

            var daysInMonth = DateTime.DaysInMonth(year, month);

            return new DateTime(year, month, daysInMonth, 0,
                0, 0, 0, dateTime.Kind);
        }

        public static DateRange GetDateRange(DateTime dateTime)
        {
            return new DateRange()
            {
                DateFrom = StartOfPreviousMonth(dateTime),
                DateTo = EndOfNextMonth(dateTime)
            };
        }
    }
}
