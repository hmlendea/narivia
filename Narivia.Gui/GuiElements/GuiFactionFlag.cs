using Microsoft.Xna.Framework;

using Narivia.Graphics;

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
        /// Initializes a new instance of the <see cref="GuiFactionFlag"/> class.
        /// </summary>
        public GuiFactionFlag()
        {
            Size = new Vector2(128, 128);
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            backgroundImage = new GuiImage
            {
                SourceRectangle = new Rectangle(0, 0, 128, 128)
            };
            emblemImage = new GuiImage
            {
                SourceRectangle = new Rectangle(0, 0, 128, 128)
            };
            skinImage = new GuiImage
            {
                SourceRectangle = new Rectangle(0, 0, 128, 128)
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
            backgroundImage.Scale = new Vector2(Size.X / backgroundImage.SourceRectangle.Size.X,
                                                Size.Y / backgroundImage.SourceRectangle.Size.Y);

            emblemImage.ContentFile = $"Interface/Flags/Emblems/{Emblem}";
            emblemImage.MaskFile = $"Interface/Flags/Skins/{Skin}_mask";
            emblemImage.TintColour = EmblemColour;
            emblemImage.Position = Position;
            emblemImage.Scale = new Vector2(Size.X / emblemImage.SourceRectangle.Size.X,
                                            Size.Y / emblemImage.SourceRectangle.Size.Y);

            skinImage.ContentFile = $"Interface/Flags/Skins/{Skin}";
            skinImage.Position = Position;
            skinImage.Scale = new Vector2(Size.X / skinImage.SourceRectangle.Size.X,
                                          Size.Y / skinImage.SourceRectangle.Size.Y);
        }
    }
}
