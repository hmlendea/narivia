namespace Narivia.DataAccess.DataObjects
{
    /// <summary>
    /// Relation data entity.
    /// </summary>
    public class RelationEntity
    {
        /// <summary>
        /// Gets the source faction identifier.
        /// </summary>
        /// <value>The source faction identifier.</value>
        public string SourceFactionId { get; set; }

        /// <summary>
        /// Gets the target region identifier.
        /// </summary>
        /// <value>The target region identifier.</value>
        public string TargetFactionId { get; set; }

        /// <summary>
        /// Gets or sets the relation value.
        /// </summary>
        /// <value>The relation value.</value>
        public int Value { get; set; }
    }
}
