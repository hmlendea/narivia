namespace Narivia.DataAccess.DataObjects
{
    /// <summary>
    /// Border data entity.
    /// </summary>
    public class BorderEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets the first province identifier.
        /// </summary>
        /// <value>The first province identifier.</value>
        public string Province1Id { get; set; }

        /// <summary>
        /// Gets the second province identifier.
        /// </summary>
        /// <value>The second province identifier.</value>
        public string Province2Id { get; set; }
    }
}
