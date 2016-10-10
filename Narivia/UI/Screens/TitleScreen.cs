using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.UI.Menus;

namespace Narivia.UI.Screens
{
    public class TitleScreen : Screen
    {
        readonly MenuManager menuManager;
        
        public TitleScreen()
        {
            menuManager = new MenuManager();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            menuManager.LoadContent("UI/Menus/TitleMenu.xml");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            menuManager.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            menuManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            menuManager.Draw(spriteBatch);
        }
    }
}
