using System;

namespace DataFactory
{
    public class DataFactoryBase
    {
        internal static readonly Random Rand = new Random();

        public static bool RandomBoolean
        {
            get { return Rand.NextDouble() > 0.5; }
        }

        public static Gender RandomGender
        {
            get { return RandomBoolean ? Gender.Male : Gender.Female; }
        }

        internal static string GetRandomResource()
        {
            return "";
        }
    }
}
