using System;

namespace DataFactory
{
    public class DateTimeGenerator : DataFactoryBase
    {
        private readonly static TimeSpan WorkingDayStart = TimeSpan.FromHours(8);
        private readonly static TimeSpan WorkingDayEnd = TimeSpan.FromHours(18);

        /// <summary>
        /// Generates a random date between the specified dates
        /// </summary>
        /// <param name="start">Start date (Defaults to now)</param>
        /// <param name="range">Length of time from the start date (Defaults to 1 year)</param>
        /// <param name="roundHalfHour">Rounds time to the nearest half hour</param>
        /// <param name="roundMinutes">Rounds time to the nearest minute</param>
        /// <param name="workingHours">Limits the time range between 8am and 6pm - Monday to Friday</param>
        /// <returns></returns>
        public static DateTime Date(DateTime? start = null, TimeSpan? range = null, bool roundHalfHour = false, bool roundMinutes = false, bool workingHours = false)
        {
            var startValue = start ?? DateTime.Now;
            var rangeValue = range ?? TimeSpan.FromDays(365);

            // Create random date
            var date = new DateTime(LongRandom(startValue.Ticks, (startValue + rangeValue).Ticks));

            if (roundHalfHour)
                date = date.RoundMinutes(MinuteRounding.HalfHour);

            if (roundMinutes)
                date = date.TrimSeconds();

            if (workingHours)
                date = LimitToWorkingHours(date);

            return date;
        }

        /// <summary>
        /// Generates a random <see cref="DateTime"></see> object based on the specified date range
        /// </summary>
        /// <param name="start">DateTime range start</param>
        /// <param name="end">DateTime range end</param>
        /// <param name="minuteRounding">Rounds time to the specified minute rounding</param>
        /// <param name="secondRounding">Rounds time to the specified second rounding</param>
        /// <param name="workingHours">Limits the time range between 8am and 6pm - Monday to Friday</param>
        /// <returns></returns>
        public static DateTime WithinRange(DateTime start, DateTime end, MinuteRounding minuteRounding = MinuteRounding.None, SecondRounding secondRounding = SecondRounding.None, bool workingHours = false)
        {
            // Assertions

            if (start > end)
                throw new ArgumentException("End date must be greater than Start");

            if (minuteRounding == MinuteRounding.HalfHour && (end - start) < TimeSpan.FromMinutes(30))
                throw new ArgumentException("Range must be greater than 30 minutes");

            if (minuteRounding == MinuteRounding.QuarterHour && (end - start) < TimeSpan.FromMinutes(15))
                throw new ArgumentException("Range must be greater than 15 minutes");

            // If the range falls outside of working hours, then throw an error
            if (workingHours && end - start < TimeSpan.FromDays(1) && start.Intersects(start.Date.Add(WorkingDayStart), start.Date.Add(WorkingDayEnd), end - start))
                throw new ArgumentException("Range falls outside of working hours");

            // Create random date
            DateTime date;
            int counter = 0;
            do
            {
                date = new DateTime(LongRandom(start.Ticks, end.Ticks));

                if (minuteRounding != MinuteRounding.None)
                    date = date.RoundMinutes(minuteRounding);

                if (secondRounding != SecondRounding.None)
                    date = date.RoundSeconds(secondRounding);

                if (workingHours)
                    date = LimitToWorkingHours(date);

                counter++;
                if (counter > 100)
                    throw new Exception("Could not create a DateTime within the specified range. Try using a larger range.");

            } while (date < start || date > end);


            return date;
        }

        private static DateTime LimitToWorkingHours(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(-date.Day);
                date = date.AddDays(Rand.Next(1, 6));
            }
            if (IsBeforeWorkingHours(date) || IsAfterWorkingHours(date))
            {
                date = date.AddHours(-date.Hour);
                date = date.AddHours(Rand.Next(WorkingDayStart.Hours, WorkingDayEnd.Hours));
            }
            return date;
        }

        private static bool IsBeforeWorkingHours(DateTime date)
        {
            return TimeSpan.FromHours(date.Hour) < WorkingDayStart;
        }

        private static bool IsAfterWorkingHours(DateTime date)
        {
            return TimeSpan.FromHours(date.Hour) > WorkingDayEnd;
        }

        private static bool IsWorkingHours(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return false;
            if (IsBeforeWorkingHours(date) || IsAfterWorkingHours(date))
                return false;
            return true;
        }

        private static long LongRandom(long min, long max)
        {
            var buf = new byte[8];
            Rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }
    }
}
