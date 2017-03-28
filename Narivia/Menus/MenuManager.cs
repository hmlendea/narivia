using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Helpers;
using Narivia.Input;
using Narivia.Screens;


namespace Narivia.Menus
{
    /// <summary>
    /// Menu manager.
    /// </summary>
    public class MenuManager
    {
        Menu menu;
        bool transitioning;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.Menus.MenuManager"/> class.
        /// </summary>
        public MenuManager()
        {
            menu = new Menu();
            menu.OnMenuChange += menu_OnMenuChange;
            transitioning = false;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="menuName">Menu name.</param>
        public void LoadContent(string menuName)
        {
            if (menuName != string.Empty)
            {
                menu.Id = menuName;
            }
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
            if (transitioning)
            {
                Transition(gameTime);
                return;
            }

            menu.Update(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.Enter))
            {
                MenuItem selectedMenuItem = menu.Items[menu.ItemNumber];

                if (selectedMenuItem.LinkType == "Screen")
                {
                    ScreenManager.Instance.ChangeScreens(selectedMenuItem.LinkId);
                }
                else if (selectedMenuItem.LinkType == "Menu")
                {
                    transitioning = true;
                    menu.Transition(1.0f);

                    foreach (MenuItem item in menu.Items)
                    {
                        item.Image.StoreEffects();
                        item.Image.ActivateEffect("FadeEffect");
                    }
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

        void Transition(GameTime gameTime)
        {
            if (!transitioning)
            {
                return;
            }

            int oldMenuCount = menu.Items.Count;

            for (int i = 0; i < oldMenuCount; i++)
            {
                menu.Items[i].Image.Update(gameTime);

                float first = menu.Items[0].Image.Opacity;
                float last = menu.Items[menu.Items.Count - 1].Image.Opacity;

                if (first == 0.0f && last == 0.0f)
                {
                    menu.Id = menu.Items[menu.ItemNumber].LinkId;
                }
                else if (first == 1.0f && last == 1.0f)
                {
                    transitioning = false;

                    menu.Items.ForEach(item => item.Image.RestoreEffects());
                }
            }

        }

        void menu_OnMenuChange(object sender, EventArgs e)
        {
            if (menu.Id == null)
            {
                return;
            }

            LoadNewMenu();

            menu.OnMenuChange += menu_OnMenuChange;
            menu.Transition(0.0f);

            foreach (MenuItem item in menu.Items)
            {
                item.Image.StoreEffects();
                item.Image.ActivateEffect("FadeEffect");
            }
        }

        void LoadNewMenu()
        {
            menu.UnloadContent();

            switch (menu.Id)
            {
                case "SettingsMenu":
                    XmlManager<SettingsMenu> xmlManagerSettingsMenu = new XmlManager<SettingsMenu>();
                    menu = xmlManagerSettingsMenu.Load("Menus/" + menu.Id + ".xml");
                    break;

                default:
                    XmlManager<Menu> xmlManager = new XmlManager<Menu>();
                    menu = xmlManager.Load("Menus/" + menu.Id + ".xml");
                    break;
            }

            menu.LoadContent();
        }
    }
}

