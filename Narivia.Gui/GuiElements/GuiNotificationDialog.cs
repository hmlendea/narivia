using System;

using Microsoft.Xna.Framework;

using Narivia.Audio;
using Narivia.Gui.GuiElements.Enumerations;
using Narivia.Input.Events;

namespace Narivia.Gui.GuiElements
{
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
        public Vector2 NotificationSize
        {
            get
            {
                return new Vector2((int)Math.Round(Size.X / tileSize),
                                   (int)Math.Round(Size.Y / tileSize));
            }
        }

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

        /// <summary>
        /// Gets or sets the text colour.
        /// </summary>
        /// <value>The text colour.</value>
        public Color TextColour { get; set; }

        GuiImage[,] images;
        GuiImage yesButtonImage;
        GuiImage noButtonImage;

        GuiText title;
        GuiText text;

        const int tileSize = 32;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiNotificationDialog"/> class.
        /// </summary>
        public GuiNotificationDialog()
        {
            Type = NotificationType.Informational;
            Style = NotificationStyle.Big;
            TextColour = Color.Black;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            string imagePath, fontName;

            images = new GuiImage[(int)NotificationSize.X, (int)NotificationSize.Y];

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

            for (int y = 0; y < NotificationSize.Y; y++)
            {
                for (int x = 0; x < NotificationSize.X; x++)
                {
                    images[x, y] = new GuiImage
                    {
                        ContentFile = imagePath,
                        Position = new Vector2(Position.X + x * tileSize, Position.Y + y * tileSize),
                        SourceRectangle = CalculateSourceRectangle(x, y)
                    };

                    Children.Add(images[x, y]);
                }
            }

            yesButtonImage = new GuiImage
            {
                ContentFile = "Interface/notification_controls",
                SourceRectangle = new Rectangle(0, 0, tileSize, tileSize),
                Position = new Vector2(Position.X + (NotificationSize.X - 1) * tileSize, Position.Y)
            };
            yesButtonImage.Clicked += yesButton_OnClicked;
            yesButtonImage.MouseEntered += yesNoButton_OnMouseEntered;

            if (Type == NotificationType.Interogative)
            {
                noButtonImage = new GuiImage
                {
                    ContentFile = "Interface/notification_controls",
                    SourceRectangle = new Rectangle(tileSize, 0, tileSize, tileSize),
                    Position = new Vector2(Position.X, Position.Y)
                };
                noButtonImage.Clicked += noButton_OnClicked;
                noButtonImage.MouseEntered += yesNoButton_OnMouseEntered;

                Children.Add(noButtonImage);
            }

            Children.Add(title);
            Children.Add(text);
            Children.Add(yesButtonImage);

            base.LoadContent();
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            title.Text = Title;
            title.TextColour = TextColour;
            title.Position = new Vector2(Position.X, Position.Y + tileSize);
            title.Size = new Vector2(NotificationSize.X * tileSize, tileSize);

            text.Text = Text;
            text.TextColour = TextColour;
            text.Position = new Vector2(Position.X + tileSize / 2, Position.Y + tileSize * 1.5f);
            text.Size = new Vector2(Size.X - tileSize, Size.Y - title.Size.Y - tileSize * 1.5f);
        }

        Rectangle CalculateSourceRectangle(int x, int y)
        {
            int sx = 1;
            int sy = 1;

            if ((int)NotificationSize.X == 1)
            {
                sx = 3;
            }
            else if (x == 0)
            {
                sx = 0;
            }
            else if (x == (int)NotificationSize.X - 1)
            {
                sx = 2;
            }

            if ((int)NotificationSize.Y == 1)
            {
                sy = 3;
            }
            else if (y == 0)
            {
                sy = 0;
            }
            else if (y == (int)NotificationSize.Y - 1)
            {
                sy = 2;
            }

            return new Rectangle(sx * tileSize, sy * tileSize, tileSize, tileSize);
        }

        void yesButton_OnClicked(object sender, MouseButtonEventArgs e)
        {
            AudioManager.Instance.PlaySound("Interface/click");

            Dispose();
        }

        void noButton_OnClicked(object sender, MouseButtonEventArgs e)
        {
            AudioManager.Instance.PlaySound("Interface/click");

            Dispose();
        }

        void yesNoButton_OnMouseEntered(object sender, MouseEventArgs e)
        {
            AudioManager.Instance.PlaySound("Interface/select");
        }
    }
}
