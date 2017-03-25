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

        List<Tile> tiles;

        public Layer()
        {
            Image = new Image();

            tiles = new List<Tile>();
        }

        public void LoadContent(Vector2 tileDimensions)
        {
            Vector2 position = -tileDimensions;
            Rectangle sourceRectangle = new Rectangle(0, 0, 0, 0);

            int mapSize = TileMap.GetLength(0);

            Image.LoadContent();

            for (int y = 0; y < mapSize; y++)
            {
                position.X = -tileDimensions.X;
                position.Y += tileDimensions.Y;

                for (int x = 0; x < mapSize; x++)
                {
                    position.X += tileDimensions.X;

                    if (TileMap[x, y] != null)
                    {
                        int gid = int.Parse(TileMap[x, y]);
                        int srX = gid % (Image.Texture.Width / (int)tileDimensions.X);
                        int srY = gid / (Image.Texture.Width / (int)tileDimensions.X);

                        sourceRectangle = new Rectangle(
                            srX * (int)tileDimensions.X,
                            srY * (int)tileDimensions.Y,
                            (int)tileDimensions.X,
                            (int)tileDimensions.Y);

                        Tile tile = new Tile();
                        tile.LoadContent(position, sourceRectangle);

                        tiles.Add(tile);
                    }
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
            Vector2 cameraStart = camera.Position - Vector2.One * 32;
            Vector2 cameraEnd = camera.Position + camera.Size;

            List<Tile> tileList = tiles.Where(tile => tile.Position.X >= cameraStart.X &&
                                                      tile.Position.Y >= cameraStart.Y &&
                                                      tile.Position.X <= cameraEnd.X &&
                                                      tile.Position.Y <= cameraEnd.Y).ToList();

            foreach (Tile tile in tileList)
            {
                Image.Position = tile.Position - camera.Position;
                Image.SourceRectangle = tile.SourceRectangle;
                Image.Draw(spriteBatch);
            }
        }
    }
}
