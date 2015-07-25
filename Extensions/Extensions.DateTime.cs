using System;

namespace DataFactory
{
    public static partial class Extensions
    {
        public static string GetTimePhrase(this DateTime source, DateTime from)
        {
            string timeString;

            TimeSpan time = source - from;

            if (time.Seconds < 0)
                return "Never";

            if (time.Days > 31)
                timeString = "Over 1 month ago";
            else if (time.Days > 1)
                timeString = String.Format("{0} days ago", time.Days);
            else if (time.Days > 0)
                timeString = "Yesterday";
            else if (time.Hours > 1)
                timeString = String.Format("{0} hours ago", time.Hours);
            else if (time.Hours > 0)
                timeString = "1 hour ago";
            else if (time.Minutes > 1)
                timeString = String.Format("{0} minutes ago", time.Minutes);
            else if (time.Minutes > 0)
                timeString = "1 minute ago";
            else
                timeString = String.Format("{0} seconds ago", time.Seconds);

            return timeString;

        }

        /// <summary>
        /// Rounds the minute component to the nearest specified time. Defaults to 1 hour
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rounding"></param>
        /// <returns></returns>
        public static DateTime RoundMinutes(this DateTime source, MinuteRounding rounding = MinuteRounding.Hour)
        {
            if (rounding == MinuteRounding.QuarterHour)
                throw new NotImplementedException("Coming soon");

            switch (rounding)
            {
                case MinuteRounding.Hour:
                    if (source.Minute >= 30)
                        return source.AddHours(1).AddMinutes(-source.Minute).TrimSeconds();
                    return source.AddMinutes(-source.Minute).TrimSeconds();
                case MinuteRounding.HalfHour:
                    if (source.Minute < 15)
                        return source.AddMinutes(-source.Minute).TrimSeconds();
                    if (source.Minute >= 15 && source.Minute < 45)
                        return source.AddMinutes(-source.Minute).AddMinutes(30).TrimSeconds();
                    return source.AddHours(1).AddMinutes(-source.Minute).TrimSeconds();
                default:
                    throw new ArgumentOutOfRangeException("rounding");
            }
        }

        /// <summary>
        /// Rounds the second component to the nearest specified time. Defaults to 1 second
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rounding"></param>
        /// <returns></returns>
        public static DateTime RoundSeconds(this DateTime source, SecondRounding rounding = SecondRounding.Second)
        {
            switch (rounding)
            {
                case SecondRounding.Second:
                    if (source.Second >= 30)
                        return source.AddMinutes(1).TrimSeconds();
                    return source.AddSeconds(-source.Second).AddMilliseconds(-source.Millisecond);
                case SecondRounding.HalfSecond:
                    if (source.Second < 15)
                        return source.TrimSeconds();
                    if (source.Second >= 15 && source.Minute < 45)
                        return source.TrimSeconds().AddSeconds(30);
                    return source.AddMinutes(1).TrimSeconds();
                default:
                    throw new ArgumentOutOfRangeException("rounding");
            }
        }

        /// <summary>
        /// Trims the seconds from the <see cref="DateTime"></see> object
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTime TrimSeconds(this DateTime source)
        {
            return source.AddSeconds(-source.Second).AddMilliseconds(-source.Millisecond);
        }

        public static bool In(this DateTime source, DateTime start, DateTime end)
        {
            return source >= start && source <= end;
        }

        public static bool Intersects(this DateTime source, DateTime rangeStart, DateTime rangeEnd, TimeSpan? duration = null)
        {
            if (source > rangeEnd)
                return false;

            if (duration.HasValue)
            {
                var end = source.Add(duration.Value);
                if (end < rangeStart)
                    return false;

                if (end >= rangeStart && end < rangeEnd)
                    return true;
            }
            
            if (source >= rangeStart && source < rangeEnd)
                return true;
            
            return false;
        }
    }
}
