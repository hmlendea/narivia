using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Narivia.WorldMap
{
    public class Tile
    {
        public Vector2 Position { get; private set; }

        public Rectangle SourceRectangle { get; set; }

        public void LoadContent(Vector2 position, Rectangle sourceRectangle)
        {
            Position = position;
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
