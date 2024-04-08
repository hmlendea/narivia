using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;
using NuciXNA.Primitives;

using Narivia.Models;
using NuciXNA.Input;

namespace Narivia.Gui.Controls
{
    public class GuiBuildingCard : GuiControl, IGuiControl
    {
        const int IconSourceSize = 1024;

        GuiImage icon;
        GuiImage frame;
        GuiTooltip tooltip;

        public string BuildingTypeId { get; set; }

        public string BuildingName { get; set; }

        public string CultureId { get; set; }

        public GuiBuildingCard()
        {
            Size = new Size2D(74, 74);
        }

        public void SetHoldingProperties(Building building)
        {
            BuildingTypeId = building.TypeId;
            BuildingName = building.Name;
        }

        protected override void DoLoadContent()
        {
            icon = new GuiImage
            {
                ContentFile = "Icons/Buildings/generic"
            };

            frame = new GuiImage
            {
                ContentFile = "Interface/ProvincePanel/holding-frame"
            };

            tooltip = new GuiTooltip
            {
                FontName = "DefaultFont",
                BackgroundColour = Colour.Black,
                ForegroundColour = Colour.Gold
            };

            RegisterChildren(icon, frame, tooltip);
            RegisterEvents();
            SetChildrenProperties();
        }

        protected override void DoUnloadContent()
        {
            UnregisterEvents();
        }

        protected override void DoUpdate(GameTime gameTime)
        {
            SetChildrenProperties();
        }

        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }

        void RegisterEvents()
        {
            MouseEntered += OnMouseEntered;
            MouseLeft += OnMouseLeft;
        }

        void UnregisterEvents()
        {
            MouseEntered -= OnMouseEntered;
            MouseLeft -= OnMouseLeft;
        }

        void SetChildrenProperties()
        {
            icon.ContentFile = $"Icons/Buildings/{CultureId}/{BuildingTypeId}";

            if (string.IsNullOrWhiteSpace(BuildingTypeId) ||
                string.IsNullOrWhiteSpace(CultureId))
            {
                icon.ContentFile = "ScreenManager/missing-texture";
            }

            icon.Size = Size;
            icon.SourceRectangle = new Rectangle2D(0, 0, IconSourceSize, IconSourceSize);
            frame.Size = Size;

            tooltip.Location = new Point2D(0, Size.Height - tooltip.Size.Height);
            tooltip.Size = new Size2D(100, 20);
            tooltip.Text = BuildingName;
        }

        void OnMouseEntered(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BuildingName))
            {
                tooltip.Show();
            }
        }

        void OnMouseLeft(object sender, MouseEventArgs e)
        {
            tooltip.Hide();
        }
    }
}
