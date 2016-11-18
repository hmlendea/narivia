using System.Collections.Generic;
using System.Linq;

namespace Narivia.Utils.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                return true;

            ICollection<T> collection = enumerable as ICollection<T>;

            if (collection != null)
                return collection.Count < 1;

            return !enumerable.Any();
        }
    }
}
