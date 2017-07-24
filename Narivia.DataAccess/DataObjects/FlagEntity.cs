namespace Narivia.DataAccess.DataObjects
{
    /// <summary>
    /// Faction flag.
    /// </summary>
    public class FlagEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the background.
        /// </summary>
        /// <value>The background.</value>
        public string Background { get; set; }

        /// <summary>
        /// Gets or sets the emblem.
        /// </summary>
        /// <value>The emblem.</value>
        public string Emblem { get; set; }

        /// <summary>
        /// Gets or sets the primary colour in hexadecimal.
        /// </summary>
        /// <value>The primary colour.</value>
        public string PrimaryColourHexadecimal { get; set; }

        /// <summary>
        /// Gets or sets the secondary colour in hexadecimal.
        /// </summary>
        /// <value>The secondary colour.</value>
        public string SecondaryColourHexadecimal { get; set; }
    }
}
