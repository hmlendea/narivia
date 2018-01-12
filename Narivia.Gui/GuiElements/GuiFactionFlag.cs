using NuciXNA.Primitives;

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
        public Colour Layer1Colour { get; set; }

        /// <summary>
        /// Gets or sets the second layer's colour.
        /// </summary>
        /// <value>The second layer's colour.</value>
        public Colour Layer2Colour { get; set; }

        /// <summary>
        /// Gets or sets the emblem colour.
        /// </summary>
        /// <value>The emblem colour.</value>
        public Colour EmblemColour { get; set; }

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
            backgroundImage.Location = Location;
            backgroundImage.Size = Size;

            layer1Image.ContentFile = $"Interface/Flags/Layers/{Layer1}";
            layer1Image.MaskFile = $"Interface/Flags/Skins/{Skin}_mask";
            layer1Image.TintColour = Layer1Colour;
            layer1Image.Location = Location;
            layer1Image.Size = Size;

            layer2Image.ContentFile = $"Interface/Flags/Layers/{Layer2}";
            layer2Image.MaskFile = $"Interface/Flags/Skins/{Skin}_mask";
            layer2Image.TintColour = Layer2Colour;
            layer2Image.Location = Location;
            layer2Image.Size = Size;

            emblemImage.ContentFile = $"Interface/Flags/Emblems/{Emblem}";
            emblemImage.MaskFile = backgroundImage.MaskFile;
            emblemImage.TintColour = EmblemColour;
            emblemImage.Location = Location;
            emblemImage.Size = Size;

            skinImage.ContentFile = $"Interface/Flags/Skins/{Skin}";
            skinImage.Location = Location;
            skinImage.Size = Size;
        }
    }
}
