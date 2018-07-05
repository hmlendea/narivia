using System.ComponentModel.DataAnnotations;

using Narivia.Models.Enumerations;

namespace Narivia.Models
{
    /// <summary>
    /// Resource domain model.
    /// </summary>
    public sealed class Resource : ModelBase
    {
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public ResourceType Type { get; set; }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>The output.</value>
        [Range(0, 100)]
        public int Output { get; set; }
    }
}
