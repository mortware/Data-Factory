using System;
using System.Collections.Generic;
using System.Linq;

namespace DataFactory
{
    public class DateTimeFactory : DataFactoryBase
    {
        readonly DayOfWeek[] _days = { DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday };

        private bool IsLimitingTimes
        {
            get { return DayEnd > TimeSpan.Zero; }
        }
        private TimeSpan _dayStart;
        public TimeSpan DayStart
        {
            get { return _dayStart; }
            set
            {
                if (value < TimeSpan.Zero || value.Days >= 1)
                    throw new Exception("Time must be within 24 hours");
                _dayStart = value;
            }
        }

        private TimeSpan _dayEnd;
        public TimeSpan DayEnd
        {
            get { return _dayEnd; }
            set
            {
                if (value < TimeSpan.Zero || value.Days >= 1)
                    throw new Exception("Time must be within 24 hours");
                _dayEnd = value;
            }
        }

        public MinuteRounding MinuteRoundingMode { get; set; }
        public SecondRounding SecondRoundingMode { get; set; }

        private bool IsLimitingDays
        {
            get { return DaysToExlude.Any(); }
        }
        public List<DayOfWeek> DaysToExlude { get; private set; }

        public DateTimeFactory()
        {
            this.DaysToExlude = new List<DayOfWeek>();
            this.DayStart = TimeSpan.Zero;
            this.DayEnd = TimeSpan.Zero;
        }

        /// <summary>
        /// Creates a random <see cref="DateTime"></see> within the specified Day
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DateTime Create(DateTime date)
        {
            if (DaysToExlude.Any(d => d == date.DayOfWeek))
                throw new ArgumentException("Invalid DayOfWeek - remove specified DayOfWeek from the DaysToExclude list");

            if (DayStart >= DayEnd || DayEnd == TimeSpan.Zero)
                throw new ArgumentException("Invalid time range - DayEnd must be greater than DayStart");

            var validate = new Func<DateTime, bool>((d) =>
            {
                if (d < date.Date || d > date.Date.AddDays(1))
                    return false;

                return true;
            });

            var newDate = date.Date; // Remove the Time component from the Date
            do
            {
                newDate = new DateTime(LongRandom(newDate.Add(DayStart).Ticks, newDate.Add(DayEnd).Ticks), DateTimeKind.Utc);
                newDate = GetRandomValidTimeInDay(newDate);

                if (MinuteRoundingMode != MinuteRounding.None)
                    newDate = newDate.RoundMinutes(MinuteRoundingMode);

                if (SecondRoundingMode != SecondRounding.None)
                    newDate = newDate.RoundSeconds(SecondRoundingMode);

            } while (!validate(newDate));

            return newDate;
        }

        /// <summary>
        /// Creates a random <see cref="DateTime"></see> within the specified date range
        /// </summary>
        /// <param name="start">DateTime range start</param>
        /// <param name="end">DateTime range end</param>
        /// <returns></returns>
        public DateTime Create(DateTime start, DateTime end)
        {
            // Assertions

            if (start > end)
                throw new ArgumentException("End date must be greater than Start");

            if (MinuteRoundingMode == MinuteRounding.HalfHour && (end - start) < TimeSpan.FromMinutes(30))
                throw new ArgumentException("Range must be greater than 30 minutes");

            if (MinuteRoundingMode == MinuteRounding.QuarterHour && (end - start) < TimeSpan.FromMinutes(15))
                throw new ArgumentException("Range must be greater than 15 minutes");

            // If the range falls outside of working hours, then throw an error
            if (IsLimitingTimes && end - start < TimeSpan.FromDays(1) && start.Intersects(start.Date.Add(_dayStart), start.Date.Add(_dayEnd), end - start))
                throw new ArgumentException("Range falls outside of working hours");

            var validate = new Func<DateTime, bool>((d) =>
            {
                if (d < start || d > end)
                    return false;

                return true;
            });

            // Create random date
            DateTime date;
            int counter = 0;
            do
            {
                date = new DateTime(LongRandom(start.Ticks, end.Ticks), DateTimeKind.Utc);

                if (IsLimitingDays)
                {
                    var ndate = GetRandomValidDayInRange(start, end);
                    date = new DateTime(ndate.Year, ndate.Month, ndate.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
                }

                if (IsLimitingTimes)
                    date = GetRandomValidTimeInDay(date);

                if (MinuteRoundingMode != MinuteRounding.None)
                    date = date.RoundMinutes(MinuteRoundingMode);

                if (SecondRoundingMode != SecondRounding.None)
                    date = date.RoundSeconds(SecondRoundingMode);

                counter++;
                if (counter > 100)
                    throw new Exception("Could not create a DateTime within the specified range. Try using a larger range.");

            } while (!validate(date));


            return date;
        }

        /// <summary>
        /// Returns a random valid day that falls between the given dates. Retains the 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private DateTime GetRandomValidDayInRange(DateTime start, DateTime end)
        {
            var days = Enumerable.Range(0, 1 + end.Subtract(start).Days)
              .Select(offset => start.AddDays(offset));
            days = days.Where(d => !DaysToExlude.Any(e => e == d.DayOfWeek));
            return days.GetRandom();
        }

        private DateTime GetRandomValidTimeInDay(DateTime date)
        {
            date = date.AddHours(-date.Hour);
            date = date.AddHours(Rand.Next(DayStart.Hours, DayEnd.Hours));
            return date;
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
