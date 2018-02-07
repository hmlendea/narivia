using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Narivia.Models
{
    /// <summary>
    /// World domain model.
    /// </summary>
    public class World : IEquatable<World>
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

        /// <summary>
        /// Determines whether the specified <see cref="World"/> is equal to the current <see cref="World"/>.
        /// </summary>
        /// <param name="other">The <see cref="World"/> to compare with the current <see cref="World"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="World"/> is equal to the current
        /// <see cref="World"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(World other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Id, other.Id) &&
                   string.Equals(Name, other.Name) &&
                   string.Equals(Description, other.Description) &&
                   string.Equals(Author, other.Author) &&
                   string.Equals(ResourcePack, other.ResourcePack) &&
                   string.Equals(Version, other.Version) &&
                   Equals(Width, other.Width) &&
                   Equals(Height, other.Height) &&
                   Equals(BaseProvinceIncome, other.BaseProvinceIncome) &&
                   Equals(BaseProvinceRecruitment, other.BaseProvinceRecruitment) &&
                   Equals(BaseFactionRecruitment, other.BaseFactionRecruitment) &&
                   Equals(MinTroopsPerAttack, other.MinTroopsPerAttack) &&
                   Equals(HoldingSlotsPerFaction, other.HoldingSlotsPerFaction) &&
                   Equals(StartingWealth, other.StartingWealth) &&
                   Equals(StartingTroops, other.StartingTroops) &&
                   Equals(HoldingsPrice, other.HoldingsPrice) &&
                   Equals(Tiles, other.Tiles);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="World"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="World"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="World"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((World)obj);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="World"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Id != null ? Id.GetHashCode() : 0) * 397) ^
                       (Name != null ? Name.GetHashCode() : 0) ^
                       (Description != null ? Description.GetHashCode() : 0) ^
                       (Author != null ? Author.GetHashCode() : 0) ^
                       (ResourcePack != null ? ResourcePack.GetHashCode() : 0) ^
                       (Version != null ? Version.GetHashCode() : 0) ^
                       Width.GetHashCode() ^
                       Height.GetHashCode() ^
                       BaseProvinceIncome.GetHashCode() ^
                       BaseProvinceRecruitment.GetHashCode() ^
                       BaseFactionRecruitment.GetHashCode() ^
                       MinTroopsPerAttack.GetHashCode() ^
                       HoldingSlotsPerFaction.GetHashCode() ^
                       StartingWealth.GetHashCode() ^
                       StartingTroops.GetHashCode() ^
                       HoldingsPrice.GetHashCode() ^
                       (Tiles != null ? Tiles.GetHashCode() : 0);
            }
        }
    }
}
