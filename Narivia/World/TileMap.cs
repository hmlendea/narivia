using System.Collections.Generic;
using System.Xml.Serialization;

namespace Narivia.WorldMap
{
    public class TileMap
    {
        [XmlElement("Row")]
        public List<string> Rows { get; set; }

        public TileMap()
        {
            Rows = new List<string>();
        }
    }
}
