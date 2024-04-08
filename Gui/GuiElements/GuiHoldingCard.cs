using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;
using NuciXNA.Primitives;

using Narivia.Models;
using NuciXNA.Input;
using Narivia.Models.Enumerations;

namespace Narivia.Gui.Controls
{
    public class GuiHoldingCard : GuiControl, IGuiControl
    {
        const int IconSourceSize = 1024;

        GuiImage icon;
        GuiImage frame;
        GuiTooltip tooltip;

        public HoldingType HoldingType { get; set; }

        public string HoldingId { get; set; }

        public string HoldingName { get; set; }

        public string CultureId { get; set; }

        public GuiHoldingCard()
        {
            Size = new Size2D(74, 74);
        }

        public void SetHoldingProperties(Holding holding)
        {
            HoldingType = holding.Type;
            HoldingId = holding.Id;
            HoldingName = holding.Name;
        }

        public override void Hide()
        {
            base.Hide();
            tooltip.Hide();
        }

        protected override void DoLoadContent()
        {
            icon = new GuiImage
            {
                ContentFile = "Icons/Holdings/generic"
            };

            frame = new GuiImage
            {
                ContentFile = "Interface/ProvincePanel/holding-frame"
            };

            tooltip = new GuiTooltip
            {
                FontName = "DefaultFont",
                BackgroundColour = Colour.Black,
                ForegroundColour = Colour.Gold,
                Size = new Size2D(160, 20)
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
            if (HoldingType is null || HoldingType.Equals(HoldingType.Empty))
            {
                Hide();
                return;
            }

            if (!string.IsNullOrWhiteSpace(CultureId) &&
                (File.Exists($"Content/Icons/Holdings/{CultureId}.xnb") ||
                File.Exists($"Content/Icons/Holdings/{CultureId}.png")))
            {
                icon.ContentFile = $"Icons/Holdings/{CultureId}";
            }
            else
            {
                icon.ContentFile = "Icons/Holdings/generic";
            }

            icon.SourceRectangle = new Rectangle2D(IconSourceSize * (HoldingType - 1), 0, IconSourceSize, IconSourceSize);
            Show();

            icon.Size = Size;
            frame.Size = Size;

            tooltip.Location = new Point2D(0, Size.Height - tooltip.Size.Height);

            if (!string.IsNullOrWhiteSpace(HoldingName))
            {
                tooltip.Size = new Size2D(160, 20);
                tooltip.Text = $"{HoldingName} {HoldingType.Name}";
            }
        }

        void OnMouseEntered(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(HoldingName))
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
