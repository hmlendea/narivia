using NuciXNA.Gui.GuiElements;
using NuciXNA.Input.Events;
using NuciXNA.Primitives;

using Narivia.Audio;
using Narivia.Gui.GuiElements.Enumerations;
using Narivia.Settings;

namespace Narivia.Gui.GuiElements
{
    // TODO: Requires refactoring
    /// <summary>
    /// Notification GUI element.
    /// </summary>
    public class GuiNotificationDialog : GuiElement
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public NotificationType Type { get; set; }

        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        /// <value>The style.</value>
        public NotificationStyle Style { get; set; }

        /// <summary>
        /// Gets or sets the size of the notification.
        /// </summary>
        /// <value>The size of the notification.</value>
        public Size2D NotificationSize
        => new Size2D(Size.Width / GameDefines.GuiTileSize,
                      Size.Height / GameDefines.GuiTileSize);

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }
        
        GuiImage[,] images;
        GuiImage yesButtonImage;
        GuiImage noButtonImage;

        GuiText title;
        GuiText text;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiNotificationDialog"/> class.
        /// </summary>
        public GuiNotificationDialog()
        {
            Type = NotificationType.Informational;
            Style = NotificationStyle.Big;
            ForegroundColour = Colour.Black;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            string imagePath, fontName;

            images = new GuiImage[NotificationSize.Width, NotificationSize.Height];

            title = new GuiText
            {
                Location = new Point2D(0, GameDefines.GuiTileSize)
            };
            text = new GuiText
            {
                Location = new Point2D(GameDefines.GuiTileSize / 2, (int)(GameDefines.GuiTileSize * 1.5f))
            };

            switch (Style)
            {
                default:
                    imagePath = "Interface/notification_big";
                    fontName = "NotificationFontBig";
                    break;

                case NotificationStyle.Small:
                    imagePath = "Interface/notification_small";
                    fontName = "NotificationFontSmall";
                    break;
            }

            title.FontName = "NotificationTitleFontBig";
            text.FontName = fontName;

            for (int y = 0; y < NotificationSize.Height; y++)
            {
                for (int x = 0; x < NotificationSize.Width; x++)
                {
                    images[x, y] = new GuiImage
                    {
                        ContentFile = imagePath,
                        Location = new Point2D(
                            x * GameDefines.GuiTileSize,
                            y * GameDefines.GuiTileSize),
                        SourceRectangle = CalculateSourceRectangle(x, y),
                        Size = new Size2D(32, 32)
                    };

                    AddChild(images[x, y]);
                }
            }

            yesButtonImage = new GuiImage
            {
                ContentFile = "Interface/notification_controls",
                SourceRectangle = new Rectangle2D(0, 0, GameDefines.GuiTileSize, GameDefines.GuiTileSize),
                Location = new Point2D((NotificationSize.Width - 1) * GameDefines.GuiTileSize, 0),
                Size = new Size2D(32, 32)
            };

            if (Type == NotificationType.Interogative)
            {
                noButtonImage = new GuiImage
                {
                    ContentFile = "Interface/notification_controls",
                    SourceRectangle = new Rectangle2D(
                        GameDefines.GuiTileSize, 0,
                        GameDefines.GuiTileSize, GameDefines.GuiTileSize)
                };

                AddChild(noButtonImage);
            }

            AddChild(title);
            AddChild(text);
            AddChild(yesButtonImage);

            base.LoadContent();
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();

            yesButtonImage.Clicked += OnYesButtonClicked;
            yesButtonImage.MouseEntered += OnYesNoButtonEntered;

            if (noButtonImage != null)
            {
                noButtonImage.Clicked += OnNoButtonClicked;
                noButtonImage.MouseEntered += OnYesNoButtonEntered;
            }
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            title.Text = Title;
            title.Size = new Size2D(
                NotificationSize.Width * GameDefines.GuiTileSize,
                GameDefines.GuiTileSize);

            text.Text = Text;
            text.Size = new Size2D(
                Size.Width - GameDefines.GuiTileSize,
                Size.Height - title.Size.Height - (int)(GameDefines.GuiTileSize * 1.5f));
        }

        Rectangle2D CalculateSourceRectangle(int x, int y)
        {
            int sx = 1;
            int sy = 1;

            if (NotificationSize.Width == 1)
            {
                sx = 3;
            }
            else if (x == 0)
            {
                sx = 0;
            }
            else if (x == NotificationSize.Width - 1)
            {
                sx = 2;
            }

            if (NotificationSize.Height == 1)
            {
                sy = 3;
            }
            else if (y == 0)
            {
                sy = 0;
            }
            else if (y == NotificationSize.Height - 1)
            {
                sy = 2;
            }

            return new Rectangle2D(
                sx * GameDefines.GuiTileSize, sy * GameDefines.GuiTileSize,
                GameDefines.GuiTileSize, GameDefines.GuiTileSize);
        }

        void OnYesButtonClicked(object sender, MouseButtonEventArgs e)
        {
            AudioManager.Instance.PlaySound("Interface/click");

            Dispose();
        }

        void OnNoButtonClicked(object sender, MouseButtonEventArgs e)
        {
            AudioManager.Instance.PlaySound("Interface/click");

            Dispose();
        }

        void OnYesNoButtonEntered(object sender, MouseEventArgs e)
        {
            AudioManager.Instance.PlaySound("Interface/select");
        }
    }
}
