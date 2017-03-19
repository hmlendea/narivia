using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Helpers;
using Narivia.Input;
using Narivia.Screens;


namespace Narivia.Menus
{
    public class MenuManager
    {
        Menu menu;
        bool transitioning;

        public MenuManager()
        {
            menu = new Menu();
            menu.OnMenuChange += menu_OnMenuChange;
            transitioning = false;
        }

        public void LoadContent(string menuPath)
        {
            if (menuPath != string.Empty)
            {
                menu.ID = menuPath;
            }
        }

        public void UnloadContent()
        {
            menu.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (!transitioning)
            {
                menu.Update(gameTime);
            }

            if (InputManager.Instance.KeyPressed(Keys.Enter) && !transitioning)
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

            Transition(gameTime);
        }

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
                    menu.ID = menu.Items[menu.ItemNumber].LinkId;
                }
                else if (first == 1.0f && last == 1.0f)
                {
                    transitioning = false;

                    foreach (MenuItem item in menu.Items)
                    {
                        item.Image.RestoreEffects();
                    }
                }
            }

        }

        void menu_OnMenuChange(object sender, EventArgs e)
        {
            if (menu.ID == null)
            {
                return;
            }

            XmlManager<Menu> xmlManager = new XmlManager<Menu>();

            menu.UnloadContent();
            menu = xmlManager.Load(menu.ID);
            menu.LoadContent();

            menu.OnMenuChange += menu_OnMenuChange;
            menu.Transition(0.0f);

            foreach (MenuItem item in menu.Items)
            {
                item.Image.StoreEffects();
                item.Image.ActivateEffect("FadeEffect");
            }
        }
    }
}

