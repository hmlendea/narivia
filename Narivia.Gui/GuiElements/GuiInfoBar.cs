using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using NuciXNA.Graphics.Enumerations;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Input.Events;
using NuciXNA.Primitives;

using Narivia.Settings;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Info bar GUI element.
    /// </summary>
    public class GuiInfoBar : GuiElement
    {
        GuiImage background;

        GuiImage provincesIcon;
        GuiImage holdingsIcon;
        GuiImage troopsIcon;
        GuiImage wealthIcon;

        GuiText provincesText;
        GuiText holdingsText;
        GuiText troopsText;
        GuiText wealthText;

        GuiTooltip provincesTooltip;
        GuiTooltip holdingsTooltip;
        GuiTooltip troopsTooltip;
        GuiTooltip wealthTooltip;

        /// <summary>
        /// Gets or sets the provinces count.
        /// </summary>
        /// <value>The provinces count.</value>
        [XmlIgnore]
        public int Provinces { get; set; }

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
        /// Initializes a new instance of the <see cref="InfoBar"/> class.
        /// </summary>
        public GuiInfoBar()
        {
            ForegroundColour = Colour.Gold;
            FontName = "InfoBarFont";
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            background = new GuiImage
            {
                ContentFile = "Interface/InfoBar/bar"
            };

            provincesIcon = new GuiImage
            {
                ContentFile = "Interface/InfoBar/icons",
                SourceRectangle = new Rectangle2D(0, 0, 26, 26),
                Location = new Point2D(16, 10),
                Size = new Size2D(26, 26)
            };
            holdingsIcon = new GuiImage
            {
                ContentFile = "Interface/InfoBar/icons",
                SourceRectangle = new Rectangle2D(26, 0, 26, 26),
                Location = new Point2D(52, 10),
                Size = new Size2D(26, 26)
            };
            troopsIcon = new GuiImage
            {
                ContentFile = "Interface/InfoBar/icons",
                SourceRectangle = new Rectangle2D(52, 0, 26, 26),
                Location = new Point2D(89, 10),
                Size = new Size2D(26, 26)
            };
            wealthIcon = new GuiImage
            {
                ContentFile = "Interface/InfoBar/icons",
                SourceRectangle = new Rectangle2D(78, 0, 26, 26),
                Location = new Point2D(124, 10),
                Size = new Size2D(26, 26)
            };

            provincesText = new GuiText
            {
                Location = new Point2D(13, 36),
                Size = new Size2D(32, 15),
                VerticalAlignment = VerticalAlignment.Centre,
                HorizontalAlignment = HorizontalAlignment.Centre
            };
            holdingsText = new GuiText
            {
                Location = new Point2D(49, 36),
                Size = new Size2D(32, 15),
                VerticalAlignment = VerticalAlignment.Centre,
                HorizontalAlignment = HorizontalAlignment.Centre
            };
            troopsText = new GuiText
            {
                Location = new Point2D(85, 36),
                Size = new Size2D(32, 15),
                VerticalAlignment = VerticalAlignment.Centre,
                HorizontalAlignment = HorizontalAlignment.Centre
            };
            wealthText = new GuiText
            {
                Location = new Point2D(121, 36),
                Size = new Size2D(32, 15),
                VerticalAlignment = VerticalAlignment.Centre,
                HorizontalAlignment = HorizontalAlignment.Centre
            };

            provincesTooltip = new GuiTooltip
            {
                Text = "Provinces",
                Size = new Size2D(100, 20),
                Visible = false
            };
            holdingsTooltip = new GuiTooltip
            {
                Text = "Holdings",
                Size = new Size2D(100, 20),
                Visible = false
            };
            troopsTooltip = new GuiTooltip
            {
                Text = "Troops",
                Size = new Size2D(128, 128),
                Visible = false
            };
            wealthTooltip = new GuiTooltip
            {
                Text = "Wealth",
                Size = new Size2D(100, 20),
                Visible = false
            };

            AddChild(background);

            AddChild(provincesIcon);
            AddChild(holdingsIcon);
            AddChild(troopsIcon);
            AddChild(wealthIcon);

            AddChild(provincesText);
            AddChild(holdingsText);
            AddChild(troopsText);
            AddChild(wealthText);

            AddChild(provincesTooltip);
            AddChild(holdingsTooltip);
            AddChild(wealthTooltip);
            AddChild(troopsTooltip);

            provincesIcon.MouseEntered += OnProvincesMouseEntered;
            provincesIcon.MouseLeft += OnProvincesMouseLeft;

            holdingsIcon.MouseEntered += OnHoldingsMouseEntered;
            holdingsIcon.MouseLeft += OnHoldingsMouseLeft;

            troopsIcon.MouseEntered += OnTroopsMouseEntered;
            troopsIcon.MouseLeft += OnTroopsMouseLeft;

            wealthIcon.MouseEntered += OnWealthMouseEntered;
            wealthIcon.MouseLeft += OnWealthMouseLeft;

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            provincesIcon.MouseEntered -= OnProvincesMouseEntered;
            provincesIcon.MouseLeft -= OnProvincesMouseLeft;

            holdingsIcon.MouseEntered -= OnHoldingsMouseEntered;
            holdingsIcon.MouseLeft -= OnHoldingsMouseLeft;

            troopsIcon.MouseEntered -= OnTroopsMouseEntered;
            troopsIcon.MouseLeft -= OnTroopsMouseLeft;

            wealthIcon.MouseEntered -= OnWealthMouseEntered;
            wealthIcon.MouseLeft -= OnWealthMouseLeft;
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();
            
            provincesText.Text = Provinces.ToString();
            holdingsText.Text = Holdings.ToString();
            troopsText.Text = "0";
            wealthText.Text = Wealth.ToString();

            provincesTooltip.Location = new Point2D(provincesIcon.Location.X, ClientRectangle.Bottom);
            holdingsTooltip.Location = new Point2D(holdingsIcon.Location.X, ClientRectangle.Bottom);
            wealthTooltip.Location = new Point2D(wealthIcon.Location.X, ClientRectangle.Bottom);
            troopsTooltip.Location = new Point2D(troopsIcon.Location.X, ClientRectangle.Bottom);

            troopsTooltip.Text = string.Empty;

            if (Troops != null && Troops.Count > 0)
            {
                troopsText.Text = Troops.Values.Sum().ToString();
                Troops.ToList().ForEach(t => troopsTooltip.Text += $"{t.Key}: {t.Value}\n");
            }
        }

        void OnHoldingsMouseEntered(object sender, MouseEventArgs e)
        {
            holdingsTooltip.Show();
        }

        void OnHoldingsMouseLeft(object sender, MouseEventArgs e)
        {
            holdingsTooltip.Hide();
        }

        void OnProvincesMouseEntered(object sender, MouseEventArgs e)
        {
            provincesTooltip.Show();
        }

        void OnProvincesMouseLeft(object sender, MouseEventArgs e)
        {
            provincesTooltip.Hide();
        }

        void OnTroopsMouseEntered(object sender, MouseEventArgs e)
        {
            troopsTooltip.Show();
        }

        void OnTroopsMouseLeft(object sender, MouseEventArgs e)
        {
            troopsTooltip.Hide();
        }

        void OnWealthMouseEntered(object sender, MouseEventArgs e)
        {
            wealthTooltip.Show();
        }

        void OnWealthMouseLeft(object sender, MouseEventArgs e)
        {
            wealthTooltip.Hide();
        }
    }
}
