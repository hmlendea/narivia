using System.Linq;

namespace Narivia.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Gets the duplicated elements.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <returns>The duplicated elements.</returns>
        public static string ToTitleCase(this string source)
        {
            return string.Join(string.Empty, source.Split(' ').Select(x => char.ToUpper(x[0]) + x.Substring(1)));
        }
    }
}
