using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

using Narivia.Infrastructure.Helpers;

namespace Narivia.Models
{
    public enum RegionType
    {
        /// <summary>
        /// Province.
        /// </summary>L
        Province = 0,

        /// <summary>
        /// Capital.
        /// </summary>
        Capital = 1,
    }

    public enum RegionState
    {
        /// <summary>
        /// Sovereign.
        /// </summary>
        Sovereign = 0,

        /// <summary>
        /// Occupied.
        /// </summary>
        Occupied = 1
    }

    public class Region : EntityBase
    {
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
        /// Gets or sets the colour.
        /// </summary>
        /// <value>The colour.</value>
        [XmlIgnore]
        public Colour Colour { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [XmlIgnore]
        public RegionType Type { get; set; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        [XmlIgnore]
        public RegionState State
        {
            get
            {
                if (FactionId == SovereignFactionId)
                    return RegionState.Sovereign;

                return RegionState.Occupied;
            }
        }

        /// <summary>
        /// Gets the faction identifier.
        /// </summary>
        /// <value>The faction identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string FactionId { get; set; }

        /// <summary>
        /// Gets the sovereign faction identifier.
        /// </summary>
        /// <value>The sovereign faction identifier.</value>
        [StringLength(40, ErrorMessage = "The {0} must be between {1} and {2} characters long", MinimumLength = 3)]
        public string SovereignFactionId { get; set; }
    }
}
