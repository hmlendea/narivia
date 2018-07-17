using System.Collections.Generic;

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
        /// Gets the terrain identifier.
        /// </summary>
        /// <value>The terrain identifier.</value>
        public string TerrainId { get; set; }

        public List<string> TerrainIds { get; set; }

        public byte Altitude { get; set; }

        public bool HasRiver { get; set; }

        public bool HasWater { get; set; }

        public WorldTileEntity()
        {
            TerrainIds = new List<string>();
        }
    }
}
