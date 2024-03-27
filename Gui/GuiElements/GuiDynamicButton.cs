using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;
using NuciXNA.Input;
using NuciXNA.Primitives;

using Narivia.Audio;
using NuciXNA.Graphics.Drawing;

namespace Narivia.Gui.Controls
{
    /// <summary>
    /// Button GUI element.
    /// </summary>
    public class GuiDynamicButton : GuiControl, IGuiControl
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        public string ContentFile { get; set; }

        public int ChunkSize { get; set; }

        GuiImage topLeftChunk;
        GuiImage topSideChunk;
        GuiImage topRightChunk;
        GuiImage bottomLeftChunk;
        GuiImage bottomSideChunk;
        GuiImage bottomRightChunk;
        GuiImage leftSideChunk;
        GuiImage rightSideChunk;
        GuiImage centerChunk;

        GuiText text;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiButton"/> class.
        /// </summary>
        public GuiDynamicButton()
        {
            ChunkSize = 9;
            FontName = "ButtonFont";
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            topLeftChunk = new GuiImage
            {
                Id = $"{Id}_{nameof(topLeftChunk)}",
                ContentFile = ContentFile,
                Size = new Size2D(ChunkSize, ChunkSize)
            };
            topSideChunk = new GuiImage
            {
                Id = $"{Id}_{nameof(topSideChunk)}",
                ContentFile = ContentFile
            };
            topRightChunk = new GuiImage
            {
                Id = $"{Id}_{nameof(topRightChunk)}",
                ContentFile = ContentFile,
                Size = new Size2D(ChunkSize, ChunkSize)
            };
            bottomLeftChunk = new GuiImage
            {
                Id = $"{Id}_{nameof(bottomLeftChunk)}",
                ContentFile = ContentFile,
                Size = new Size2D(ChunkSize, ChunkSize)
            };
            bottomSideChunk = new GuiImage
            {
                Id = $"{Id}_{nameof(bottomSideChunk)}",
                ContentFile = ContentFile
            };
            bottomRightChunk = new GuiImage
            {
                Id = $"{Id}_{nameof(bottomRightChunk)}",
                ContentFile = ContentFile,
                Size = new Size2D(ChunkSize, ChunkSize)
            };
            leftSideChunk = new GuiImage
            {
                Id = $"{Id}_{nameof(leftSideChunk)}",
                ContentFile = ContentFile
            };
            rightSideChunk = new GuiImage
            {
                Id = $"{Id}_{nameof(rightSideChunk)}",
                ContentFile = ContentFile
            };
            centerChunk = new GuiImage
            {
                Id = $"{Id}_{nameof(centerChunk)}",
                ContentFile = ContentFile
            };

            text = new GuiText()
            {
                Id = $"{Id}_{nameof(text)}"
            };

            RegisterChildren(
                topSideChunk, bottomSideChunk, leftSideChunk, rightSideChunk,
                topLeftChunk, topRightChunk, bottomLeftChunk, bottomRightChunk,
                centerChunk, text);
            RegisterEvents();

            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            UnregisterEvents();
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

        void RegisterEvents()
        {
            Clicked += OnClicked;
            MouseEntered += OnMouseEntered;
        }

        void UnregisterEvents()
        {
            Clicked -= OnClicked;
            MouseEntered -= OnMouseEntered;
        }

        void SetChildrenProperties()
        {
            int sourceRectangleOffset = 0;

            if (IsHovered)
            {
                sourceRectangleOffset = ChunkSize * 3;
            }

            topLeftChunk.Location = Point2D.Empty;
            topLeftChunk.SourceRectangle = new Rectangle2D(sourceRectangleOffset, 0, ChunkSize, ChunkSize);

            topSideChunk.Location = new Point2D(ChunkSize, 0);
            topSideChunk.SourceRectangle = new Rectangle2D(sourceRectangleOffset + ChunkSize, 0, ChunkSize, ChunkSize);
            topSideChunk.Size = new Size2D(Size.Width - ChunkSize * 2, ChunkSize);

            topRightChunk.Location = new Point2D(Size.Width - ChunkSize, 0);
            topRightChunk.SourceRectangle = new Rectangle2D(sourceRectangleOffset + ChunkSize * 2, 0, ChunkSize, ChunkSize);

            bottomLeftChunk.Location = new Point2D(0, Size.Height - ChunkSize);
            bottomLeftChunk.SourceRectangle = new Rectangle2D(sourceRectangleOffset, ChunkSize * 2, ChunkSize, ChunkSize);
            bottomLeftChunk.Size = new Size2D(ChunkSize, ChunkSize);

            bottomSideChunk.Location = new Point2D(ChunkSize, Size.Height - ChunkSize);
            bottomSideChunk.SourceRectangle = new Rectangle2D(sourceRectangleOffset + ChunkSize, ChunkSize * 2, ChunkSize, ChunkSize);
            bottomSideChunk.Size = new Size2D(Size.Width - ChunkSize * 2, ChunkSize);

            bottomRightChunk.Location = new Point2D(Size.Width - ChunkSize, Size.Height - ChunkSize);
            bottomRightChunk.SourceRectangle = new Rectangle2D(sourceRectangleOffset + ChunkSize * 2, ChunkSize * 2, ChunkSize, ChunkSize);

            leftSideChunk.Location = new Point2D(0, ChunkSize);
            leftSideChunk.SourceRectangle = new Rectangle2D(sourceRectangleOffset, ChunkSize, ChunkSize, ChunkSize);
            leftSideChunk.Size = new Size2D(ChunkSize, Size.Height - ChunkSize * 2);

            rightSideChunk.Location = new Point2D(Size.Width - ChunkSize, ChunkSize);
            rightSideChunk.SourceRectangle = new Rectangle2D(sourceRectangleOffset + ChunkSize * 2, ChunkSize, ChunkSize, ChunkSize);
            rightSideChunk.Size = new Size2D(ChunkSize, Size.Height - ChunkSize * 2);

            centerChunk.Location = new Point2D(ChunkSize, ChunkSize);
            centerChunk.SourceRectangle = new Rectangle2D(sourceRectangleOffset + ChunkSize, ChunkSize, ChunkSize, ChunkSize);
            centerChunk.Size = new Size2D(Size.Width - ChunkSize * 2, Size.Height - ChunkSize * 2);

            text.Text = Text;
        }

        /// <summary>
        /// Fired by the Clicked event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.Button != MouseButton.Left)
            {
                return;
            }

            AudioManager.Instance.PlaySound("Interface/click");
        }

        /// <summary>
        /// Fired by the MouseEntered event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        void OnMouseEntered(object sender, MouseEventArgs e)
        {
            AudioManager.Instance.PlaySound("Interface/select");
        }
    }
}
