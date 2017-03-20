using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Screens;
using Narivia.Input;

namespace Narivia.Menus
{
    public class Menu
    {
        private string id;
        
        public string ID
        {
            get { return id; }
            set
            {
                id = value;
                OnMenuChange(this, null);
            }
        }

        public string Axis { get; set; }

        public string Effects { get; set; }

        [XmlElement("Item")]
        public List<MenuItem> Items { get; set; }

        [XmlIgnore]
        public int ItemNumber { get; private set; }
        
        public event EventHandler OnMenuChange;
        
        public Menu()
        {
            id = string.Empty;
            ItemNumber = 0;
            Axis = "Y";
            Effects = string.Empty;
            Items = new List<MenuItem>();
        }
        
        public void LoadContent()
        {
            string[] split = Effects.Split(':');

            foreach (MenuItem item in Items)
            {
                item.Image.LoadContent();

                foreach (string s in split)
                {
                    item.Image.ActivateEffect(s);
                }
            }

            AlignMenuItems();
        }

        public void UnloadContent()
        {
            foreach (MenuItem item in Items)
            {
                item.Image.UnloadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            if ("Xx".Contains(Axis))
            {
                if (InputManager.Instance.KeyPressed(Keys.Right))
                {
                    ItemNumber++;
                }
                else if (InputManager.Instance.KeyPressed(Keys.Left))
                {
                    ItemNumber--;
                }
            }
            else if ("Yy".Contains(Axis))
            {
                if (InputManager.Instance.KeyPressed(Keys.Down))
                {
                    ItemNumber++;
                }
                else if (InputManager.Instance.KeyPressed(Keys.Up))
                {
                    ItemNumber--;
                }
            }

            if (ItemNumber < 0)
            {
                ItemNumber = 0;
            }
            else if (ItemNumber > Items.Count - 1)
            {
                ItemNumber = Items.Count - 1;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                if (i == ItemNumber)
                {
                    Items[i].Image.Active = true;
                }
                else
                {
                    Items[i].Image.Active = false;
                }

                Items[i].Image.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MenuItem item in Items)
            {
                item.Image.Draw(spriteBatch);
            }
        }

        public void Transition(float alpha)
        {
            foreach (MenuItem item in Items)
            {
                item.Image.Active = true;
                item.Image.Opacity = alpha;

                if (alpha == 0.0f)
                {
                    item.Image.FadeEffect.Increasing = true;
                }
                else
                {
                    item.Image.FadeEffect.Increasing = false;
                }
            }
        }

        void AlignMenuItems()
        {
            Vector2 dimensions = Vector2.Zero;

            foreach (MenuItem item in Items)
            {
                dimensions += new Vector2(item.Image.SourceRectangle.Width, item.Image.SourceRectangle.Height);
            }

            dimensions = new Vector2(
                (ScreenManager.Instance.Dimensions.X - dimensions.X) / 2,
                (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2);

            foreach (MenuItem item in Items)
            {
                if (Axis == "X" || Axis == "x")
                {
                    item.Image.Position = new Vector2(
                        dimensions.X,
                        (ScreenManager.Instance.Dimensions.Y - item.Image.SourceRectangle.Height) / 2);
                }
                else if (Axis == "Y" || Axis == "y")
                {
                    item.Image.Position = new Vector2(
                        (ScreenManager.Instance.Dimensions.X - item.Image.SourceRectangle.Width) / 2,
                        dimensions.Y);
                }

                dimensions += new Vector2(item.Image.SourceRectangle.Width, item.Image.SourceRectangle.Height);
            }
        }
    }
}

