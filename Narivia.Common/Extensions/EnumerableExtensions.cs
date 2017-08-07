using System;
using System.Collections.Generic;
using System.Linq;

namespace Narivia.Common.Extensions
{
    /// <summary>
    /// Enumerable extensions.
    /// </summary>
    public static class EnumerableExtensions
    {
        static Random random;

        /// <summary>
        /// Gets the duplicated elements.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <returns>The duplicated elements.</returns>
        public static IEnumerable<T> GetDuplicates<T>(this IEnumerable<T> source)
        {
            HashSet<T> itemsSeen = new HashSet<T>();
            HashSet<T> itemsYielded = new HashSet<T>();

            foreach (T item in source)
            {
                if (!itemsSeen.Add(item))
                {
                    if (itemsYielded.Add(item))
                    {
                        yield return item;
                    }
                }
            }
        }

        /// <summary>
        /// Checks wether the collection is empty.
        /// </summary>
        /// <param name="enumerable">The collection.</param>
        /// <returns>True if the collection is empty, false otherwise.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }

            return enumerable.Count() < 1;
        }

        /// <summary>
        /// Gets a random element.
        /// </summary>
        /// <returns>The element.</returns>
        /// <param name="enumerable">Enumerable.</param>
        public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable.IsNullOrEmpty())
            {
                throw new NullReferenceException();
            }

            if (random == null)
            {
                random = new Random();
            }

            return enumerable.ElementAt(random.Next(enumerable.Count()));
        }
    }
}
