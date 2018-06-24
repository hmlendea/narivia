using NuciXNA.Graphics.Enumerations;
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
        GuiSimpleButton closeButton;
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
            closeButton = new GuiSimpleButton
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
                VerticalAlignment = VerticalAlignment.Centre,
                HorizontalAlignment = HorizontalAlignment.Centre,
                FontOutline = FontOutline.Around
            };

            AddChild(background);
            AddChild(crystal);
            AddChild(crystalOverlay);
            AddChild(closeButton);
            AddChild(title);

            closeButton.Clicked += CloseButton_Clicked;

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            closeButton.Clicked -= CloseButton_Clicked;

            base.UnloadContent();
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
