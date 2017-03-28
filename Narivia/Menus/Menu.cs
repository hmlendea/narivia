using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Narivia.Audio;
using Narivia.Screens;
using Narivia.Input;

namespace Narivia.Menus
{
    /// <summary>
    /// Menu.
    /// </summary>
    public class Menu
    {
        string id;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnMenuChange(this, null);
            }
        }

        /// <summary>
        /// Gets or sets the axis.
        /// </summary>
        /// <value>The axis.</value>
        public string Axis { get; set; }

        /// <summary>
        /// Gets or sets the spacing.
        /// </summary>
        /// <value>The spacing.</value>
        public int Spacing { get; set; }

        /// <summary>
        /// Gets or sets the effects.
        /// </summary>
        /// <value>The effects.</value>
        public string Effects { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        [XmlElement("Item")]
        public List<MenuItem> Items { get; set; }

        /// <summary>
        /// Gets the item number.
        /// </summary>
        /// <value>The item number.</value>
        [XmlIgnore]
        public int ItemNumber { get; private set; }

        /// <summary>
        /// Occurs when the Id changes.
        /// </summary>
        public event EventHandler OnMenuChange;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.Menus.Menu"/> class.
        /// </summary>
        public Menu()
        {
            id = string.Empty;
            ItemNumber = 0;
            Axis = "Y";
            Spacing = 30;
            Effects = string.Empty;
            Items = new List<MenuItem>();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public virtual void LoadContent()
        {
            List<string> split = Effects.Split(':').ToList();

            foreach (MenuItem item in Items)
            {
                item.Image.LoadContent();

                split.ForEach(item.Image.ActivateEffect);
            }

            AlignMenuItems();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            Items.ForEach(item => item.Image.UnloadContent());
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            int newSelectedItemIndex = ItemNumber;

            if ("Xx".Contains(Axis))
            {
                if (InputManager.Instance.IsKeyPressed(Keys.Right, Keys.D))
                {
                    newSelectedItemIndex = ItemNumber + 1;
                }
                else if (InputManager.Instance.IsKeyPressed(Keys.Left, Keys.A))
                {
                    newSelectedItemIndex = ItemNumber - 1;
                }
            }
            else if ("Yy".Contains(Axis))
            {
                if (InputManager.Instance.IsKeyPressed(Keys.Down, Keys.S))
                {
                    newSelectedItemIndex = ItemNumber + 1;
                }
                else if (InputManager.Instance.IsKeyPressed(Keys.Up, Keys.W))
                {
                    newSelectedItemIndex = ItemNumber - 1;
                }
            }

            for (int i = 0; i < Items.Count; i++)
            {
                if (InputManager.Instance.IsCursorInArea(Items[i].Image.ScreenArea))
                {
                    newSelectedItemIndex = i;
                }
            }

            if (newSelectedItemIndex < 0)
            {
                newSelectedItemIndex = 0;
            }
            else if (newSelectedItemIndex > Items.Count - 1)
            {
                newSelectedItemIndex = Items.Count - 1;
            }

            if (newSelectedItemIndex != ItemNumber)
            {
                ItemNumber = newSelectedItemIndex;

                AudioManager.Instance.PlaySound("Interface/select");
            }

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Image.Active = (i == ItemNumber);
                Items[i].Image.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Items.ForEach(item => item.Image.Draw(spriteBatch));
        }

        /// <summary>
        /// Transitions the menu to another one.
        /// </summary>
        /// <param name="alpha">Alpha.</param>
        public void Transition(float alpha)
        {
            foreach (MenuItem item in Items)
            {
                item.Image.Active = true;
                item.Image.Opacity = alpha;
                item.Image.FadeEffect.Increasing = (alpha == 0.0f);
            }
        }

        void AlignMenuItems()
        {
            Vector2 dimensions = Vector2.Zero;

            Items.ForEach(item => dimensions += new Vector2(item.Image.SourceRectangle.Width + Spacing / 2,
                                                            item.Image.SourceRectangle.Height + Spacing / 2));

            dimensions = new Vector2(
                (ScreenManager.Instance.Size.X - dimensions.X) / 2,
                (ScreenManager.Instance.Size.Y - dimensions.Y) / 2);

            foreach (MenuItem item in Items)
            {
                if ("Xx".Contains(Axis))
                {
                    item.Image.Position = new Vector2(
                        dimensions.X,
                        (ScreenManager.Instance.Size.Y - item.Image.SourceRectangle.Height) / 2);
                }
                else if ("Yy".Contains(Axis))
                {
                    item.Image.Position = new Vector2(
                        (ScreenManager.Instance.Size.X - item.Image.SourceRectangle.Width) / 2,
                        dimensions.Y);
                }

                dimensions += new Vector2(item.Image.SourceRectangle.Width + Spacing / 2, item.Image.SourceRectangle.Height + Spacing / 2);
            }
        }
    }
}

