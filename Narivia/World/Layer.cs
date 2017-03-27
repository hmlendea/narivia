using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;
using Narivia.WorldMap.Entities;

namespace Narivia.WorldMap
{
    public class Layer
    {
        public string[,] TileMap { get; set; }

        public Image Image { get; set; }

        Vector2 tileDimensions;

        List<Tile> tiles;

        public Layer()
        {
            Image = new Image();

            tiles = new List<Tile>();
        }

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

        public void UnloadContent()
        {
            tiles.ForEach(tile => tile.UnloadContent());
        }

        public void Update(GameTime gameTime)
        {
            Image.Update(gameTime);

            tiles.ForEach(tile => tile.Update(gameTime));
        }

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
