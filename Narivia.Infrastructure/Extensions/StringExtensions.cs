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
            return char.ToUpper(source[0]) + source.Substring(1);
        }
    }
}
