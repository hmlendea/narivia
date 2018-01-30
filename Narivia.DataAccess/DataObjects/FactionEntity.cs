using NuciXNA.DataAccess.DataObjects;

namespace Narivia.DataAccess.DataObjects
{
    /// <summary>
    /// Faction data entity.
    /// </summary>
    public class FactionEntity : EntityBase
    {
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
        /// Gets or sets the colour in hexadecimal.
        /// </summary>
        /// <value>The colour's hexadecimal value.</value>
        public string ColourHexadecimal { get; set; }

        /// <summary>
        /// Gets or sets the flag identifier.
        /// </summary>
        /// <value>The flag identifier.</value>
        public string FlagId { get; set; }

        /// <summary>
        /// Gets or sets the culture identifier.
        /// </summary>
        /// <value>The culture identifier.</value>
        public string CultureId { get; set; }
    }
}
