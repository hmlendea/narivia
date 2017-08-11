namespace Narivia.DataAccess.DataObjects
{
    /// <summary>
    /// World tile entity.
    /// </summary>
    public class WorldTileEntity
    {
        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>The region identifier.</value>
        public string RegionId { get; set; }

        /// <summary>
        /// Gets the biome identifier.
        /// </summary>
        /// <value>The biome identifier.</value>
        public string BiomeId { get; set; }
    }
}
