using System.IO;

using NuciXNA.Gui.GuiElements;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers;
using Narivia.Models;
using NuciXNA.Input;

namespace Narivia.Gui.GuiElements
{
    public class GuiHoldingCard : GuiElement
    {
        readonly IGameManager game;

        string currentHoldingId;

        GuiImage icon;
        GuiImage frame;
        GuiTooltip tooltip;

        public string HoldingId { get; set; }

        public string CultureId { get; set; }

        public GuiHoldingCard(IGameManager game)
        {
            this.game = game;

            Size = new Size2D(74, 74);
        }

        public override void LoadContent()
        {
            icon = new GuiImage
            {
                ContentFile = "Icons/Holdings/generic",
                Size = new Size2D(64, 64),
                Location = new Point2D(5, 5),
                SourceRectangle = new Rectangle2D(0, 0, 64, 64)
            };

            frame = new GuiImage
            {
                ContentFile = "Interface/ProvincePanel/holding-frame"
            };

            tooltip = new GuiTooltip
            {
                Size = new Size2D(100, 25),
                Location = new Point2D(0, 50),
                FontName = "DefaultFont",
                BackgroundColour = Colour.Black,
                ForegroundColour = Colour.Gold
            };

            base.LoadContent();
        }

        protected override void RegisterChildren()
        {
            base.RegisterChildren();

            AddChild(icon);
            AddChild(frame);
            AddChild(tooltip);
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            if (currentHoldingId == HoldingId)
            {
                return;
            }

            currentHoldingId = HoldingId;

            Holding holding = game.GetHolding(HoldingId);

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

            if (string.IsNullOrWhiteSpace(HoldingId))
            {
                Visible = false;
            }
            else
            {
                icon.SourceRectangle = new Rectangle2D(64 * (holding.Type - 1), 0, 64, 64);
                Visible = true;
            }

            tooltip.Text = holding.Name;
        }

        protected override void OnMouseEntered(object sender, MouseEventArgs e)
        {
            base.OnMouseEntered(sender, e);

            tooltip.Show();
        }

        protected override void OnMouseLeft(object sender, MouseEventArgs e)
        {
            base.OnMouseLeft(sender, e);

            tooltip.Hide();
        }
    }
}
