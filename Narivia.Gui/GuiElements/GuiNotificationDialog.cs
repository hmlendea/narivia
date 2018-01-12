using NuciXNA.Primitives;

using Narivia.Audio;
using Narivia.Gui.GuiElements.Enumerations;
using Narivia.Input.Events;
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
        => new Size2D(Size.Width / GameDefines.GUI_TILE_SIZE,
                      Size.Height / GameDefines.GUI_TILE_SIZE);

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

            title = new GuiText();
            text = new GuiText();

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
                        Location = new Point2D(Location.X + x * GameDefines.GUI_TILE_SIZE,
                                               Location.Y + y * GameDefines.GUI_TILE_SIZE),
                        SourceRectangle = CalculateSourceRectangle(x, y)
                    };

                    Children.Add(images[x, y]);
                }
            }

            yesButtonImage = new GuiImage
            {
                ContentFile = "Interface/notification_controls",
                SourceRectangle = new Rectangle2D(0, 0, GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE),
                Location = new Point2D(Location.X + (NotificationSize.Width - 1) * GameDefines.GUI_TILE_SIZE, Location.Y)
            };

            if (Type == NotificationType.Interogative)
            {
                noButtonImage = new GuiImage
                {
                    ContentFile = "Interface/notification_controls",
                    SourceRectangle = new Rectangle2D(GameDefines.GUI_TILE_SIZE, 0,
                                                      GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE),
                    Location = new Point2D(Location.X, Location.Y)
                };

                Children.Add(noButtonImage);
            }

            Children.Add(title);
            Children.Add(text);
            Children.Add(yesButtonImage);

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
            title.ForegroundColour = ForegroundColour;
            title.Location = new Point2D(Location.X, Location.Y + GameDefines.GUI_TILE_SIZE);
            title.Size = new Size2D(NotificationSize.Width * GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE);

            text.Text = Text;
            text.ForegroundColour = ForegroundColour;
            text.Location = new Point2D(Location.X + GameDefines.GUI_TILE_SIZE / 2,
                                        Location.Y + (int)(GameDefines.GUI_TILE_SIZE * 1.5f));
            text.Size = new Size2D(Size.Width - GameDefines.GUI_TILE_SIZE,
                                   Size.Height - title.Size.Height - (int)(GameDefines.GUI_TILE_SIZE * 1.5f));
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

            return new Rectangle2D(sx * GameDefines.GUI_TILE_SIZE, sy * GameDefines.GUI_TILE_SIZE,
                                   GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE);
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
