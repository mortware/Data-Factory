using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DataFactory
{
    public class AddressFactory : DataFactoryBase
    {
        /// <summary>
        /// Generates a random first line for a UK address
        /// </summary>
        /// <returns></returns>
        public static string FirstLine()
        {
            return string.Format("{0} {1} {2}",
                Rand.Next(1, 120),
                StreetName(),
                StreetType());
        }

        /// <summary>
        /// Returns a random UK Town and County (Key = Town, Value = County)
        /// </summary>
        /// <returns></returns>
        public static KeyValuePair<string, string> LocalityAndRegion()
        {
            return Data.TownsAndCounties.ElementAt(Rand.Next(Data.TownsAndCounties.Count));
        }

        /// <summary>
        /// Generates a random UK Postcode. Note: Postcodes are not valid, but should not contain any invalid characters
        /// </summary>
        /// <returns></returns>
        public static string Postcode()
        {
            string first = Rand.Next(100) % 2 > 0
                               ? String.Format("{0}{1}",
                                               Data.PostcodeAlpha[Rand.Next(Data.PostcodeAlpha.Length)],
                                               Data.PostcodeAlpha[Rand.Next(Data.PostcodeAlpha.Length)])
                               : String.Format("{0}",
                                               Data.PostcodeAlpha[Rand.Next(Data.PostcodeAlpha.Length)]);

            string second = Rand.Next(1, 99).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');

            string last = String.Format("{0}{1}{2}",
                                  Rand.Next(0, 9),
                                  Data.PostcodeAlpha[Rand.Next(Data.PostcodeAlpha.Length)],
                                  Data.PostcodeAlpha[Rand.Next(Data.PostcodeAlpha.Length)]);

            return first + second + " " + last;
        }
        
        /// <summary>
        /// Returns a random street name
        /// </summary>
        /// <returns></returns>
        public static string StreetName()
        {
            return Properties.Resources.StreetNames.Split(',').GetRandom();
        }

        /// <summary>
        /// Returns a random street type (Street, Avenue, Crescent etc.)
        /// </summary>
        /// <returns></returns>
        public static string StreetType()
        {
            return Properties.Resources.StreetTypes.Split(',').GetRandom();
        }
    }
}
