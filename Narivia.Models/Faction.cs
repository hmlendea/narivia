using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

using Narivia.Infrastructure.Helpers;

namespace Narivia.Models
{
    public class Faction : EntityBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [MaxLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        /// <value>The colour.</value>
        [XmlIgnore]
        public Colour Colour { get; set; }

        /// <summary>
        /// Gets or sets the wealth.
        /// </summary>
        /// <value>The wealth.</value>
        [Range(0, int.MaxValue)]
        public int Wealth { get; set; }
    }
}
