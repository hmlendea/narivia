using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Narivia.Screens;
using Narivia.Input;
using Narivia.Widgets;

namespace Narivia.Menus
{
    /// <summary>
    /// Menu.
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

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
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        [XmlElement("Item")]
        public List<MenuLink> Items { get; set; }

        /// <summary>
        /// Gets the item number.
        /// </summary>
        /// <value>The item number.</value>
        [XmlIgnore]
        public int ItemNumber { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.Menus.Menu"/> class.
        /// </summary>
        public Menu()
        {
            Id = string.Empty;
            ItemNumber = 0;
            Axis = "Y";
            Spacing = 30;
            Items = new List<MenuLink>();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public virtual void LoadContent()
        {
            Items.ForEach(item => item.LoadContent());

            AlignMenuItems();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            Items.ForEach(item => item.UnloadContent());
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
                if (InputManager.Instance.IsCursorInArea(Items[i].ScreenArea))
                {
                    newSelectedItemIndex = i;
                }
            }

            if (newSelectedItemIndex < 0)
            {
                newSelectedItemIndex = 0;
            }
            else if (newSelectedItemIndex > Items.Count - 1 && Items.Count > 0)
            {
                newSelectedItemIndex = Items.Count - 1;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Selected = (i == newSelectedItemIndex);
            }

            ItemNumber = newSelectedItemIndex;

            Items.ForEach(item => item.Update(gameTime));
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Items.ForEach(item => item.Draw(spriteBatch));
        }

        void AlignMenuItems()
        {
            Vector2 dimensions = Vector2.Zero;

            Items.ForEach(item => dimensions += new Vector2(item.Size.X + Spacing / 2,
                                                            item.Size.Y + Spacing / 2));

            dimensions = new Vector2(
                (ScreenManager.Instance.Size.X - dimensions.X) / 2,
                (ScreenManager.Instance.Size.Y - dimensions.Y) / 2);

            foreach (MenuLink item in Items)
            {
                if ("Xx".Contains(Axis))
                {
                    item.Position = new Vector2(
                        dimensions.X,
                        (ScreenManager.Instance.Size.Y - item.Size.Y) / 2);
                }
                else if ("Yy".Contains(Axis))
                {
                    item.Position = new Vector2(
                        (ScreenManager.Instance.Size.X - item.Size.X) / 2,
                        dimensions.Y);
                }

                dimensions += new Vector2(item.Size.X + Spacing / 2,
                                          item.Size.Y + Spacing / 2);
            }
        }
    }
}

