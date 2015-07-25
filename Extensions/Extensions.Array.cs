using System;
using System.Collections.Generic;
using System.Linq;

namespace DataFactory
{
    public static partial class Extensions
    {
		internal static readonly Random Random = new Random();

		/// <summary>
		/// Returns a random item from a collection <see cref="IEnumerable"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="values"></param>
		/// <returns></returns>
        public static T GetRandom<T>(this IEnumerable<T> values)
        {
            lock (Random)
            {
                var valueArray = values.ToArray();
                return valueArray[Random.Next(valueArray.Length)];
            }
        }

        /// <summary>
        /// Returns a random item from an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this T[] values)
        {
            lock (Random)
            {
                return values[Random.Next(values.Length)];
            }
        }
    }
}
