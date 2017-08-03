using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;
using Narivia.Settings;

namespace Narivia.Gui.WorldMap
{
    /// <summary>
    /// Map layer.
    /// </summary>
    public class Layer
    {
        /// <summary>
        /// Gets or sets the tile map.
        /// </summary>
        /// <value>The tile map.</value>
        public int[,] TileMap { get; set; }

        /// <summary>
        /// Gets or sets the tileset.
        /// </summary>
        /// <value>The tileset.</value>
        public string Tileset { get; set; }

        Sprite sprite { get; set; }

        List<Tile> tiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="Layer"/> class.
        /// </summary>
        public Layer()
        {
            tiles = new List<Tile>();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            int mapHeight = TileMap.GetLength(0);
            int mapWidth = TileMap.GetLength(1);

            sprite = new Sprite
            {
                ContentFile = $"World/Terrain/{Tileset}"
            };

            ConcurrentBag<Tile> layerTiles = new ConcurrentBag<Tile>();

            sprite.LoadContent();

            int cols = (int)(sprite.TextureSize.X / GameDefines.TILE_DIMENSIONS);

            Parallel.For(0, mapHeight, y => Parallel.For(0, mapWidth, x =>
            {
                if (TileMap[x, y] >= 0)
                {
                    int gid = TileMap[x, y];

                    Rectangle sourceRectangle = new Rectangle(
                        gid % cols * GameDefines.TILE_DIMENSIONS,
                        gid / cols * GameDefines.TILE_DIMENSIONS,
                        GameDefines.TILE_DIMENSIONS,
                        GameDefines.TILE_DIMENSIONS);

                    Tile tile = new Tile();
                    tile.LoadContent(x, y, sourceRectangle);

                    layerTiles.Add(tile);
                }
            }));

            tiles.AddRange(layerTiles);
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            tiles.Clear();
            sprite.UnloadContent();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        /// <param name="camera">Camera.</param>
        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Point camCoordsBegin = new Point(camera.Position.X / GameDefines.TILE_DIMENSIONS,
                                             camera.Position.Y / GameDefines.TILE_DIMENSIONS);
            Point camCoordsEnd = new Point(camCoordsBegin.X + camera.Size.X / GameDefines.TILE_DIMENSIONS,
                                           camCoordsBegin.Y + camera.Size.Y / GameDefines.TILE_DIMENSIONS);

            List<Tile> tilesToDraw = tiles.Where(tile => tile.X >= camCoordsBegin.X - 1 && tile.X <= camCoordsEnd.X + 1 &&
                                                         tile.Y >= camCoordsBegin.Y - 1 && tile.Y <= camCoordsEnd.Y + 1).ToList();

            foreach (Tile tile in tilesToDraw)
            {
                sprite.Position = new Point(tile.X * GameDefines.TILE_DIMENSIONS - camera.Position.X,
                                            tile.Y * GameDefines.TILE_DIMENSIONS - camera.Position.Y);
                sprite.SourceRectangle = tile.SourceRectangle;
                sprite.Draw(spriteBatch);
            }
        }
    }
}
