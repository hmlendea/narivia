using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;
using Narivia.Models;

namespace Narivia.Gui.WorldMap
{
    /// <summary>
    /// Map.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        /// <value>The layers.</value>
        public List<Layer> Layers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        public Map()
        {
            Layers = new List<Layer>();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="worldName">World name.</param>
        public void LoadContent(IEnumerable<WorldGeoLayer> worldGeoLayers)
        {
            foreach (WorldGeoLayer worldGeoLayer in worldGeoLayers)
            {
                Layer layer = new Layer
                {
                    Tileset = worldGeoLayer.Tileset,
                    TileMap = new string[worldGeoLayer.Width, worldGeoLayer.Height]
                };

                Parallel.For(0, worldGeoLayer.Height,
                             y => Parallel.For(0, worldGeoLayer.Width,
                                               x =>
                {
                    int tile = worldGeoLayer.Tiles[x, y];

                    if (tile > 0)
                    {
                        layer.TileMap[x, y] = tile.ToString();
                    }
                }));

                Layers.Add(layer);

                layer.LoadContent();
            }
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            Parallel.ForEach(Layers, l => l.UnloadContent());
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            Parallel.ForEach(Layers, l => l.Update(gameTime));
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        /// <param name="camera">Camera.</param>
        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            // The order in which the layers are drawn is very important
            Layers.ForEach(layer => layer.Draw(spriteBatch, camera));
        }
    }
}
