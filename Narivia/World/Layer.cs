using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;

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
                        string[] values = TileMap[x, y].Split(':');

                        int value1 = int.Parse(values[0]);
                        int value2 = int.Parse(values[1]);

                        sourceRectangle = new Rectangle(
                            value1 * (int)tileDimensions.X,
                            value2 * (int)tileDimensions.Y,
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

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in tiles)
            {
                Image.Position = tile.Position;
                Image.SourceRectangle = tile.SourceRectangle;
                Image.Draw(spriteBatch);
            }
        }
    }
}
