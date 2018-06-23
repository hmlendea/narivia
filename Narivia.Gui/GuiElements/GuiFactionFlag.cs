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
        public override void LoadContent()
        {
            backgroundImage = new GuiImage
            {
                SourceRectangle = Rectangle2D.Empty
            };
            layer1Image = new GuiImage
            {
                SourceRectangle = Rectangle2D.Empty
            };
            layer2Image = new GuiImage
            {
                SourceRectangle = Rectangle2D.Empty
            };
            emblemImage = new GuiImage
            {
                SourceRectangle = Rectangle2D.Empty
            };
            skinImage = new GuiImage
            {
                SourceRectangle = Rectangle2D.Empty
            };

            AddChild(backgroundImage);
            AddChild(layer1Image);
            AddChild(layer2Image);
            AddChild(emblemImage);
            AddChild(skinImage);

            base.LoadContent();
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            backgroundImage.ContentFile = $"Interface/Flags/Skins/{Flag.Skin}_mask";
            backgroundImage.MaskFile = $"Interface/Flags/Skins/{Flag.Skin}_mask";
            backgroundImage.TintColour = Flag.BackgroundColour.ToColour();

            layer1Image.ContentFile = $"Interface/Flags/Layers/{Flag.Layer1}";
            layer1Image.MaskFile = $"Interface/Flags/Skins/{Flag.Skin}_mask";
            layer1Image.TintColour = Flag.Layer1Colour.ToColour();

            layer2Image.ContentFile = $"Interface/Flags/Layers/{Flag.Layer2}";
            layer2Image.MaskFile = $"Interface/Flags/Skins/{Flag.Skin}_mask";
            layer2Image.TintColour = Flag.Layer2Colour.ToColour();

            emblemImage.ContentFile = $"Interface/Flags/Emblems/{Flag.Emblem}";
            emblemImage.MaskFile = backgroundImage.MaskFile;
            emblemImage.TintColour = Flag.EmblemColour.ToColour();

            skinImage.ContentFile = $"Interface/Flags/Skins/{Flag.Skin}";
        }
    }
}
