
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Audio;
using Narivia.Helpers;
using Narivia.Input;
using Narivia.Screens;
using Narivia.Widgets;

namespace Narivia.Menus
{
    /// <summary>
    /// Menu manager.
    /// </summary>
    public class MenuManager
    {
        Menu menu;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.Menus.MenuManager"/> class.
        /// </summary>
        public MenuManager()
        {
            menu = new Menu();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="menuName">Menu name.</param>
        public void LoadContent(string menuName)
        {
            if (string.IsNullOrEmpty(menuName))
            {
                return;
            }

            LoadNewMenu(menuName);
            menu.Id = menuName;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            menu.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            menu.Update(gameTime);

            MenuLink selectedItem = menu.Items[menu.ItemNumber];

            if ((InputManager.Instance.IsMouseButtonPressed(MouseButton.LeftButton) && InputManager.Instance.IsCursorInArea(selectedItem.ScreenArea)) ||
                 InputManager.Instance.IsKeyPressed(Keys.Enter, Keys.E))
            {
                AudioManager.Instance.PlaySound("Interface/click");

                switch (selectedItem.LinkType)
                {
                    case "Screen":
                        ScreenManager.Instance.ChangeScreens(selectedItem.LinkId);
                        break;

                    case "Menu":
                        LoadNewMenu(selectedItem.LinkId);
                        break;

                    case "Action":
                        PerformAction(selectedItem.LinkId);
                        break;
                }
            }
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
        }

        void LoadNewMenu(string menuname)
        {
            menu.UnloadContent();

            switch (menuname)
            {
                case "SettingsMenu":
                    XmlManager<SettingsMenu> xmlManagerSettingsMenu = new XmlManager<SettingsMenu>();
                    menu = xmlManagerSettingsMenu.Load("Menus/" + menuname + ".xml");
                    break;

                default:
                    XmlManager<Menu> xmlManager = new XmlManager<Menu>();
                    menu = xmlManager.Load("Menus/" + menuname + ".xml");
                    break;
            }

            menu.LoadContent();
        }

        void PerformAction(string action)
        {
            switch (action)
            {
                case "Exit":
                    Program.Game.Exit();
                    break;
            }
        }
    }
}

