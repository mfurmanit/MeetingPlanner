using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IdentityServer4.Extensions;

namespace MeetingPlanner.Others.Utils
{
    public class TimeUtils
    {
        public static void ValidateHourFormat(string hour)
        {
            if (!hour.IsNullOrEmpty())
            {
                Match match = Regex.Match(hour, @"^([0-2][0-3]|[0-1][0-9]):[0-5][0-9]+$", RegexOptions.IgnoreCase);

                if (!match.Success)
                    throw new ArgumentException("Wprowadzono niewłaściwy format godziny!");
            }
        }

        public static void ValidateBothHours(string hourFrom, string hourTo)
        {
            if (!hourFrom.IsNullOrEmpty() && !hourTo.IsNullOrEmpty())
            {
                var timeFrom = hourFrom.Split(":");
                var timeTo = hourTo.Split(":");
                var minutesFrom = CountMinutes(timeFrom[0], timeFrom[1]);
                var minutesTo = CountMinutes(timeTo[0], timeTo[1]);
                
                if (minutesFrom > minutesTo)
                    throw new ArgumentException("Godzina rozpoczęcia nie może być większa niż godzina zakończenia spotkania!");
            }
        }

        private static int CountMinutes(string hours, string minutes)
        {
            return Convert.ToInt32(minutes) + (Convert.ToInt32(hours) * 60);
        }
    }
}
