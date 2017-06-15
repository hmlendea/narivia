using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    /// <summary>
    /// World domain model.
    /// </summary>
    public class World
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [StringLength(20, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [StringLength(300, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string Description { get; set; }

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
        /// Gets or sets the base region income.
        /// </summary>
        /// <value>The base region income.</value>
        [Range(0, int.MaxValue)]
        public int BaseRegionIncome { get; set; }

        /// <summary>
        /// Gets or sets the base region recruitment.
        /// </summary>
        /// <value>The base region recruitment.</value>
        [Range(0, int.MaxValue)]
        public int BaseRegionRecruitment { get; set; }

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
    }
}
