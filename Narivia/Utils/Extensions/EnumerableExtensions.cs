using System.Collections.Generic;
using System.Linq;

namespace Narivia.Utils.Extensions
{
    public static class EnumerableExtensions
    {
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
    }
}
