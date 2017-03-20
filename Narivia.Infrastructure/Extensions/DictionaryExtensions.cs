using System.Collections.Generic;

namespace Narivia.Infrastructure.Extensions
{
    public static class DictionaryExtensions
    {
        public static void Register<TKey, TElement>(this IDictionary<TKey, TElement> source, TKey key, TElement element)
        {
            if (source.ContainsKey(key))
            {
                source[key] = element;
            }
            else
            {
                source.Add(key, element);
            }
        }
    }
}
