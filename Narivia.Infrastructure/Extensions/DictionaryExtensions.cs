using System.Collections.Generic;

namespace Narivia.Infrastructure.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Registers the specified key-element pair into the source dictionary.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="key">Key.</param>
        /// <param name="element">Element.</param>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TElement">The element type.</typeparam>
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
