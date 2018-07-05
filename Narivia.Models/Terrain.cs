using System.Drawing;
using System.Xml.Serialization;

namespace Narivia.Models
{
    /// <summary>
    /// Terrain domain model.
    /// </summary>
    public sealed class Terrain : ModelBase
    {
        public string Spritesheet { get; set; }

        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        /// <value>The colour.</value>
        [XmlIgnore]
        public Color Colour { get; set; }

        public int ZIndex { get; set; }
    }
}
