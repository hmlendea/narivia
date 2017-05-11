using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Graphics;

namespace Narivia.Widgets
{
    public class Widget
    {
        protected Image Image { get; set; }

        /// <summary>
        /// Gets the position of this widget.
        /// </summary>
        /// <value>The position.</value>
        public Vector2 Position
        {
            get { return Image.Position; }
            set { Image.Position = value; }
        }

        /// <summary>
        /// Gets the size of this widget.
        /// </summary>
        /// <value>The size.</value>
        public Vector2 Size
        {
            get { return Image.Size; }
            set { Image.Size = value; }
        }

        /// <summary>
        /// Gets the screen area covered by this widget.
        /// </summary>
        /// <value>The screen area.</value>
        public Rectangle ScreenArea => Image.ScreenArea;

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>The opacity.</value>
        public float Opacity
        {
            get { return Image.Opacity; }
            set { Image.Opacity = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Narivia.Widgets.Widget"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Narivia.Widgets.Widget"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.Widgets.Widget"/> class.
        /// </summary>
        public Widget()
        {
            Image = new Image();
            Enabled = true;
            Visible = true;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public virtual void LoadContent()
        {
            Image.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public virtual void UnloadContent()
        {
            Image.UnloadContent();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                Image.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                Image.Draw(spriteBatch);
            }
        }
    }
}
