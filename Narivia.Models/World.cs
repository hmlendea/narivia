using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Narivia.Models
{
    /// <summary>
    /// World domain model.
    /// </summary>
    public class World : ModelBase
    {
        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>The author.</value>
        [StringLength(20, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the resource pack.
        /// </summary>
        /// <value>The resource pack.</value>
        [StringLength(20, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string ResourcePack { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        [StringLength(10, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 1)]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the base province income.
        /// </summary>
        /// <value>The base province income.</value>
        [Range(0, int.MaxValue)]
        public int BaseProvinceIncome { get; set; }

        /// <summary>
        /// Gets or sets the base province recruitment.
        /// </summary>
        /// <value>The base province recruitment.</value>
        [Range(0, int.MaxValue)]
        public int BaseProvinceRecruitment { get; set; }

        /// <summary>
        /// Gets or sets the base faction recruitment.
        /// </summary>
        /// <value>The base faction recruitment.</value>
        [Range(0, int.MaxValue)]
        public int BaseFactionRecruitment { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of troops required to attack.
        /// </summary>
        /// <value>The minimum number of troops to attack.</value>
        [Range(0, int.MaxValue)]
        public int MinTroopsPerAttack { get; set; }

        /// <summary>
        /// Gets or sets the number of holding slots per faction.
        /// </summary>
        /// <value>The number of holding slots per faction.</value>
        [Range(0, int.MaxValue)]
        public int HoldingSlotsPerFaction { get; set; }

        /// <summary>
        /// Gets or sets the starting wealth.
        /// </summary>
        /// <value>The starting wealth.</value>
        [Range(0, int.MaxValue)]
        public int StartingWealth { get; set; }

        /// <summary>
        /// Gets or sets the starting troops.
        /// </summary>
        /// <value>The starting troops.</value>
        [Range(0, int.MaxValue)]
        public int StartingTroops { get; set; }

        /// <summary>
        /// Gets or sets the price of holdings.
        /// </summary>
        /// <value>The holdings price.</value>
        [Range(0, int.MaxValue)]
        public int HoldingsPrice { get; set; }

        /// <summary>
        /// Gets or sets the tiles.
        /// </summary>
        /// <value>The tiles.</value>
        [XmlIgnore]
        public WorldTile[,] Tiles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="World"/> class.
        /// </summary>
        public World()
        {
            
        }
    }
}
