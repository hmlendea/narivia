using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using System.Drawing;

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
        public Color Colour { get; set; }

        /// <summary>
        /// Gets or sets the colour using the hexadecimal format.
        /// </summary>
        /// <value>The colour's hex value.</value>
        [XmlElement("Colour")]
        public string ColourHex
        {
            get { return ColorTranslator.ToHtml(Colour); }
            set { Colour = ColorTranslator.FromHtml(value); }
        }

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
        public RegionState State { get; set; }
    }
}
