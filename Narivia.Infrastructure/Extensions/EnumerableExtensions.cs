using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Narivia.Infrastructure.Extensions
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

            ICollection<T> collection = enumerable as ICollection<T>;

            if (collection != null)
            {
                return collection.Count < 1;
            }

            return !enumerable.Any();
        }

        /// <summary>
        /// Gets a random element.
        /// </summary>
        /// <returns>The element.</returns>
        /// <param name="enumerable">Enumerable.</param>
        public static T RandomElement<T>(this IEnumerable<T> enumerable)
        {
            int count = enumerable.Count();

            if (count == 0)
            {
                return default(T);
            }

            if (random == null)
            {
                random = new Random();
            }

            return enumerable.ElementAt(random.Next(count));
        }

        /// <summary>
        /// Gets the display name of the enumeration item.
        /// </summary>
        /// <returns>The display name string.</returns>
        /// <param name="value">Enumeration item.</param>
        public static string GetDisplayName(this Enum value)
        {
            DisplayAttribute displayAttribute = value.GetType()
                                                     .GetMember(value.ToString())
                                                     .FirstOrDefault()
                                                     .GetCustomAttribute<DisplayAttribute>();

            if(displayAttribute != null)
            {
                return displayAttribute.GetName();
            }

            return value.ToString();
        }
    }
}
