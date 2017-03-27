using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Narivia.WorldMap
{
    public class Tile
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Rectangle SourceRectangle { get; set; }

        public void LoadContent(int x, int y, Rectangle sourceRectangle)
        {
            X = x;
            Y = y;
            SourceRectangle = sourceRectangle;
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
