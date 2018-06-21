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

        /// <summary>
        /// Gets or sets the faction identifier.
        /// </summary>
        /// <value>The faction identifier.</value>
        [XmlIgnore]
        public string FactionId { get; set; }

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
            
            AddChild(background);
            AddChild(factionImage);

            AddChild(factionText);

            base.LoadContent();
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();
            
            factionImage.Location = new Point2D(
                (Size.Width - factionImage.ClientRectangle.Width) / 2,
                factionText.ClientRectangle.Bottom + margins);

            factionText.Text = game.GetFaction(FactionId).Name;
            
            factionImage.Flag = game.GetFactionFlag(FactionId);
        }
    }
}
