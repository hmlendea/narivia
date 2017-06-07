using System;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;

namespace Narivia.Interface.Widgets
{
    public class SideBar : Widget
    {
        Image background;

        Image factionSymbol;
        Image factionName;
        Image turnCounter;

        Button turnButton;
        Button statsButton;
        Button relationsButton;

        [XmlIgnore]
        public string FactionId { get; set; }

        [XmlIgnore]
        public string FactionName { get; set; }

        [XmlIgnore]
        public string WorldId { get; set; }

        [XmlIgnore]
        public int Turn { get; set; }

        public Color TextColour { get; set; }

        public event EventHandler TurnButtonClicked;
        public event EventHandler StatsButtonClicked;
        public event EventHandler RelationsButtonClicked;

        int margins = 5;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            background = new Image
            {
                ImagePath = "Interface/backgrounds",
                SourceRectangle = new Rectangle(0, 0, 32, 32),
                Position = Position,
                TextureFillMode = TextureFillMode.Tile,
                Scale = Size / 32
            };

            factionSymbol = new Image
            {
                ImagePath = $"World/Assets/{WorldId}/symbols/{FactionId}",
                SourceRectangle = new Rectangle(0, 0, 128, 128)
            };

            factionName = new Image
            {
                Text = FactionName,
                FontName = "SideBarFont",
                SpriteSize = new Vector2(Size.X * 2 / 3, 48),
                TextVerticalAlignment = VerticalAlignment.Left,
                Tint = TextColour
            };

            turnCounter = new Image
            {
                Text = $"Turn: {Turn}",
                FontName = "SideBarFont",
                SpriteSize = new Vector2(Size.X / 3, 48),
                TextVerticalAlignment = VerticalAlignment.Right,
                Tint = TextColour
            };

            turnButton = new Button
            {
                Text = "End Turn",
                TextColour = TextColour,
                Size = new Vector2(224, 32)
            };
            statsButton = new Button
            {
                Text = "Stats",
                TextColour = TextColour,
                Size = new Vector2(96, 32)
            };
            relationsButton = new Button
            {
                Text = "Relations",
                TextColour = TextColour,
                Size = new Vector2(96, 32)
            };

            background.LoadContent();
            factionSymbol.LoadContent();

            factionName.LoadContent();
            turnCounter.LoadContent();

            turnButton.LoadContent();
            statsButton.LoadContent();
            relationsButton.LoadContent();

            turnButton.Clicked += OnTurnButtonClicked;
            statsButton.Clicked += OnStatsButtonClicked;
            relationsButton.Clicked += OnRelationsButtonClicked;

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            background.UnloadContent();
            factionSymbol.UnloadContent();

            factionName.UnloadContent();
            turnCounter.UnloadContent();

            turnButton.UnloadContent();
            statsButton.UnloadContent();
            relationsButton.UnloadContent();

            base.UnloadContent();
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

            turnCounter.Text = $"Turn: {Turn}";

            factionName.Position = Position + new Vector2(margins, margins);
            turnCounter.Position = Position + new Vector2(Size.X - turnCounter.ScreenArea.Width - margins, margins);

            factionSymbol.Position = Position + new Vector2((Size.X - factionSymbol.ScreenArea.Width) / 2, factionName.ScreenArea.Bottom + margins);

            turnButton.Position = Position + new Vector2((Size.X - turnButton.Size.X) / 2,
                                                          Size.Y - turnButton.Size.Y - margins * 5);
            statsButton.Position = Position + new Vector2((int)(Size.X - statsButton.Size.X - relationsButton.Size.X - margins * 5) / 2,
                                                          (int)(Size.Y - statsButton.Size.Y) / 2);
            relationsButton.Position = Position + new Vector2((int)(Size.X + statsButton.Size.X + margins * 5 - relationsButton.Size.X) / 2,
                                                              (int)(Size.Y - relationsButton.Size.Y) / 2);

            background.Update(gameTime);
            factionSymbol.Update(gameTime);

            factionName.Update(gameTime);
            turnCounter.Update(gameTime);

            turnButton.Update(gameTime);
            statsButton.Update(gameTime);
            relationsButton.Update(gameTime);

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
            factionSymbol.Draw(spriteBatch);

            factionName.Draw(spriteBatch);
            turnCounter.Draw(spriteBatch);

            turnButton.Draw(spriteBatch);
            statsButton.Draw(spriteBatch);
            relationsButton.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        protected void OnTurnButtonClicked(object sender, EventArgs e)
        {
            if (TurnButtonClicked != null)
            {
                TurnButtonClicked(this, null);
            }
        }

        protected void OnStatsButtonClicked(object sender, EventArgs e)
        {
            if (StatsButtonClicked != null)
            {
                StatsButtonClicked(this, null);
            }
        }

        protected void OnRelationsButtonClicked(object sender, EventArgs e)
        {
            if (RelationsButtonClicked != null)
            {
                RelationsButtonClicked(this, null);
            }
        }
    }
}
