using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TiledSharp;

using Narivia.Graphics;
using Narivia.Infrastructure.Helpers;
using Narivia.WorldMap.Entities;

namespace Narivia.WorldMap
{
    public class Map
    {
        TmxMap tmxMap;

        public List<Layer> Layers { get; set; }

        public Vector2 TileDimensions { get; set; }

        public Map()
        {
            Layers = new List<Layer>();
            TileDimensions = Vector2.Zero;
        }

        public void LoadContent(string worldName)
        {
            tmxMap = new TmxMap(Path.Combine(ApplicationPaths.WorldsDirectory, worldName, "world.tmx"));

            foreach (TmxLayer tmxLayer in tmxMap.Layers)
            {
                string tilesetName = tmxLayer.Properties["tileset"];
                Image layerImage = new Image
                {
                    ImagePath = "World/Terrain/" + tilesetName
                };
                Layer layer = new Layer
                {
                    Image = layerImage,
                    TileMap = new string[tmxMap.Width, tmxMap.Height]
                };

                TmxTileset tmxTileset = tmxMap.Tilesets[tilesetName];

                foreach (TmxLayerTile tile in tmxLayer.Tiles.Where(x => x.Gid > 0))
                {
                    int index = tile.Gid - tmxTileset.FirstGid;

                    layer.TileMap[tile.X, tile.Y] = index.ToString();
                }

                Layers.Add(layer);

                layer.LoadContent(TileDimensions);
            }
        }

        public void UnloadContent()
        {
            Layers.ForEach(layer => layer.UnloadContent());
        }

        public void Update(GameTime gameTime)
        {
            Layers.ForEach(layer => layer.Update(gameTime));
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Layers.ForEach(layer => layer.Draw(spriteBatch, camera));
        }
    }
}
