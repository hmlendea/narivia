using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Infrastructure.Helpers;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// GUI faction flag element.
    /// </summary>
    public class GuiFactionFlag : GuiElement
    {
        /// <summary>
        /// Gets or sets the background.
        /// </summary>
        /// <value>The background.</value>
        public string Background { get; set; }

        /// <summary>
        /// Gets or sets the emblem.
        /// </summary>
        /// <value>The emblem.</value>
        public string Emblem { get; set; }

        /// <summary>
        /// Gets or sets the primary colour.
        /// </summary>
        /// <value>The primary colour.</value>
        public Colour PrimaryColour { get; set; }

        /// <summary>
        /// Gets or sets the secondary colour.
        /// </summary>
        /// <value>The secondary colour.</value>
        public Colour SecondaryColour { get; set; }

        GuiImage backgroundImage;
        GuiImage emblemImage;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            backgroundImage = new GuiImage
            {
                ContentFile = $"Interface/Flags/Backgrounds/{Background}",
                SourceRectangle = new Rectangle(0, 0, 128, 128)
            };
            emblemImage = new GuiImage
            {
                ContentFile = $"Interface/Flags/Emblems/{Emblem}",
                SourceRectangle = new Rectangle(0, 0, 128, 128)
            };

            Children.Add(backgroundImage);
            Children.Add(emblemImage);

            SetChildrenProperties();

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            SetChildrenProperties();

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        void SetChildrenProperties()
        {
            backgroundImage.Position = Position;
            emblemImage.Position = Position;
        }
    }
}
