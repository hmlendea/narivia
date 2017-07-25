using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics;
using Narivia.Infrastructure.Helpers;
using Narivia.Models;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Side bar GUI element.
    /// </summary>
    public class GuiSideBar : GuiElement
    {
        GuiImage background;
        GuiFactionFlag factionImage;

        GuiText factionText;
        GuiText turnText;

        /// <summary>
        /// Gets or sets the faction identifier.
        /// </summary>
        /// <value>The faction identifier.</value>
        [XmlIgnore]
        public string FactionId { get; set; }
        
        /// <summary>
        /// Gets or sets the text colour.
        /// </summary>
        /// <value>The text colour.</value>
        public Colour TextColour { get; set; }

        /// <summary>
        /// Gets the stats button.
        /// </summary>
        /// <value>The stats button.</value>
        [XmlIgnore]
        public GuiButton StatsButton { get; private set; }

        /// <summary>
        /// Gets the recruit button.
        /// </summary>
        /// <value>The recruit button.</value>
        [XmlIgnore]
        public GuiButton RecruitButton { get; private set; }

        /// <summary>
        /// Gets the build button.
        /// </summary>
        /// <value>The build button.</value>
        [XmlIgnore]
        public GuiButton BuildButton { get; private set; }

        /// <summary>
        /// Gets the turn button.
        /// </summary>
        /// <value>The turn button.</value>
        [XmlIgnore]
        public GuiButton TurnButton { get; private set; }

        IGameManager game;

        int margins = 5;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            background = new GuiImage
            {
                ContentFile = "Interface/backgrounds",
                SourceRectangle = new Rectangle(0, 0, 32, 32),
                FillMode = TextureFillMode.Tile
            };

            factionImage = new GuiFactionFlag
            {
                Size = new Vector2(128, 128)
            };

            factionText = new GuiText
            {
                FontName = "SideBarFont",
                Size = new Vector2(Size.X * 2 / 3, 48),
                VerticalAlignment = VerticalAlignment.Left
            };

            turnText = new GuiText
            {
                FontName = "SideBarFont",
                Size = new Vector2(Size.X / 3, 48),
                VerticalAlignment = VerticalAlignment.Right
            };

            StatsButton = new GuiButton
            {
                Text = "Stats",
                TextColour = TextColour,
                Size = new Vector2(96, 32)
            };
            RecruitButton = new GuiButton
            {
                Text = "Recruit",
                TextColour = TextColour,
                Size = new Vector2(96, 32)
            };
            BuildButton = new GuiButton
            {
                Text = "Build",
                TextColour = TextColour,
                Size = new Vector2(96, 32)
            };
            TurnButton = new GuiButton
            {
                Text = "End Turn",
                TextColour = TextColour,
                Size = new Vector2(224, 32)
            };

            SetChildrenProperties();

            Children.Add(background);
            Children.Add(factionImage);

            Children.Add(factionText);
            Children.Add(turnText);

            Children.Add(StatsButton);
            Children.Add(RecruitButton);
            Children.Add(BuildButton);
            Children.Add(TurnButton);

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            SetChildrenProperties();

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        // TODO: Handle this better
        /// <summary>
        /// Associates the game manager.
        /// </summary>
        /// <param name="game">Game.</param>
        public void AssociateGameManager(ref IGameManager game)
        {
            this.game = game;
        }

        void SetChildrenProperties()
        {
            background.Position = Position;
            background.Scale = Size / background.SourceRectangle.Width;

            factionText.Position = Position + new Vector2(margins, margins);
            turnText.Position = Position + new Vector2(Size.X - turnText.ScreenArea.Width - margins, margins);

            factionImage.Position = Position + new Vector2((Size.X - factionImage.ScreenArea.Width) / 2, factionText.ScreenArea.Bottom + margins);

            TurnButton.Position = Position + new Vector2((int)(Size.X - TurnButton.Size.X) / 2,
                                                         (int)(Size.Y - TurnButton.Size.Y - margins));
            StatsButton.Position = Position + new Vector2((int)(Size.X - StatsButton.Size.X) / 2,
                                                          (int)(Size.Y - StatsButton.Size.Y - RecruitButton.Size.Y - margins) / 2);
            RecruitButton.Position = Position + new Vector2((int)(Size.X - RecruitButton.Size.X - BuildButton.Size.X - margins * 5) / 2,
                                                            (int)(Size.Y + StatsButton.Size.Y - RecruitButton.Size.Y + margins) / 2);
            BuildButton.Position = Position + new Vector2((int)(Size.X + RecruitButton.Size.X + margins * 5 - BuildButton.Size.X) / 2,
                                                          (int)(Size.Y + RecruitButton.Size.Y - BuildButton.Size.Y + margins) / 2);

            factionText.Text = game.GetFactionName(FactionId);
            factionText.TextColour = TextColour;

            Flag factionFlag = game.GetFactionFlag(FactionId);

            factionImage.Background = factionFlag.Background;
            factionImage.Emblem = factionFlag.Emblem;
            factionImage.Skin = factionFlag.Skin;
            factionImage.BackgroundPrimaryColour = factionFlag.BackgroundPrimaryColour;
            factionImage.BackgroundSecondaryColour = factionFlag.BackgroundSecondaryColour;
            factionImage.EmblemColour = factionFlag.EmblemColour;

            turnText.Text = $"Turn: {game.Turn}";
            turnText.TextColour = TextColour;
        }
    }
}