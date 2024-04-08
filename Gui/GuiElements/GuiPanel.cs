using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.Controls;
using NuciXNA.Input;
using NuciXNA.Primitives;

namespace Narivia.Gui.Controls
{
    public class GuiPanel : GuiControl, IGuiControl
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

        public virtual void Close()
        {
            Hide();
            Closed?.Invoke(this, new MouseButtonEventArgs(MouseButton.Left, ButtonState.Pressed, InputManager.Instance.MouseLocation));
        }

        protected override void DoLoadContent()
        {
            background = new GuiImage
            {
                Id = $"{Id}_{nameof(background)}",
                ContentFile = "Interface/ProvincePanel/panel"
            };
            crystal = new GuiImage
            {
                Id = $"{Id}_{nameof(crystal)}",
                ContentFile = "Interface/ProvincePanel/crystal",
                Size = new Size2D(125, 31),
                Location = new Point2D(76, 4)
            };
            crystalOverlay = new GuiImage
            {
                Id = $"{Id}_{nameof(crystalOverlay)}",
                ContentFile = "Interface/ProvincePanel/crystal_overlay",
                Size = new Size2D(163, 43),
                Location = new Point2D(55, 0)
            };
            closeButton = new GuiButton
            {
                Id = $"{Id}_{nameof(closeButton)}",
                ContentFile = "Interface/ProvincePanel/close-button",
                Size = new Size2D(28, 28),
                Location = new Point2D(241, 0)
            };
            title = new GuiText
            {
                Id = $"{Id}_{nameof(title)}",
                ForegroundColour = Colour.Gold,
                Size = new Size2D(125, 31),
                Location = new Point2D(76, 4),
                FontName = "ProvincePanelTitleFont",
                FontOutline = FontOutline.Around
            };

            RegisterChildren(background, crystal, crystalOverlay, closeButton, title);
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
            closeButton.Clicked += OnCloseButtonClicked;
        }

        void UnregisterEvents()
        {
            closeButton.Clicked -= OnCloseButtonClicked;
        }

        void SetChildrenProperties()
        {
            crystal.TintColour = CrystalColour;
            title.Text = Title;
        }

        void OnCloseButtonClicked(object sender, MouseButtonEventArgs e)
        => Close();
    }
}
