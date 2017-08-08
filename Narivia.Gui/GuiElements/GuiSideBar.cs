using System.Xml.Serialization;

using Microsoft.Xna.Framework;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics.Enumerations;
using Narivia.Graphics.Extensions;
using Narivia.Gui.GuiElements.Enumerations;
using Narivia.Models;
using Narivia.Settings;

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
                ContentFile = "Interface/Backgrounds/stone-bricks",
                TextureLayout = TextureLayout.Tile
            };

            factionImage = new GuiFactionFlag
            {
                Size = new Point(128, 128)
            };

            factionText = new GuiText
            {
                FontName = "SideBarFont",
                Size = new Point(Size.X * 2 / 3, 48),
                VerticalAlignment = VerticalAlignment.Left
            };

            turnText = new GuiText
            {
                FontName = "SideBarFont",
                Size = new Point(Size.X / 3, 48),
                VerticalAlignment = VerticalAlignment.Right
            };

            StatsButton = new GuiButton
            {
                Text = "Stats",
                ForegroundColour = ForegroundColour,
                Size = new Point(GameDefines.GUI_TILE_SIZE * 3, GameDefines.GUI_TILE_SIZE)
            };
            RecruitButton = new GuiButton
            {
                Text = "Recruit",
                ForegroundColour = ForegroundColour,
                Size = new Point(GameDefines.GUI_TILE_SIZE * 3, GameDefines.GUI_TILE_SIZE)
            };
            BuildButton = new GuiButton
            {
                Text = "Build",
                ForegroundColour = ForegroundColour,
                Size = new Point(GameDefines.GUI_TILE_SIZE * 3, GameDefines.GUI_TILE_SIZE)
            };
            TurnButton = new GuiButton
            {
                Text = "End Turn",
                ForegroundColour = ForegroundColour,
                Size = new Point(GameDefines.GUI_TILE_SIZE * 7, GameDefines.GUI_TILE_SIZE),
                Style = ButtonStyle.Narivian
            };

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

        // TODO: Handle this better
        /// <summary>
        /// Associates the game manager.
        /// </summary>
        /// <param name="game">Game.</param>
        public void AssociateGameManager(ref IGameManager game)
        {
            this.game = game;
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            background.Position = Position;
            background.Size = Size;

            factionText.Position = Position + new Point(margins, margins);
            turnText.Position = Position + new Point(Size.X - turnText.ClientRectangle.Width - margins, margins);

            factionImage.Position = Position + new Point((Size.X - factionImage.ClientRectangle.Width) / 2, factionText.ClientRectangle.Bottom + margins);

            TurnButton.Position = Position + new Point((Size.X - TurnButton.Size.X) / 2,
                                                       (Size.Y - TurnButton.Size.Y - margins));
            StatsButton.Position = Position + new Point((Size.X - StatsButton.Size.X) / 2,
                                                        (Size.Y - StatsButton.Size.Y - RecruitButton.Size.Y - margins) / 2);
            RecruitButton.Position = Position + new Point((Size.X - RecruitButton.Size.X - BuildButton.Size.X - margins * 5) / 2,
                                                          (Size.Y + StatsButton.Size.Y - RecruitButton.Size.Y + margins) / 2);
            BuildButton.Position = Position + new Point((Size.X + RecruitButton.Size.X + margins * 5 - BuildButton.Size.X) / 2,
                                                        (Size.Y + RecruitButton.Size.Y - BuildButton.Size.Y + margins) / 2);

            factionText.Text = game.GetFaction(FactionId).Name;
            factionText.ForegroundColour = ForegroundColour;

            Flag factionFlag = game.GetFactionFlag(FactionId);

            factionImage.Background = factionFlag.Background;
            factionImage.Emblem = factionFlag.Emblem;
            factionImage.Skin = factionFlag.Skin;
            factionImage.BackgroundPrimaryColour = factionFlag.BackgroundPrimaryColour.ToXnaColor();
            factionImage.BackgroundSecondaryColour = factionFlag.BackgroundSecondaryColour.ToXnaColor();
            factionImage.EmblemColour = factionFlag.EmblemColour.ToXnaColor();

            turnText.Text = $"Turn: {game.Turn}";
            turnText.ForegroundColour = ForegroundColour;
        }
    }
}
