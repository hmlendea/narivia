using NuciXNA.Gui.GuiElements;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers.Interfaces;

namespace Narivia.Gui.GuiElements
{
    public class GuiFactionBar : GuiElement
    {
        readonly IGameManager game;

        GuiImage bar;
        GuiFactionFlag flag;

        public GuiFactionBar(IGameManager game)
        {
            this.game = game;
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

            flag.Flag = game.GetFactionFlag(game.PlayerFactionId);
        }
    }
}
