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
        /// Gets or sets the skin.
        /// </summary>
        /// <value>The skin.</value>
        public string Skin { get; set; }

        /// <summary>
        /// Gets or sets the background primary colour.
        /// </summary>
        /// <value>The background primary colour.</value>
        public Colour BackgroundPrimaryColour { get; set; }

        /// <summary>
        /// Gets or sets the background secondary colour.
        /// </summary>
        /// <value>The background secondary colour.</value>
        public Colour BackgroundSecondaryColour { get; set; }

        /// <summary>
        /// Gets or sets the emblem colour.
        /// </summary>
        /// <value>The emblem colour.</value>
        public Colour EmblemColour { get; set; }

        GuiImage backgroundImage;
        GuiImage emblemImage;
        GuiImage skinImage;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            backgroundImage = new GuiImage
            {
                ContentFile = $"Interface/Flags/Backgrounds/{Background}",
                MaskFile = $"Interface/Flags/Skins/{Skin}_mask",
                SourceRectangle = new Rectangle(0, 0, 128, 128),
                RedReplacement = BackgroundPrimaryColour,
                GreenReplacement = BackgroundSecondaryColour
            };
            emblemImage = new GuiImage
            {
                ContentFile = $"Interface/Flags/Emblems/{Emblem}",
                MaskFile = $"Interface/Flags/Skins/{Skin}_mask",
                SourceRectangle = new Rectangle(0, 0, 128, 128),
                TintColour = EmblemColour
            };
            skinImage = new GuiImage
            {
                ContentFile = $"Interface/Flags/Skins/{Skin}",
                SourceRectangle = new Rectangle(0, 0, 128, 128)
            };

            Children.Add(backgroundImage);
            Children.Add(emblemImage);
            Children.Add(skinImage);

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
            skinImage.Position = Position;
        }
    }
}
