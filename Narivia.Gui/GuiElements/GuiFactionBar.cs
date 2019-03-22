using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            bar = new GuiImage
            {
                Id = $"{Id}_{nameof(bar)}",
                ContentFile = "Interface/FactionBar/bar"
            };
            flag = new GuiFactionFlag
            {
                Id = $"{Id}_{nameof(flag)}",
                Location = new Point2D(83, 14),
                Size = new Size2D(76, 76)
            };
            
            RegisterChildren();
            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {

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

        void RegisterChildren()
        {
            AddChild(bar);
            AddChild(flag);
        }

        void SetChildrenProperties()
        {
            string factionId = gameManager.PlayerFactionId;
            flag.Flag = gameManager.GetFactionFlag(factionId);
        }
    }
}
