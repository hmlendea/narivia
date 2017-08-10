using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Narivia.Graphics;
using Narivia.Graphics.Enumerations;
using Narivia.Graphics.Geometry;
using Narivia.Input.Events;
using Narivia.Settings;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Info bar GUI element.
    /// </summary>
    public class GuiInfoBar : GuiElement
    {
        GuiImage background;

        // TODO: Create a GUI Element that contains both the icon and the text

        GuiInfoBarItem regionsItem;
        GuiInfoBarItem holdingsItem;
        GuiInfoBarItem wealthItem;
        GuiInfoBarItem troopsItem;

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
        /// Gets or sets the spacing.
        /// </summary>
        /// <value>The spacing.</value>
        public int Spacing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoBar"/> class.
        /// </summary>
        public GuiInfoBar()
        {
            BackgroundColour = Colour.Black;
            Spacing = GameDefines.GUI_SPACING;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            background = new GuiImage
            {
                ContentFile = "ScreenManager/FillImage",
                TextureLayout = TextureLayout.Tile
            };

            regionsItem = new GuiInfoBarItem
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(0, 0, 16, 16)
            };
            holdingsItem = new GuiInfoBarItem
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(48, 0, 16, 16)
            };
            wealthItem = new GuiInfoBarItem
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(16, 0, 16, 16)
            };
            troopsItem = new GuiInfoBarItem
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle2D(32, 0, 16, 16)
            };

            regionsTooltip = new GuiTooltip
            {
                Text = "Regions",
                Size = new Size2D(100, 20),
                Visible = false
            };
            holdingsTooltip = new GuiTooltip
            {
                Text = "Holdings",
                Size = new Size2D(100, 20),
                Visible = false
            };
            wealthTooltip = new GuiTooltip
            {
                Text = "Wealth",
                Size = new Size2D(100, 20),
                Visible = false
            };
            troopsTooltip = new GuiTooltip
            {
                Text = "Troops",
                Size = new Size2D(128, 128),
                Visible = false
            };

            Children.Add(background);

            Children.Add(regionsItem);
            Children.Add(holdingsItem);
            Children.Add(wealthItem);
            Children.Add(troopsItem);
            
            Children.Add(regionsTooltip);
            Children.Add(holdingsTooltip);
            Children.Add(wealthTooltip);
            Children.Add(troopsTooltip);

            holdingsItem.MouseEntered += OnHoldingsMouseEntered;
            holdingsItem.MouseLeft += OnHoldingsMouseLeft;

            regionsItem.MouseEntered += OnRegionsMouseEntered;
            regionsItem.MouseLeft += OnRegionsMouseLeft;

            troopsItem.MouseEntered += OnTroopsMouseEntered;
            troopsItem.MouseLeft += OnTroopsMouseLeft;

            wealthItem.MouseEntered += OnWealthMouseEntered;
            wealthItem.MouseLeft += OnWealthMouseLeft;

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            holdingsItem.MouseEntered -= OnHoldingsMouseEntered;
            holdingsItem.MouseLeft -= OnHoldingsMouseLeft;

            regionsItem.MouseEntered -= OnRegionsMouseEntered;
            regionsItem.MouseLeft -= OnRegionsMouseLeft;

            troopsItem.MouseEntered -= OnTroopsMouseEntered;
            troopsItem.MouseLeft -= OnTroopsMouseLeft;

            wealthItem.MouseEntered -= OnWealthMouseEntered;
            wealthItem.MouseLeft -= OnWealthMouseLeft;
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            background.Location = Location;
            background.Size = Size;
            background.TintColour = BackgroundColour;

            regionsItem.ForegroundColour = ForegroundColour;
            regionsItem.Location = new Point2D(Location.X + Spacing, Location.Y + (Size.Height - regionsItem.ClientRectangle.Height) / 2);
            regionsItem.Size = new Size2D((Size.Width - Spacing * 3) / 4, 16);
            regionsItem.Text = Regions.ToString();

            holdingsItem.ForegroundColour = ForegroundColour;
            holdingsItem.Location = new Point2D(regionsItem.ClientRectangle.Right + Spacing, regionsItem.Location.Y);
            holdingsItem.Size = new Size2D((Size.Width - Spacing * 3) / 4, 16);
            holdingsItem.Text = Holdings.ToString();

            wealthItem.ForegroundColour = ForegroundColour;
            wealthItem.Location = new Point2D(holdingsItem.ClientRectangle.Right + Spacing, holdingsItem.Location.Y);
            wealthItem.Size = new Size2D((Size.Width - Spacing * 3) / 4, 16);
            wealthItem.Text = Wealth.ToString();

            troopsItem.ForegroundColour = ForegroundColour;
            troopsItem.Location = new Point2D(wealthItem.ClientRectangle.Right + Spacing, wealthItem.Location.Y);
            troopsItem.Size = new Size2D((Size.Width - Spacing * 3) / 4, 16);
            troopsItem.Text = "0";

            regionsTooltip.Location = new Point2D(regionsItem.Location.X, ClientRectangle.Bottom);
            holdingsTooltip.Location = new Point2D(holdingsItem.Location.X, ClientRectangle.Bottom);
            wealthTooltip.Location = new Point2D(wealthItem.Location.X, ClientRectangle.Bottom);
            troopsTooltip.Location = new Point2D(troopsItem.Location.X, ClientRectangle.Bottom);
            
            troopsTooltip.Text = string.Empty;

            if (Troops != null && Troops.Count > 0)
            {
                troopsItem.Text = Troops.Values.Sum().ToString();
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

        void OnRegionsMouseEntered(object sender, MouseEventArgs e)
        {
            regionsTooltip.Show();
        }

        void OnRegionsMouseLeft(object sender, MouseEventArgs e)
        {
            regionsTooltip.Hide();
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
