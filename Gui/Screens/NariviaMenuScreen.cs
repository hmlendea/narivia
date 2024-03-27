using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui;
using NuciXNA.Gui.Controls;
using NuciXNA.Gui.Screens;
using NuciXNA.Primitives;

namespace Narivia.Gui.Screens
{
    public class NariviaMenuScreen : Screen
    {
        protected static int ControlSpacing => 8;
        protected static int ButtonSpacing => 48;
        protected static Size2D ButtonSize => new(300, 50);

        GuiImage backgroundImage;

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            BackgroundColour = Colour.MaterialGrey;

            backgroundImage = new GuiImage()
            {
                Id = $"{Id}_{nameof(backgroundImage)}",
                Location = Point2D.Empty,
                TextureLayout = TextureLayout.Tile,
                SourceRectangle = new Rectangle2D(Point2D.Empty, new Point2D(32, 32)),
                ContentFile = "Content/Interface/Backgrounds/stone-bricks"
            };

            GuiManager.Instance.RegisterControls(backgroundImage);
        }

        protected override void DoUnloadContent()
        {

        }

        protected override void DoUpdate(GameTime gameTime)
        {
            backgroundImage.Size = ScreenManager.Instance.Size;
        }

        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }
    }
}
