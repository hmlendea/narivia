using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;

namespace Narivia.WorldMap
{
    public class Tile
    {
        public Image Image { get; private set; }

        public Vector2 Position { get; private set; }

        public string EntityId { get; private set; }

        public void LoadContent(Image image, Vector2 position, string entityId)
        {
            Image = image;
            Position = position;
            EntityId = entityId;

            Image.LoadContent();
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}
