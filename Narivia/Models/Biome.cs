using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using System.Drawing;

namespace Narivia.Models
{
    public class Biome : EntityBase
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
    }
}
