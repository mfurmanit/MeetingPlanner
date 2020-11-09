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

        public static void ValidateEventDateOnCreate(DateTime dateTime)
        {
            if (StartOfDay(dateTime) < StartOfDay(DateTime.Now))
                throw new ArgumentException("Nie można tworzyć spotkań posiadających datę wcześniejszą niż dzisiejsza!");
        }

        public static void ValidateEventDateOnUpdate(DateTime requestDateTime, DateTime dbDateTime)
        {
            if (requestDateTime != dbDateTime && StartOfDay(requestDateTime) < StartOfDay(DateTime.Now))
                throw new ArgumentException("Data spotkania może zostać zaktualizowana wyłącznie na późniejszą niż dzisiejsza!");
        }

        public static DateTime StartOfDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0,
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

        private static DateTime StartOfPreviousMonth(DateTime dateTime)
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

        private static DateTime EndOfNextMonth(DateTime dateTime)
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
    }
}
