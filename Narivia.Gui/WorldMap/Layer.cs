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
        public string[,] TileMap { get; set; }

        /// <summary>
        /// Gets or sets the tileset.
        /// </summary>
        /// <value>The tileset.</value>
        public string Tileset { get; set; }

        Sprite sprite { get; set; }

        ConcurrentBag<Tile> tiles;

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="tileDimensions">Tile dimensions.</param>
        public void LoadContent()
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, 0, 0);

            int mapHeight = TileMap.GetLength(0);
            int mapWidth = TileMap.GetLength(1);

            sprite = new Sprite
            {
                ContentFile = $"World/Terrain/{Tileset}"
            };

            tiles = new ConcurrentBag<Tile>();

            sprite.LoadContent();

            Parallel.For(0, mapHeight,
                         y => Parallel.For(0, mapWidth,
                                           x =>
            {
                if (!string.IsNullOrEmpty(TileMap[x, y]))
                {
                    int gid = int.Parse(TileMap[x, y]);
                    int cols = (int)(sprite.TextureSize.X / GameDefines.TILE_DIMENSIONS);

                    sourceRectangle = new Rectangle(
                        gid % cols * GameDefines.TILE_DIMENSIONS,
                        gid / cols * GameDefines.TILE_DIMENSIONS,
                        GameDefines.TILE_DIMENSIONS,
                        GameDefines.TILE_DIMENSIONS);

                    Tile tile = new Tile();
                    tile.LoadContent(x, y, sourceRectangle);

                    tiles.Add(tile);
                }
            }));
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            tiles = null;
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
            Vector2 camCoordsBegin = camera.Position / GameDefines.TILE_DIMENSIONS;
            Vector2 camCoordsEnd = camCoordsBegin + camera.Size / GameDefines.TILE_DIMENSIONS;

            List<Tile> tilesToDraw = tiles.Where(tile => tile.X >= camCoordsBegin.X - 1 && tile.X <= camCoordsEnd.X + 1 &&
                                                         tile.Y >= camCoordsBegin.Y - 1 && tile.Y <= camCoordsEnd.Y + 1).ToList();

            foreach (Tile tile in tilesToDraw)
            {
                sprite.Position = new Vector2((tile.X - camCoordsBegin.X) * GameDefines.TILE_DIMENSIONS,
                                              (tile.Y - camCoordsBegin.Y) * GameDefines.TILE_DIMENSIONS);
                sprite.SourceRectangle = tile.SourceRectangle;
                sprite.Draw(spriteBatch);
            }
        }
    }
}
