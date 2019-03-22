using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Input;
using NuciXNA.Primitives;

namespace Narivia.Gui.GuiElements
{
    public class GuiPanel : GuiElement
    {
        GuiImage background;
        GuiImage crystal;
        GuiImage crystalOverlay;
        GuiButton closeButton;
        GuiText title;

        public string Title { get; set; }

        public Colour CrystalColour { get; set; }

        public event MouseButtonEventHandler Closed;

        public GuiPanel()
        {
            Size = new Size2D(274, 424);
            CrystalColour = Colour.Red;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            background = new GuiImage
            {
                ContentFile = "Interface/ProvincePanel/panel"
            };
            crystal = new GuiImage
            {
                ContentFile = "Interface/ProvincePanel/crystal",
                Size = new Size2D(125, 31),
                Location = new Point2D(76, 4)
            };
            crystalOverlay = new GuiImage
            {
                ContentFile = "Interface/ProvincePanel/crystal_overlay",
                Size = new Size2D(163, 43),
                Location = new Point2D(55, 0)
            };
            closeButton = new GuiButton
            {
                ContentFile = "Interface/ProvincePanel/close-button",
                Size = new Size2D(28, 28),
                Location = new Point2D(241, 0)
            };
            title = new GuiText
            {
                ForegroundColour = Colour.Gold,
                Size = new Size2D(125, 31),
                Location = new Point2D(76, 4),
                FontName = "ProvincePanelTitleFont",
                FontOutline = FontOutline.Around
            };

            SetChildrenProperties();

            closeButton.Clicked += OnCloseButtonClicked;

            AddChild(background);
            AddChild(crystal);
            AddChild(crystalOverlay);
            AddChild(closeButton);
            AddChild(title);
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            closeButton.Clicked -= OnCloseButtonClicked;
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

        void SetChildrenProperties()
        {
            crystal.TintColour = CrystalColour;
            title.Text = Title;
        }

        void OnCloseButtonClicked(object sender, MouseButtonEventArgs e)
        {
            Hide();
            Closed?.Invoke(this, e);
        }
    }
}
