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
    }
}
