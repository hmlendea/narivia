namespace Narivia.DataAccess.DataObjects
{
    /// <summary>
    /// Culture data entity.
    /// </summary>
    public class CultureEntity
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
        /// Gets or sets the texture set.
        /// </summary>
        /// <value>The texture set.</value>
        public string TextureSet { get; set; }
    }
}
