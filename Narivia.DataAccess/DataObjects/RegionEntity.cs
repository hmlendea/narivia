using Narivia.Infrastructure.Helpers;

namespace Narivia.DataAccess.DataObjects
{
    /// <summary>
    /// Region data entity.
    /// </summary>
    public class RegionEntity
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

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets the faction identifier.
        /// </summary>
        /// <value>The faction identifier.</value>
        public string FactionId { get; set; }

        /// <summary>
        /// Gets the sovereign faction identifier.
        /// </summary>
        /// <value>The sovereign faction identifier.</value>
        public string SovereignFactionId { get; set; }
    }
}
