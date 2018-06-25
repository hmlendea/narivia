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
                Location = new Point2D(16, 10),
                Size = new Size2D(26, 26)
            };
            RecruitButton = new GuiSimpleButton
            {
                ContentFile = "Interface/AdministrationBar/button-recruit",
                Location = new Point2D(52, 10),
                Size = new Size2D(26, 26)
            };
            StatsButton = new GuiSimpleButton
            {
                ContentFile = "Interface/AdministrationBar/button-stats",
                Location = new Point2D(89, 10),
                Size = new Size2D(26, 26)
            };

            base.LoadContent();
        }

        protected override void RegisterChildren()
        {
            base.RegisterChildren();
            
            AddChild(bar);
            AddChild(BuildButton);
            AddChild(RecruitButton);
            AddChild(StatsButton);
        }
    }
}
