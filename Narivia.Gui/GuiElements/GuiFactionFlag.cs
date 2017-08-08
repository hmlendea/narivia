using Microsoft.Xna.Framework;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// GUI faction flag element.
    /// </summary>
    public class GuiFactionFlag : GuiElement
    {
        /// <summary>
        /// Gets or sets the first layer.
        /// </summary>
        /// <value>The first layer.</value>
        public string Layer1 { get; set; }

        /// <summary>
        /// Gets or sets the second layer.
        /// </summary>
        /// <value>The second layer.</value>
        public string Layer2 { get; set; }

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
        /// Gets or sets the first layer's colour.
        /// </summary>
        /// <value>The first layer's colour.</value>
        public Color Layer1Colour { get; set; }

        /// <summary>
        /// Gets or sets the second layer's colour.
        /// </summary>
        /// <value>The second layer's colour.</value>
        public Color Layer2Colour { get; set; }

        /// <summary>
        /// Gets or sets the emblem colour.
        /// </summary>
        /// <value>The emblem colour.</value>
        public Color EmblemColour { get; set; }

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
            Size = new Point(128, 128);
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            backgroundImage = new GuiImage
            {
                SourceRectangle = Rectangle.Empty
            };
            layer1Image = new GuiImage
            {
                SourceRectangle = Rectangle.Empty
            };
            layer2Image = new GuiImage
            {
                SourceRectangle = Rectangle.Empty
            };
            emblemImage = new GuiImage
            {
                SourceRectangle = Rectangle.Empty
            };
            skinImage = new GuiImage
            {
                SourceRectangle = Rectangle.Empty
            };

            Children.Add(backgroundImage);
            Children.Add(layer1Image);
            Children.Add(layer2Image);
            Children.Add(emblemImage);
            Children.Add(skinImage);

            base.LoadContent();
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            backgroundImage.ContentFile = $"Interface/Flags/Skins/{Skin}_mask";
            backgroundImage.MaskFile = $"Interface/Flags/Skins/{Skin}_mask";
            backgroundImage.TintColour = BackgroundColour;
            backgroundImage.Position = Position;
            backgroundImage.Size = Size;

            layer1Image.ContentFile = $"Interface/Flags/Layers/{Layer1}";
            layer1Image.MaskFile = $"Interface/Flags/Skins/{Skin}_mask";
            layer1Image.TintColour = Layer1Colour;
            layer1Image.Position = Position;
            layer1Image.Size = Size;

            layer2Image.ContentFile = $"Interface/Flags/Layers/{Layer2}";
            layer2Image.MaskFile = $"Interface/Flags/Skins/{Skin}_mask";
            layer2Image.TintColour = Layer2Colour;
            layer2Image.Position = Position;
            layer2Image.Size = Size;

            emblemImage.ContentFile = $"Interface/Flags/Emblems/{Emblem}";
            emblemImage.MaskFile = backgroundImage.MaskFile;
            emblemImage.TintColour = EmblemColour;
            emblemImage.Position = Position;
            emblemImage.Size = Size;

            skinImage.ContentFile = $"Interface/Flags/Skins/{Skin}";
            skinImage.Position = Position;
            skinImage.Size = Size;
        }
    }
}
