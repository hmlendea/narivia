using Narivia.Infrastructure.Helpers;

namespace Narivia.DataAccess.DataObjects
{
    /// <summary>
    /// Biome data emtity.
    /// </summary>
    public class BiomeEmtity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        /// <value>The colour.</value>
        public Colour Colour { get; set; }
    }
}
