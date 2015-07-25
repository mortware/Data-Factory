using System;

namespace DataFactory
{
    public class ColorFactory : DataFactoryBase
    {
        /// <summary>
        /// Returns a random color as a hex value string (e.g. #FF00CC)
        /// </summary>
        /// <returns></returns>
        public static string Hex()
        {
            return String.Format("#{0:X6}", Rand.Next(0x1000000));
        }
    }
}
