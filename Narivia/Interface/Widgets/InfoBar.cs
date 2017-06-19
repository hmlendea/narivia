using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;
using Narivia.Input;
using Narivia.Input.Events;

namespace Narivia.Interface.Widgets
{
    /// <summary>
    /// Info bar widget.
    /// </summary>
    public class InfoBar : Widget
    {
        Image background;

        Image regionsIcon, regionsText;
        Image holdingsIcon, holdingsText;
        Image wealthIcon, wealthText;
        Image troopsIcon, troopsText;

        ToolTip regionsTooltip;
        ToolTip holdingsTooltip;
        ToolTip wealthTooltip;
        ToolTip troopsTooltip;

        /// <summary>
        /// Gets or sets the regions count.
        /// </summary>
        /// <value>The regions count.</value>
        [XmlIgnore]
        public int Regions { get; set; }

        /// <summary>
        /// Gets or sets the holdings count.
        /// </summary>
        /// <value>The holdings count.</value>
        [XmlIgnore]
        public int Holdings { get; set; }

        /// <summary>
        /// Gets or sets the wealth.
        /// </summary>
        /// <value>The wealth.</value>
        [XmlIgnore]
        public int Wealth { get; set; }

        /// <summary>
        /// Gets or sets the troops count.
        /// </summary>
        /// <value>The troops count.</value>
        [XmlIgnore]
        public Dictionary<string, int> Troops { get; set; }

        /// <summary>
        /// Gets or sets the background colour.
        /// </summary>
        /// <value>The background colour.</value>
        public Color BackgroundColour { get; set; }

        /// <summary>
        /// Gets or sets the text colour.
        /// </summary>
        /// <value>The text colour.</value>
        public Color TextColour { get; set; }

        /// <summary>
        /// Gets or sets the spacing.
        /// </summary>
        /// <value>The spacing.</value>
        public float Spacing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.Interface.Widgets.InfoBar"/> class.
        /// </summary>
        public InfoBar()
        {
            BackgroundColour = Color.Black;
            TextColour = Color.Gold;

            Spacing = 6.0f;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            background = new Image
            {
                ImagePath = "ScreenManager/FillImage",
                SourceRectangle = new Rectangle(0, 0, 1, 1),
                Position = Position,
                Scale = Size,
                Tint = BackgroundColour
            };

            regionsIcon = new Image
            {
                ImagePath = "Interface/game_icons",
                SourceRectangle = new Rectangle(0, 0, 16, 16)
            };
            holdingsIcon = new Image
            {
                ImagePath = "Interface/game_icons",
                SourceRectangle = new Rectangle(48, 0, 16, 16)
            };
            wealthIcon = new Image
            {
                ImagePath = "Interface/game_icons",
                SourceRectangle = new Rectangle(16, 0, 16, 16)
            };
            troopsIcon = new Image
            {
                ImagePath = "Interface/game_icons",
                SourceRectangle = new Rectangle(32, 0, 16, 16)
            };

            regionsText = new Image
            {
                SpriteSize = new Vector2(32, 16),
                FontName = "InfoBarFont",
                Tint = TextColour
            };
            holdingsText = new Image
            {
                SpriteSize = new Vector2(32, 16),
                FontName = "InfoBarFont",
                Tint = TextColour
            };
            wealthText = new Image
            {
                SpriteSize = new Vector2(48, 16),
                FontName = "InfoBarFont",
                Tint = TextColour
            };
            troopsText = new Image
            {
                SpriteSize = new Vector2(48, 16),
                FontName = "InfoBarFont",
                Tint = TextColour
            };

            regionsTooltip = new ToolTip
            {
                Text = "Regions",
                Size = new Vector2(100, 20),
                Visible = false
            };
            holdingsTooltip = new ToolTip
            {
                Text = "Holdings",
                Size = new Vector2(100, 20),
                Visible = false
            };
            wealthTooltip = new ToolTip
            {
                Text = "Wealth",
                Size = new Vector2(100, 20),
                Visible = false
            };
            troopsTooltip = new ToolTip
            {
                Text = "Troops",
                Size = new Vector2(128, 128),
                Visible = false
            };

            background.LoadContent();

            regionsIcon.LoadContent();
            holdingsIcon.LoadContent();
            wealthIcon.LoadContent();
            troopsIcon.LoadContent();

            regionsText.LoadContent();
            holdingsText.LoadContent();
            wealthText.LoadContent();
            troopsText.LoadContent();

            regionsTooltip.LoadContent();
            holdingsTooltip.LoadContent();
            wealthTooltip.LoadContent();
            troopsTooltip.LoadContent();

            base.LoadContent();

            InputManager.Instance.MouseMoved += InputManager_OnMouseMoved;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            background.UnloadContent();

            regionsIcon.UnloadContent();
            holdingsIcon.UnloadContent();
            wealthIcon.UnloadContent();
            troopsIcon.UnloadContent();

            regionsText.UnloadContent();
            holdingsText.UnloadContent();
            wealthText.UnloadContent();
            troopsText.UnloadContent();

            regionsTooltip.UnloadContent();
            holdingsTooltip.UnloadContent();
            wealthTooltip.UnloadContent();
            troopsTooltip.UnloadContent();

            base.UnloadContent();

            InputManager.Instance.MouseMoved -= InputManager_OnMouseMoved;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enabled)
            {
                return;
            }

