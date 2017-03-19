using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;

namespace Narivia.WorldMap
{
    public class Layer
    {
        public TileMap TileMap { get; set; }

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

            Image.LoadContent();

            foreach (string row in TileMap.Rows)
            {
                string[] split = row.Split(']');

                position.X = -tileDimensions.X;
                position.Y += tileDimensions.Y;

                foreach (string item in split)
                {
                    if (item != string.Empty)
                    {
                        position.X += tileDimensions.X;

                        if (!item.Contains("x"))
                        {
                            string[] values = item.Replace("[", string.Empty).Split(':');

                            int val1 = int.Parse(values[0]);
                            int val2 = int.Parse(values[1]);

                            sourceRectangle = new Rectangle(
                                val1 * (int)tileDimensions.X, val2 * (int)tileDimensions.Y,
                                (int)tileDimensions.X, (int)tileDimensions.Y);

                            Tile tile = new Tile();
                            tile.LoadContent(position, sourceRectangle);

                            tiles.Add(tile);
                        }
                    }
                }
            }
        }

        public void UnloadContent()
        {
            foreach (Tile tile in tiles)
            {
                tile.UnloadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            Image.Update(gameTime);

            foreach (Tile tile in tiles)
            {
                tile.Update(gameTime);
            }
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
