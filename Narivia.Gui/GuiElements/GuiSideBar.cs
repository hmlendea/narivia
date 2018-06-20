using System.Xml.Serialization;

using NuciXNA.Graphics.Enumerations;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Gui.GuiElements.Enumerations;
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
                VerticalAlignment = VerticalAlignment.Left,
                Location = new Point2D(margins, margins)
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
                Style = ButtonStyle.Narivian,
                Size = new Size2D(GameDefines.GuiTileSize * 7, GameDefines.GuiTileSize)
            };
            TurnButton.Location = new Point2D(
                (Size.Width - TurnButton.Size.Width) / 2,
                (Size.Height - TurnButton.Size.Height - margins));

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
            
            turnText.Location = new Point2D(Size.Width - turnText.ClientRectangle.Width - margins, margins);

            factionImage.Location = new Point2D(
                (Size.Width - factionImage.ClientRectangle.Width) / 2,
                factionText.ClientRectangle.Bottom + margins);

            factionText.Text = game.GetFaction(FactionId).Name;
            factionText.ForegroundColour = ForegroundColour;
            
            factionImage.Flag = game.GetFactionFlag(FactionId);

            turnText.Text = $"Turn: {game.Turn}";
            turnText.ForegroundColour = ForegroundColour;
        }
    }
}