            regionsText.Text = Regions.ToString();
            holdingsText.Text = Holdings.ToString();
            wealthText.Text = Wealth.ToString();
            troopsText.Text = Troops.Values.Sum().ToString();

            regionsIcon.Position = new Vector2(Position.X + Spacing, Position.Y + (Size.Y - regionsIcon.ScreenArea.Height) / 2);
            regionsText.Position = new Vector2(regionsIcon.ScreenArea.Right + Spacing, Position.Y + (Size.Y - regionsText.ScreenArea.Height) / 2);
            holdingsIcon.Position = new Vector2(regionsText.ScreenArea.Right + Spacing, regionsIcon.Position.Y);
            holdingsText.Position = new Vector2(holdingsIcon.ScreenArea.Right + Spacing, regionsText.Position.Y);
            wealthIcon.Position = new Vector2(holdingsText.ScreenArea.Right + Spacing, holdingsIcon.Position.Y);
            wealthText.Position = new Vector2(wealthIcon.ScreenArea.Right + Spacing, holdingsText.Position.Y);
            troopsIcon.Position = new Vector2(wealthText.ScreenArea.Right + Spacing, wealthIcon.Position.Y);
            troopsText.Position = new Vector2(troopsIcon.ScreenArea.Right + Spacing, wealthText.Position.Y);

            regionsTooltip.Position = new Vector2(regionsIcon.Position.X, ScreenArea.Bottom);
            holdingsTooltip.Position = new Vector2(holdingsIcon.Position.X, ScreenArea.Bottom);
            wealthTooltip.Position = new Vector2(wealthIcon.Position.X, ScreenArea.Bottom);
            troopsTooltip.Position = new Vector2(troopsIcon.Position.X, ScreenArea.Bottom);

            troopsTooltip.Text = string.Empty;
            Troops.ToList().ForEach(t => troopsTooltip.Text += $"{t.Key}: {t.Value}\n");

            background.Update(gameTime);

            regionsIcon.Update(gameTime);
            holdingsIcon.Update(gameTime);
            wealthIcon.Update(gameTime);
            troopsIcon.Update(gameTime);

            regionsText.Update(gameTime);
            holdingsText.Update(gameTime);
            wealthText.Update(gameTime);
            troopsText.Update(gameTime);

            regionsTooltip.Update(gameTime);
            holdingsTooltip.Update(gameTime);
            wealthTooltip.Update(gameTime);
            troopsTooltip.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }

            background.Draw(spriteBatch);

            regionsIcon.Draw(spriteBatch);
            holdingsIcon.Draw(spriteBatch);
            wealthIcon.Draw(spriteBatch);
            troopsIcon.Draw(spriteBatch);

            regionsText.Draw(spriteBatch);
            holdingsText.Draw(spriteBatch);
            wealthText.Draw(spriteBatch);
            troopsText.Draw(spriteBatch);

            regionsTooltip.Draw(spriteBatch);
            holdingsTooltip.Draw(spriteBatch);
            wealthTooltip.Draw(spriteBatch);
            troopsTooltip.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        void InputManager_OnMouseMoved(object sender, MouseEventArgs e)
        {
            if (regionsIcon.ScreenArea.Contains(e.CurrentMousePosition) ||
                regionsText.ScreenArea.Contains(e.CurrentMousePosition))
            {
                regionsTooltip.Visible = true;
            }
            else
            {
                regionsTooltip.Visible = false;
            }

            if (holdingsIcon.ScreenArea.Contains(e.CurrentMousePosition) ||
                holdingsText.ScreenArea.Contains(e.CurrentMousePosition))
            {
                holdingsTooltip.Visible = true;
            }
            else
            {
                holdingsTooltip.Visible = false;
            }

            if (wealthIcon.ScreenArea.Contains(e.CurrentMousePosition) ||
                wealthText.ScreenArea.Contains(e.CurrentMousePosition))
            {
                wealthTooltip.Visible = true;
            }
            else
            {
                wealthTooltip.Visible = false;
            }

            if (troopsIcon.ScreenArea.Contains(e.CurrentMousePosition) ||
                troopsText.ScreenArea.Contains(e.CurrentMousePosition))
            {
                troopsTooltip.Visible = true;
            }
            else
            {
                troopsTooltip.Visible = false;
            }
        }
    }
}
