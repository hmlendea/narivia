using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;

namespace Narivia.Interface.Widgets
{
    /// <summary>
    /// Side bar widget.
    /// </summary>
    public class SideBar : Widget
    {
        Image background;

        Image factionSymbol;
        Image factionName;
        Image turnCounter;

        /// <summary>
        /// Gets or sets the faction identifier.
        /// </summary>
        /// <value>The faction identifier.</value>
        [XmlIgnore]
        public string FactionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the faction.
        /// </summary>
        /// <value>The name of the faction.</value>
        [XmlIgnore]
        public string FactionName { get; set; }

        /// <summary>
        /// Gets or sets the world identifier.
        /// </summary>
        /// <value>The world identifier.</value>
        [XmlIgnore]
        public string WorldId { get; set; }

        /// <summary>
        /// Gets or sets the turn.
        /// </summary>
        /// <value>The turn.</value>
        [XmlIgnore]
        public int Turn { get; set; }

        /// <summary>
        /// Gets or sets the text colour.
        /// </summary>
        /// <value>The text colour.</value>
        public Color TextColour { get; set; }

        /// <summary>
        /// Gets the turn button.
        /// </summary>
        /// <value>The turn button.</value>
        [XmlIgnore]
        public Button TurnButton { get; private set; }

        /// <summary>
        /// Gets the stats button.
        /// </summary>
        /// <value>The stats button.</value>
        [XmlIgnore]
        public Button StatsButton { get; private set; }

        /// <summary>
        /// Gets the relations button.
        /// </summary>
        /// <value>The relations button.</value>
        [XmlIgnore]
        public Button RelationsButton { get; private set; }

        /// <summary>
        /// Gets the recruit button.
        /// </summary>
        /// <value>The recruit button.</value>
        [XmlIgnore]
        public Button RecruitButton { get; private set; }

        /// <summary>
        /// Gets the build button.
        /// </summary>
        /// <value>The build button.</value>
        [XmlIgnore]
        public Button BuildButton { get; private set; }

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

            TurnButton = new Button
            {
                Text = "End Turn",
                TextColour = TextColour,
                Size = new Vector2(224, 32)
            };
            StatsButton = new Button
            {
                Text = "Stats",
                TextColour = TextColour,
                Size = new Vector2(96, 32)
            };
            RelationsButton = new Button
            {
                Text = "Relations",
                TextColour = TextColour,
                Size = new Vector2(96, 32)
            };
            RecruitButton = new Button
            {
                Text = "Recruit",
                TextColour = TextColour,
                Size = new Vector2(96, 32)
            };
            BuildButton = new Button
            {
                Text = "Build",
                TextColour = TextColour,
                Size = new Vector2(96, 32)
            };

            SetChildrenPositions();

            background.LoadContent();
            factionSymbol.LoadContent();

            factionName.LoadContent();
            turnCounter.LoadContent();

            StatsButton.LoadContent();
            RelationsButton.LoadContent();
            RecruitButton.LoadContent();
            BuildButton.LoadContent();
            TurnButton.LoadContent();

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

            StatsButton.UnloadContent();
            RelationsButton.UnloadContent();
            RecruitButton.UnloadContent();
            BuildButton.UnloadContent();
            TurnButton.UnloadContent();

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

            SetChildrenPositions();

            background.Update(gameTime);
            factionSymbol.Update(gameTime);

            factionName.Update(gameTime);
            turnCounter.Update(gameTime);

            StatsButton.Update(gameTime);
            RelationsButton.Update(gameTime);
            RecruitButton.Update(gameTime);
            BuildButton.Update(gameTime);
            TurnButton.Update(gameTime);

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

            StatsButton.Draw(spriteBatch);
            RelationsButton.Draw(spriteBatch);
            RecruitButton.Draw(spriteBatch);
            BuildButton.Draw(spriteBatch);
            TurnButton.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        void SetChildrenPositions()
        {
            background.Position = Position;

            factionName.Position = Position + new Vector2(margins, margins);
            turnCounter.Position = Position + new Vector2(Size.X - turnCounter.ScreenArea.Width - margins, margins);

            factionSymbol.Position = Position + new Vector2((Size.X - factionSymbol.ScreenArea.Width) / 2, factionName.ScreenArea.Bottom + margins);

            TurnButton.Position = Position + new Vector2((Size.X - TurnButton.Size.X) / 2,
                                                          Size.Y - TurnButton.Size.Y - margins * 5);
            StatsButton.Position = Position + new Vector2((int)(Size.X - StatsButton.Size.X - RelationsButton.Size.X - margins * 5) / 2,
                                                          (int)(Size.Y - StatsButton.Size.Y - RecruitButton.Size.Y - margins) / 2);
            RelationsButton.Position = Position + new Vector2((int)(Size.X + StatsButton.Size.X + margins * 5 - RelationsButton.Size.X) / 2,
                                                              (int)(Size.Y - RelationsButton.Size.Y - BuildButton.Size.Y - margins) / 2);
            RecruitButton.Position = Position + new Vector2((int)(Size.X - RecruitButton.Size.X - BuildButton.Size.X - margins * 5) / 2,
                                                            (int)(Size.Y + StatsButton.Size.Y - RecruitButton.Size.Y + margins) / 2);
            BuildButton.Position = Position + new Vector2((int)(Size.X + RecruitButton.Size.X + margins * 5 - BuildButton.Size.X) / 2,
                                                          (int)(Size.Y + RelationsButton.Size.Y - BuildButton.Size.Y + margins) / 2);
        }
    }
}
