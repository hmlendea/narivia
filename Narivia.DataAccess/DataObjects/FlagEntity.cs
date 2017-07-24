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
        /// Gets or sets the skin.
        /// </summary>
        /// <value>The skin.</value>
        public string Skin { get; set; }

        /// <summary>
        /// Gets or sets the background primary colour in hexadecimal.
        /// </summary>
        /// <value>The background primary colour in hexadecimal.</value>
        public string BackgroundPrimaryColourHexadecimal { get; set; }

        /// <summary>
        /// Gets or sets the background secondary colour in hexadecimal.
        /// </summary>
        /// <value>The background secondary colour in hexadecimal.</value>
        public string BackgroundSecondaryColourHexadecimal { get; set; }

        /// <summary>
        /// Gets or sets the emblem colour in hexadecimal.
        /// </summary>
        /// <value>The emblem colour in hexadecimal.</value>
        public string EmblemColourHexadecimal { get; set; }
    }
}
