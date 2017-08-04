using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Narivia.Common.Extensions
{
    /// <summary>
    /// Enumerable extensions.
    /// </summary>
    public static class EnumerationExtensions
    {
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

            if (displayAttribute != null)
            {
                return displayAttribute.GetName();
            }

            return value.ToString();
        }
    }
}
