using NuciXNA.Gui.GuiElements;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Models;

namespace Narivia.Gui.GuiElements
{
    public class GuiHoldingCard : GuiElement
    {
        readonly IGameManager game;

        string currentHoldingId;

        GuiImage icon;
        GuiImage frame;

        public string HoldingId { get; set; }

        public GuiHoldingCard(IGameManager game)
        {
            this.game = game;

            Size = new Size2D(274, 424);
        }

        public override void LoadContent()
        {
            icon = new GuiImage
            {
                ContentFile = $"World/Assets/{game.WorldId}/holdings/generic",
                Size = new Size2D(64, 64),
                Location = new Point2D(5, 5),
                SourceRectangle = new Rectangle2D(0, 0, 64, 64)
            };

            frame = new GuiImage
            {
                ContentFile = "Interface/ProvincePanel/holding-frame"
            };

            base.LoadContent();
        }

        protected override void RegisterChildren()
        {
            base.RegisterChildren();

            AddChild(icon);
            AddChild(frame);
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

            if (string.IsNullOrWhiteSpace(HoldingId))
            {
                Visible = false;
            }
            else
            {
                icon.SourceRectangle = new Rectangle2D(64 * ((int)holding.Type - 1), 0, 64, 64);
                Visible = true;
            }
        }
    }
}
