using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Volunteers.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTimeOffset SetOffset(this DateTimeOffset dto, int timeZoneDiff)
        {
            return new DateTimeOffset(dto.DateTime, TimeSpan.FromHours(timeZoneDiff));
        }
        public static string GetLocalTimezone(int number)
        {
            if (number == 7)
                return "WIB";
            else if (number == 8)
                return "WITA";
            else if (number == 9)
                return "WIT";
            else if (number < 0)
                return "UTC" + number;
            else return "UTC+" + number;
        }
        public static string GetLocalDay(DateTimeOffset datetime)
        {
            return datetime.ToString("dddd", new CultureInfo("id-ID"));
        }
        public static string GetLocalTime(DateTimeOffset datetime)
        {
            var timezone = GetLocalTimezone(datetime.Offset.Hours);
            return datetime.ToString("HH:mm " + timezone);
        }
        public static string GetLocalDate(DateTimeOffset dateTime)
        {
            return dateTime.ToString("dd MMMM yyyy", new CultureInfo("id-ID"));
        }
        public static string GetLocalDateTime(DateTimeOffset datetime)
        {
            var timezone = GetLocalTimezone(datetime.Offset.Hours);
            return datetime.ToString("dd-MM-yyyy HH:mm " + timezone, new CultureInfo("id-ID"));
        }
        public static string GetLocalDateTimeZone(DateTimeOffset datetime, int timeZone)
        {
            var timezone = GetLocalTimezone(timeZone);
            return datetime.AddHours(timeZone).ToString("dd-MM-yyyy HH:mm " + timezone, new CultureInfo("id-ID"));
        }
        public static string GetTime(DateTimeOffset datetime)
        {
            return datetime.ToString("HH:mm ");
        }
    }
}