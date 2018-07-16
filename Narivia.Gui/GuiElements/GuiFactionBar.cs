using NuciXNA.Gui.GuiElements;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers;

namespace Narivia.Gui.GuiElements
{
    public class GuiFactionBar : GuiElement
    {
        readonly IGameManager gameManager;

        GuiImage bar;
        GuiFactionFlag flag;

        public GuiFactionBar(IGameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public override void LoadContent()
        {
            bar = new GuiImage
            {
                ContentFile = "Interface/FactionBar/bar"
            };
            flag = new GuiFactionFlag
            {
                Location = new Point2D(83, 14),
                Size = new Size2D(76, 76)
            };

            base.LoadContent();
        }

        protected override void RegisterChildren()
        {
            base.RegisterChildren();

            AddChild(bar);
            AddChild(flag);
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            string factionId = gameManager.PlayerFactionId;
            flag.Flag = gameManager.GetFactionFlag(factionId);
        }
    }
}
