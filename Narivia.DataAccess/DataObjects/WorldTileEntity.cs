namespace Narivia.DataAccess.DataObjects
{
    /// <summary>
    /// World tile entity.
    /// </summary>
    public class WorldTileEntity
    {
        /// <summary>
        /// Gets or sets the province identifier.
        /// </summary>
        /// <value>The province identifier.</value>
        public string ProvinceId { get; set; }

        /// <summary>
        /// Gets the biome identifier.
        /// </summary>
        /// <value>The biome identifier.</value>
        public string BiomeId { get; set; }
        
        public bool HasRiver { get; set; }
    }
}
