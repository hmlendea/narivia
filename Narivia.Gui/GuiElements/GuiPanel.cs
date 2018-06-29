using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Input.Events;
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

        public override void LoadContent()
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

            base.LoadContent();
        }

        protected override void RegisterChildren()
        {
            base.RegisterChildren();

            AddChild(background);
            AddChild(crystal);
            AddChild(crystalOverlay);
            AddChild(closeButton);
            AddChild(title);
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();

            closeButton.Clicked += CloseButton_Clicked;
        }

        protected override void UnregisterEvents()
        {
            base.UnregisterEvents();

            closeButton.Clicked -= CloseButton_Clicked;
        }

        protected override void SetChildrenProperties()
        {
            crystal.TintColour = CrystalColour;
            title.Text = Title;

            base.SetChildrenProperties();
        }

        void CloseButton_Clicked(object sender, MouseButtonEventArgs e)
        {
            Hide();
            Closed?.Invoke(this, e);
        }
    }
}
