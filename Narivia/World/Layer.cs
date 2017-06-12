using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;
using Narivia.Interface;

namespace Narivia.WorldMap
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
        public Image Image { get; set; }

        Vector2 tileDimensions;

        readonly List<Tile> tiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.WorldMap.Layer"/> class.
        /// </summary>
        public Layer()
        {
            Image = new Image();

            tiles = new List<Tile>();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="tileDimensions">Tile dimensions.</param>
        public void LoadContent(Vector2 tileDimensions)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, 0, 0);
            this.tileDimensions = tileDimensions;

            int mapSize = TileMap.GetLength(0);

            Image.LoadContent();

            for (int y = 0; y < mapSize; y++)
            {
                for (int x = 0; x < mapSize; x++)
                {
                    if (string.IsNullOrEmpty(TileMap[x, y]))
                    {
                        continue;
                    }

                    int gid = int.Parse(TileMap[x, y]);
                    int cols = Image.Texture.Width / (int)tileDimensions.X;
                    int srX = gid % cols;
                    int srY = gid / cols;

                    sourceRectangle = new Rectangle(
                        srX * (int)tileDimensions.X,
                        srY * (int)tileDimensions.Y,
                        (int)tileDimensions.X,
                        (int)tileDimensions.Y);

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
            tiles.ForEach(tile => tile.UnloadContent());
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            Image.Update(gameTime);

            tiles.ForEach(tile => tile.Update(gameTime));
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        /// <param name="camera">Camera.</param>
        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Vector2 camCoordsBegin = camera.Position / tileDimensions;
            Vector2 camCoordsEnd = camCoordsBegin + camera.Size / tileDimensions;

            List<Tile> tileList = tiles.Where(tile => tile.X >= camCoordsBegin.X - 1 &&
                                                      tile.Y >= camCoordsBegin.Y - 1 &&
                                                      tile.X <= camCoordsEnd.X + 1 &&
                                                      tile.Y <= camCoordsEnd.Y + 1).ToList();

            foreach (Tile tile in tileList)
            {
                Image.Position = new Vector2(tile.X - camCoordsBegin.X, tile.Y - camCoordsBegin.Y) * tileDimensions;
                Image.SourceRectangle = tile.SourceRectangle;
                Image.Draw(spriteBatch);
            }
        }
    }
}
