using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Narivia.UI.Screens
{
    public class SplashScreen : Screen
    {
        Texture2D image;
        string path;

        public override void LoadContent()
        {
            base.LoadContent();

            path = "SplashScreen/SplashImage";
            image = content.Load<Texture2D>(path);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Draw(image, Vector2.Zero, Color.White);
        }
    }
}

