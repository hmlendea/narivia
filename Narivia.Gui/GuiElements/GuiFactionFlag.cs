using Microsoft.Xna.Framework;

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
        public Color BackgroundPrimaryColour { get; set; }

        /// <summary>
        /// Gets or sets the background secondary colour.
        /// </summary>
        /// <value>The background secondary colour.</value>
        public Color BackgroundSecondaryColour { get; set; }

        /// <summary>
        /// Gets or sets the emblem colour.
        /// </summary>
        /// <value>The emblem colour.</value>
        public Color EmblemColour { get; set; }

        GuiImage backgroundImage;
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
            emblemImage = new GuiImage
            {
                SourceRectangle = Rectangle.Empty
            };
            skinImage = new GuiImage
            {
                SourceRectangle = Rectangle.Empty
            };

            Children.Add(backgroundImage);
            Children.Add(emblemImage);
            Children.Add(skinImage);

            base.LoadContent();
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            backgroundImage.ContentFile = $"Interface/Flags/Backgrounds/{Background}";
            backgroundImage.MaskFile = $"Interface/Flags/Skins/{Skin}_mask";
            backgroundImage.RedReplacement = BackgroundPrimaryColour;
            backgroundImage.GreenReplacement = BackgroundSecondaryColour;
            backgroundImage.Position = Position;
            backgroundImage.Size = Size;

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
