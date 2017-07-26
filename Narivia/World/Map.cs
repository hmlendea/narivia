using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TiledSharp;

using Narivia.Graphics;
using Narivia.Infrastructure.Helpers;
using Narivia.Gui;

namespace Narivia.WorldMap
{
    /// <summary>
    /// Map.
    /// </summary>
    public class Map
    {
        TmxMap tmxMap;

        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        /// <value>The layers.</value>
        public List<Layer> Layers { get; set; }

        /// <summary>
        /// Gets or sets the tile dimensions.
        /// </summary>
        /// <value>The tile dimensions.</value>
        public Vector2 TileDimensions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.WorldMap.Map"/> class.
        /// </summary>
        public Map()
        {
            Layers = new List<Layer>();
            TileDimensions = Vector2.Zero;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="worldName">World name.</param>
        public void LoadContent(string worldName)
        {
            tmxMap = new TmxMap(Path.Combine(ApplicationPaths.WorldsDirectory, worldName, "world.tmx"));

            // TODO: Consider parallelisation
            foreach (TmxLayer tmxLayer in tmxMap.Layers)
            {
                // Check if the tileset layer property exists
                if (!tmxLayer.Properties.ContainsKey("tileset"))
                {
                    // TODO: Log warning
                    continue;
                }

                string tilesetName = tmxLayer.Properties["tileset"];

                // Check if the tileset with the specified name exists among the map tilesets
                if (tmxMap.Tilesets.All(x => x.Name != tilesetName))
                {
                    // TODO: Log warning
                    continue;
                }

                Sprite layerSprite = new Sprite
                {
                    ContentFile = "World/Terrain/" + tilesetName
                };
                Layer layer = new Layer
                {
                    Sprite = layerSprite,
                    TileMap = new string[tmxMap.Width, tmxMap.Height]
                };

                TmxTileset tmxTileset = tmxMap.Tilesets[tilesetName];

                Parallel.ForEach(tmxLayer.Tiles.Where(tile => tile.Gid > 0),
                                 tile => layer.TileMap[tile.X, tile.Y] = (tile.Gid - tmxTileset.FirstGid).ToString());

                Layers.Add(layer);

                layer.LoadContent(TileDimensions);
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
