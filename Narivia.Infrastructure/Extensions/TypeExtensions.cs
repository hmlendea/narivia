using System;

namespace Narivia.Infrastructure.Extensions
{
    /// <summary>
    /// Type extensions.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets the type name without namespace.
        /// </summary>
        /// <returns>The type name without namespace.</returns>
        /// <param name="source">Type.</param>
        public static string GetTypeNameWithoutNamespace(this Type source)
        {
            string typeName = source.ToString();
            typeName = typeName.Substring(typeName.LastIndexOf('.') + 1);

            return typeName;
        }
    }
}
