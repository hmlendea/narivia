using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers;
using Narivia.Models;
using NuciXNA.Input;

namespace Narivia.Gui.Controls
{
    public class GuiHoldingCard : GuiControl, IGuiControl
    {
        const int IconSourceSize = 1024;
        readonly IHoldingManager holdingManager;

        string currentHoldingId;

        GuiImage icon;
        GuiImage frame;
        GuiTooltip tooltip;

        public string HoldingId { get; set; }

        public string CultureId { get; set; }

        public GuiHoldingCard(IHoldingManager holdingManager)
        {
            this.holdingManager = holdingManager;

            Size = new Size2D(74, 74);
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            icon = new GuiImage
            {
                ContentFile = "Icons/Holdings/generic",
                Size = new Size2D(64, 64),
                Location = new Point2D(5, 5)
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

            RegisterChildren(icon, frame, tooltip);
            RegisterEvents();
            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            UnregisterEvents();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            SetChildrenProperties();
        }

        /// <summary>
        /// Draw the content on the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
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
            if (currentHoldingId == HoldingId)
            {
                return;
            }

            currentHoldingId = HoldingId;

            Holding holding = holdingManager.GetHolding(HoldingId);

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
                Hide();
            }
            else
            {
                icon.SourceRectangle = new Rectangle2D(IconSourceSize * (holding.Type - 1), 0, IconSourceSize, IconSourceSize);
                Show();
            }

            tooltip.Text = holding.Name;
        }

        void OnMouseEntered(object sender, MouseEventArgs e)
        {
            tooltip.Show();
        }

        void OnMouseLeft(object sender, MouseEventArgs e)
        {
            tooltip.Hide();
        }
    }
}
