using System.Collections.Generic;
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
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public Sprite Sprite { get; set; }

        readonly List<Tile> tiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.WorldMap.Layer"/> class.
        /// </summary>
        public Layer()
        {
            Sprite = new Sprite();

            tiles = new List<Tile>();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="tileDimensions">Tile dimensions.</param>
        public void LoadContent()
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, 0, 0);

            int mapSize = TileMap.GetLength(0);

            Sprite.LoadContent();

            for (int y = 0; y < mapSize; y++)
            {
                for (int x = 0; x < mapSize; x++)
                {
                    if (string.IsNullOrEmpty(TileMap[x, y]))
                    {
                        continue;
                    }

                    int gid = int.Parse(TileMap[x, y]);
                    int cols = (int)(Sprite.TextureSize.X / GameDefines.TILE_DIMENSIONS);
                    int srX = gid % cols;
                    int srY = gid / cols;

                    sourceRectangle = new Rectangle(
                        srX * GameDefines.TILE_DIMENSIONS,
                        srY * GameDefines.TILE_DIMENSIONS,
                        GameDefines.TILE_DIMENSIONS,
                        GameDefines.TILE_DIMENSIONS);

                    Tile tile = new Tile();
                    tile.LoadContent(x, y, sourceRectangle);

                    tiles.Add(tile);
                }
            }
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
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
                Sprite.Position = new Vector2((tile.X - camCoordsBegin.X) * GameDefines.TILE_DIMENSIONS,
                                              (tile.Y - camCoordsBegin.Y) * GameDefines.TILE_DIMENSIONS);
                Sprite.SourceRectangle = tile.SourceRectangle;
                Sprite.Draw(spriteBatch);
            }
        }
    }
}
