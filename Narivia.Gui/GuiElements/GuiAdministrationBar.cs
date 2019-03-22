using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.GuiElements;
using NuciXNA.Primitives;

namespace Narivia.Gui.GuiElements
{
    public class GuiAdministrationBar : GuiElement
    {
        GuiImage bar;

        public GuiButton BuildButton { get; private set; }
        public GuiButton RecruitButton { get; private set; }
        public GuiButton StatsButton { get; private set; }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            bar = new GuiImage
            {
                ContentFile = "Interface/AdministrationBar/bar"
            };
            BuildButton = new GuiButton
            {
                Id = $"{Id}_{nameof(BuildButton)}",
                ContentFile = "Interface/AdministrationBar/button-build",
                Location = new Point2D(16, 10),
                Size = new Size2D(26, 26)
            };
            RecruitButton = new GuiButton
            {
                Id = $"{Id}_{nameof(RecruitButton)}",
                ContentFile = "Interface/AdministrationBar/button-recruit",
                Location = new Point2D(52, 10),
                Size = new Size2D(26, 26)
            };
            StatsButton = new GuiButton
            {
                Id = $"{Id}_{nameof(StatsButton)}",
                ContentFile = "Interface/AdministrationBar/button-stats",
                Location = new Point2D(89, 10),
                Size = new Size2D(26, 26)
            };
            
            AddChild(bar);
            AddChild(BuildButton);
            AddChild(RecruitButton);
            AddChild(StatsButton);
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

        }

        /// <summary>
        /// Draw the content on the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }
    }
}
