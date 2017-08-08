using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;

using Narivia.Graphics.Enumerations;
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
            BackgroundColour = Color.Black;
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
                SourceRectangle = new Rectangle(0, 0, 16, 16)
            };
            holdingsItem = new GuiInfoBarItem
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle(48, 0, 16, 16)
            };
            wealthItem = new GuiInfoBarItem
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle(16, 0, 16, 16)
            };
            troopsItem = new GuiInfoBarItem
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle(32, 0, 16, 16)
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

            background.Position = Position;
            background.Size = Size;
            background.TintColour = BackgroundColour;

            regionsItem.ForegroundColour = ForegroundColour;
            regionsItem.Position = new Point(Position.X + Spacing, Position.Y + (Size.Y - regionsItem.ClientRectangle.Height) / 2);
            regionsItem.Size = new Point((Size.X - Spacing * 3) / 4, 16);
            regionsItem.Text = Regions.ToString();

            holdingsItem.ForegroundColour = ForegroundColour;
            holdingsItem.Position = new Point(regionsItem.ClientRectangle.Right + Spacing, regionsItem.Position.Y);
            holdingsItem.Size = new Point((Size.X - Spacing * 3) / 4, 16);
            holdingsItem.Text = Holdings.ToString();

            wealthItem.ForegroundColour = ForegroundColour;
            wealthItem.Position = new Point(holdingsItem.ClientRectangle.Right + Spacing, holdingsItem.Position.Y);
            wealthItem.Size = new Point((Size.X - Spacing * 3) / 4, 16);
            wealthItem.Text = Wealth.ToString();

            troopsItem.ForegroundColour = ForegroundColour;
            troopsItem.Position = new Point(wealthItem.ClientRectangle.Right + Spacing, wealthItem.Position.Y);
            troopsItem.Size = new Point((Size.X - Spacing * 3) / 4, 16);
            troopsItem.Text = "0";

            regionsTooltip.Position = new Point(regionsItem.Position.X, ClientRectangle.Bottom);
            holdingsTooltip.Position = new Point(holdingsItem.Position.X, ClientRectangle.Bottom);
            wealthTooltip.Position = new Point(wealthItem.Position.X, ClientRectangle.Bottom);
            troopsTooltip.Position = new Point(troopsItem.Position.X, ClientRectangle.Bottom);
            
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
