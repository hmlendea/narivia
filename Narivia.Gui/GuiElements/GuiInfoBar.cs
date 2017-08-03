using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;

using Narivia.Graphics.Enumerations;
using Narivia.Input.Events;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Info bar GUI element.
    /// </summary>
    public class GuiInfoBar : GuiElement
    {
        GuiImage background;

        GuiImage regionsIcon;
        GuiImage holdingsIcon;
        GuiImage wealthIcon;
        GuiImage troopsIcon;

        GuiText regionsText;
        GuiText holdingsText;
        GuiText wealthText;
        GuiText troopsText;

        GuiTooltip regionsTooltip;
        GuiTooltip holdingsTooltip;
        GuiTooltip wealthTooltip;
        GuiTooltip troopsTooltip;

        /// <summary>
        /// Gets or sets the regions count.
        /// </summary>
        /// <value>The regions count.</value>
        [XmlIgnore]
        public int Regions { get; set; }

        /// <summary>
        /// Gets or sets the holdings count.
        /// </summary>
        /// <value>The holdings count.</value>
        [XmlIgnore]
        public int Holdings { get; set; }

        /// <summary>
        /// Gets or sets the wealth.
        /// </summary>
        /// <value>The wealth.</value>
        [XmlIgnore]
        public int Wealth { get; set; }

        /// <summary>
        /// Gets or sets the troops count.
        /// </summary>
        /// <value>The troops count.</value>
        [XmlIgnore]
        public Dictionary<string, int> Troops { get; set; }

        /// <summary>
        /// Gets or sets the background colour.
        /// </summary>
        /// <value>The background colour.</value>
        public Color BackgroundColour { get; set; }

        /// <summary>
        /// Gets or sets the text colour.
        /// </summary>
        /// <value>The text colour.</value>
        public Color TextColour { get; set; }

        /// <summary>
        /// Gets or sets the spacing.
        /// </summary>
        /// <value>The spacing.</value>
        public int Spacing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.Interface.GUI elements.InfoBar"/> class.
        /// </summary>
        public GuiInfoBar()
        {
            BackgroundColour = Color.Black;
            TextColour = Color.Gold;

            Spacing = 6;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            background = new GuiImage
            {
                ContentFile = "ScreenManager/FillImage",
                SourceRectangle = new Rectangle(0, 0, 1, 1)
            };

            regionsIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle(0, 0, 16, 16)
            };
            holdingsIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle(48, 0, 16, 16)
            };
            wealthIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle(16, 0, 16, 16)
            };
            troopsIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle(32, 0, 16, 16)
            };

            regionsText = new GuiText
            {
                Size = new Point(40, 16),
                VerticalAlignment = VerticalAlignment.Left
            };
            holdingsText = new GuiText
            {
                Size = new Point(60, 16),
                VerticalAlignment = VerticalAlignment.Left
            };
            wealthText = new GuiText
            {
                Size = new Point(60, 16),
                VerticalAlignment = VerticalAlignment.Left
            };
            troopsText = new GuiText
            {
                Size = new Point(60, 16),
                VerticalAlignment = VerticalAlignment.Left
            };

            regionsTooltip = new GuiTooltip
            {
                Text = "Regions",
                Size = new Point(100, 20),
                Visible = false
            };
            holdingsTooltip = new GuiTooltip
            {
                Text = "Holdings",
                Size = new Point(100, 20),
                Visible = false
            };
            wealthTooltip = new GuiTooltip
            {
                Text = "Wealth",
                Size = new Point(100, 20),
                Visible = false
            };
            troopsTooltip = new GuiTooltip
            {
                Text = "Troops",
                Size = new Point(128, 128),
                Visible = false
            };

            Children.Add(background);

            Children.Add(regionsIcon);
            Children.Add(holdingsIcon);
            Children.Add(wealthIcon);
            Children.Add(troopsIcon);

            Children.Add(regionsText);
            Children.Add(holdingsText);
            Children.Add(wealthText);
            Children.Add(troopsText);

            Children.Add(regionsTooltip);
            Children.Add(holdingsTooltip);
            Children.Add(wealthTooltip);
            Children.Add(troopsTooltip);

            base.LoadContent();
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            regionsIcon.Position = new Point(Position.X + Spacing, Position.Y + (Size.Y - regionsIcon.ScreenArea.Height) / 2);
            regionsText.Position = new Point(regionsIcon.ScreenArea.Right + Spacing, Position.Y + (Size.Y - regionsText.ScreenArea.Height) / 2);
            holdingsIcon.Position = new Point(regionsText.ScreenArea.Right + Spacing, regionsIcon.Position.Y);
            holdingsText.Position = new Point(holdingsIcon.ScreenArea.Right + Spacing, regionsText.Position.Y);
            wealthIcon.Position = new Point(holdingsText.ScreenArea.Right + Spacing, holdingsIcon.Position.Y);
            wealthText.Position = new Point(wealthIcon.ScreenArea.Right + Spacing, holdingsText.Position.Y);
            troopsIcon.Position = new Point(wealthText.ScreenArea.Right + Spacing, wealthIcon.Position.Y);
            troopsText.Position = new Point(troopsIcon.ScreenArea.Right + Spacing, wealthText.Position.Y);

            regionsTooltip.Position = new Point(regionsIcon.Position.X, ScreenArea.Bottom);
            holdingsTooltip.Position = new Point(holdingsIcon.Position.X, ScreenArea.Bottom);
            wealthTooltip.Position = new Point(wealthIcon.Position.X, ScreenArea.Bottom);
            troopsTooltip.Position = new Point(troopsIcon.Position.X, ScreenArea.Bottom);

            regionsText.Text = Regions.ToString();
            holdingsText.Text = Holdings.ToString();
            wealthText.Text = Wealth.ToString();
            troopsText.Text = "0";

            troopsTooltip.Text = string.Empty;

            if (Troops != null && Troops.Count > 0)
            {
                troopsText.Text = Troops.Values.Sum().ToString();
                Troops.ToList().ForEach(t => troopsTooltip.Text += $"{t.Key}: {t.Value}\n");
            }

            regionsText.TextColour = TextColour;
            holdingsText.TextColour = TextColour;
            wealthText.TextColour = TextColour;
            troopsText.TextColour = TextColour;

            background.Position = Position;
            background.Size = Size;
            background.TintColour = BackgroundColour;
        }

        /// <summary>
        /// Fired by the MouseMoved event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected override void OnMouseMoved(object sender, MouseEventArgs e)
        {
            base.OnMouseMoved(sender, e);

            if (regionsIcon.ScreenArea.Contains(e.CurrentMousePosition) ||
                regionsText.ScreenArea.Contains(e.CurrentMousePosition))
            {
                regionsTooltip.Visible = true;
            }
            else
            {
                regionsTooltip.Visible = false;
            }

            if (holdingsIcon.ScreenArea.Contains(e.CurrentMousePosition) ||
                holdingsText.ScreenArea.Contains(e.CurrentMousePosition))
            {
                holdingsTooltip.Visible = true;
            }
            else
            {
                holdingsTooltip.Visible = false;
            }

            if (wealthIcon.ScreenArea.Contains(e.CurrentMousePosition) ||
                wealthText.ScreenArea.Contains(e.CurrentMousePosition))
            {
                wealthTooltip.Visible = true;
            }
            else
            {
                wealthTooltip.Visible = false;
            }

            if (troopsIcon.ScreenArea.Contains(e.CurrentMousePosition) ||
                troopsText.ScreenArea.Contains(e.CurrentMousePosition))
            {
                troopsTooltip.Visible = true;
            }
            else
            {
                troopsTooltip.Visible = false;
            }
        }
    }
}
