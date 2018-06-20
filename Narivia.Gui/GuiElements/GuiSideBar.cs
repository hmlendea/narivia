using System.Xml.Serialization;

using NuciXNA.Graphics.Enumerations;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

using Narivia.GameLogic.GameManagers.Interfaces;
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
        /// Gets the turn button.
        /// </summary>
        /// <value>The turn button.</value>
        [XmlIgnore]
        public GuiButton TurnButton { get; private set; }

        IGameManager game;

        int margins = 5;

        public GuiSideBar(IGameManager game)
        {
            this.game = game;
        }

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

            TurnButton = new GuiButton
            {
                Text = "End Turn",
                ForegroundColour = ForegroundColour,
                Size = new Size2D(GameDefines.GuiTileSize * 7, GameDefines.GuiTileSize),
                Style = ButtonStyle.Narivian
            };

            AddChild(background);
            AddChild(factionImage);

            AddChild(factionText);
            AddChild(turnText);

            AddChild(TurnButton);

            base.LoadContent();
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            background.Size = Size;

            factionText.Location = new Point2D(margins, margins);
            turnText.Location = new Point2D(Size.Width - turnText.ClientRectangle.Width - margins, margins);

            factionImage.Location = new Point2D(
                (Size.Width - factionImage.ClientRectangle.Width) / 2,
                factionText.ClientRectangle.Bottom + margins);

            TurnButton.Location = new Point2D(
                (Size.Width - TurnButton.Size.Width) / 2,
                (Size.Height - TurnButton.Size.Height - margins));

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
