using NuciXNA.Gui.GuiElements;
using NuciXNA.Primitives;

namespace Narivia.Gui.GuiElements
{
    public class GuiAdministrationBar : GuiElement
    {
        GuiImage bar;

        public GuiSimpleButton BuildButton { get; private set; }
        public GuiSimpleButton RecruitButton { get; private set; }
        public GuiSimpleButton StatsButton { get; private set; }

        public override void LoadContent()
        {
            bar = new GuiImage
            {
                ContentFile = "Interface/AdministrationBar/bar"
            };
            BuildButton = new GuiSimpleButton
            {
                ContentFile = "Interface/AdministrationBar/button-build",
                Size = new Size2D(26, 26)
            };
            RecruitButton = new GuiSimpleButton
            {
                ContentFile = "Interface/AdministrationBar/button-recruit",
                Size = new Size2D(26, 26)
            };
            StatsButton = new GuiSimpleButton
            {
                ContentFile = "Interface/AdministrationBar/button-stats",
                Size = new Size2D(26, 26)
            };

            Children.Add(bar);
            Children.Add(BuildButton);
            Children.Add(RecruitButton);
            Children.Add(StatsButton);

            base.LoadContent();
        }

        protected override void SetChildrenProperties()
        {
            bar.Location = Location;

            BuildButton.Location = new Point2D(Location.X + 16, Location.Y + 10);
            RecruitButton.Location = new Point2D(Location.X + 52, Location.Y + 10);
            StatsButton.Location = new Point2D(Location.X + 89, Location.Y + 10);

            base.SetChildrenProperties();
        }
    }
}
