using System.Xml.Serialization;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics.Enumerations;
using Narivia.Graphics.Geometry;
using Narivia.Graphics.Geometry.Mapping;
using Narivia.Graphics.Mapping;
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
                Size = new Size2D(128, 128)
            };

            factionText = new GuiText
            {
                FontName = "SideBarFont",
                Size = new Size2D(Size.Width * 2 / 3, 48),
                VerticalAlignment = VerticalAlignment.Left
            };

            turnText = new GuiText
            {
                FontName = "SideBarFont",
                Size = new Size2D(Size.Width / 3, 48),
                VerticalAlignment = VerticalAlignment.Right
            };

            StatsButton = new GuiButton
            {
                Text = "Stats",
                ForegroundColour = ForegroundColour,
                Size = new Size2D(GameDefines.GUI_TILE_SIZE * 3, GameDefines.GUI_TILE_SIZE)
            };
            RecruitButton = new GuiButton
            {
                Text = "Recruit",
                ForegroundColour = ForegroundColour,
                Size = new Size2D(GameDefines.GUI_TILE_SIZE * 3, GameDefines.GUI_TILE_SIZE)
            };
            BuildButton = new GuiButton
            {
                Text = "Build",
                ForegroundColour = ForegroundColour,
                Size = new Size2D(GameDefines.GUI_TILE_SIZE * 3, GameDefines.GUI_TILE_SIZE)
            };
            TurnButton = new GuiButton
            {
                Text = "End Turn",
                ForegroundColour = ForegroundColour,
                Size = new Size2D(GameDefines.GUI_TILE_SIZE * 7, GameDefines.GUI_TILE_SIZE),
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

            background.Location = Location;
            background.Size = Size;

            factionText.Location = new Point2D(Location.X + margins, Location.Y + margins);
            turnText.Location = new Point2D(Location.X + Size.Width - turnText.ClientRectangle.Width - margins,
                                            Location.Y + margins);

            factionImage.Location = new Point2D(Location.X + (Size.Width - factionImage.ClientRectangle.Width) / 2,
                                                Location.Y + factionText.ClientRectangle.Bottom + margins);

            TurnButton.Location = new Point2D(Location.X + (Size.Width - TurnButton.Size.Width) / 2,
                                              Location.Y + (Size.Height - TurnButton.Size.Height - margins));
            StatsButton.Location = new Point2D(Location.X + (Size.Width - StatsButton.Size.Width) / 2,
                                               Location.Y + (Size.Height - StatsButton.Size.Height - RecruitButton.Size.Height - margins) / 2);
            RecruitButton.Location = new Point2D(Location.X + (Size.Width - RecruitButton.Size.Width - BuildButton.Size.Width - margins * 5) / 2,
                                                 Location.Y + (Size.Height + StatsButton.Size.Height - RecruitButton.Size.Height + margins) / 2);
            BuildButton.Location = new Point2D(Location.X + (Size.Width + RecruitButton.Size.Width + margins * 5 - BuildButton.Size.Width) / 2,
                                               Location.Y + (Size.Height + RecruitButton.Size.Height - BuildButton.Size.Height + margins) / 2);

            factionText.Text = game.GetFaction(FactionId).Name;
            factionText.ForegroundColour = ForegroundColour;

            Flag factionFlag = game.GetFactionFlag(FactionId);

            factionImage.Layer1 = factionFlag.Layer1;
            factionImage.Layer2 = factionFlag.Layer2;
            factionImage.Emblem = factionFlag.Emblem;
            factionImage.Skin = factionFlag.Skin;
            factionImage.BackgroundColour = factionFlag.BackgroundColour.ToColour();
            factionImage.Layer1Colour = factionFlag.Layer1Colour.ToColour();
            factionImage.Layer2Colour = factionFlag.Layer2Colour.ToColour();
            factionImage.EmblemColour = factionFlag.EmblemColour.ToColour();

            turnText.Text = $"Turn: {game.Turn}";
            turnText.ForegroundColour = ForegroundColour;
        }
    }
}
