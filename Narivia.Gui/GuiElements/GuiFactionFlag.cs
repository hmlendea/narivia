using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.GuiElements;
using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

using Narivia.Models;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// GUI faction flag element.
    /// </summary>
    public class GuiFactionFlag : GuiElement
    {
        public Flag Flag { get; set; }

        GuiImage backgroundImage;
        GuiImage layer1Image;
        GuiImage layer2Image;
        GuiImage emblemImage;
        GuiImage skinImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiFactionFlag"/> class.
        /// </summary>
        public GuiFactionFlag()
        {
            Size = new Size2D(128, 128);
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            backgroundImage = new GuiImage
            {
                Id = $"{Id}_{nameof(backgroundImage)}",
                SourceRectangle = Rectangle2D.Empty
            };
            layer1Image = new GuiImage
            {
                Id = $"{Id}_{nameof(layer1Image)}",
                SourceRectangle = Rectangle2D.Empty
            };
            layer2Image = new GuiImage
            {
                Id = $"{Id}_{nameof(layer2Image)}",
                SourceRectangle = Rectangle2D.Empty
            };
            emblemImage = new GuiImage
            {
                Id = $"{Id}_{nameof(emblemImage)}",
                SourceRectangle = Rectangle2D.Empty
            };
            skinImage = new GuiImage
            {
                Id = $"{Id}_{nameof(skinImage)}",
                SourceRectangle = Rectangle2D.Empty
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
            AddChild(backgroundImage);
            AddChild(layer1Image);
            AddChild(layer2Image);
            AddChild(emblemImage);
            AddChild(skinImage);
        }

        void SetChildrenProperties()
        {
            backgroundImage.ContentFile = $"Interface/Flags/Skins/{Flag.Skin}_mask";
            backgroundImage.MaskFile = $"Interface/Flags/Skins/{Flag.Skin}_mask";
            backgroundImage.TintColour = Flag.BackgroundColour;

            layer1Image.ContentFile = $"Interface/Flags/Layers/{Flag.Layer1}";
            layer1Image.MaskFile = $"Interface/Flags/Skins/{Flag.Skin}_mask";
            layer1Image.TintColour = Flag.Layer1Colour;

            layer2Image.ContentFile = $"Interface/Flags/Layers/{Flag.Layer2}";
            layer2Image.MaskFile = $"Interface/Flags/Skins/{Flag.Skin}_mask";
            layer2Image.TintColour = Flag.Layer2Colour;

            emblemImage.ContentFile = $"Interface/Flags/Emblems/{Flag.Emblem}";
            emblemImage.MaskFile = backgroundImage.MaskFile;
            emblemImage.TintColour = Flag.EmblemColour;

            skinImage.ContentFile = $"Interface/Flags/Skins/{Flag.Skin}";
        }
    }
}
