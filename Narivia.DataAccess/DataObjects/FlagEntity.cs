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
        /// Gets or sets the first layer.
        /// </summary>
        /// <value>The first layer.</value>
        public string Layer1 { get; set; }

        /// <summary>
        /// Gets or sets the second layer.
        /// </summary>
        /// <value>The second layer.</value>
        public string Layer2 { get; set; }

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
        /// Gets or sets the background colour in hexadecimal.
        /// </summary>
        /// <value>The background colour in hexadecimal.</value>
        public string BackgroundColourHexadecimal { get; set; }

        /// <summary>
        /// Gets or sets the first layer's colour in hexadecimal.
        /// </summary>
        /// <value>The first layer's colour in hexadecimal.</value>
        public string Layer1ColourHexadecimal { get; set; }

        /// <summary>
        /// Gets or sets the second layer's colour in hexadecimal.
        /// </summary>
        /// <value>The second layer's colour in hexadecimal.</value>
        public string Layer2ColourHexadecimal { get; set; }

        /// <summary>
        /// Gets or sets the emblem colour in hexadecimal.
        /// </summary>
        /// <value>The emblem colour in hexadecimal.</value>
        public string EmblemColourHexadecimal { get; set; }
    }
}
