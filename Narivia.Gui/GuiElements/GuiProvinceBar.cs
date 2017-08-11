using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics;
using Narivia.Graphics.Enumerations;
using Narivia.Graphics.Geometry;
using Narivia.Graphics.Mapping;
using Narivia.Models;
using Narivia.Settings;

using Province = Narivia.Models.Province;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Province bar GUI element.
    /// </summary>
    public class GuiProvinceBar : GuiElement
    {
        /// <summary>
        /// Gets the province identifier.
        /// </summary>
        /// <value>The province identifier.</value>
        [XmlIgnore]
        public string ProvinceId { get; private set; }

        IGameManager game;

        GuiImage background;

        GuiImage provinceNameBackground;
        GuiImage provinceNameBackgroundDecor;
        GuiText provinceNameText;
        GuiFactionFlag factionFlag;

        GuiImage resourceImage;
        GuiText resourceText;

        List<GuiImage> holdingImages;
        List<GuiText> holdingTexts;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            holdingImages = new List<GuiImage>();
            holdingTexts = new List<GuiText>();

            background = new GuiImage
            {
                ContentFile = "Interface/Backgrounds/wool",
                TextureLayout = TextureLayout.Tile
            };

            provinceNameBackground = new GuiImage
            {
                Size = new Size2D(256, 48),
                ContentFile = "Interface/province-panel-label",
                SourceRectangle = new Rectangle2D(0, 0, 256, 48)
            };
            provinceNameBackgroundDecor = new GuiImage
            {
                Size = provinceNameBackground.Size,
                ContentFile = provinceNameBackground.ContentFile,
                SourceRectangle = new Rectangle2D(0, 48, 256, 48)
            };
            provinceNameText = new GuiText
            {
                FontName = "SideBarFont", // TODO: Consider providing a dedicated font
                Size = new Size2D(provinceNameBackground.SourceRectangle.Width,
                                  provinceNameBackground.SourceRectangle.Height),
                TextOutline = true
            };
            factionFlag = new GuiFactionFlag
            {
                Size = new Size2D(provinceNameBackground.Size.Height,
                                  provinceNameBackground.Size.Height)
            };

            resourceImage = new GuiImage
            {
                SourceRectangle = new Rectangle2D(0, 0, 64, 64)
            };
            resourceText = new GuiText
            {
                FontName = "ProvinceBarHoldingFont",
                ForegroundColour = Colour.Black,
                HorizontalAlignment = HorizontalAlignment.Top
            };

            ProvinceId = game.GetFactionProvinces(game.PlayerFactionId).First().Id;

            Children.Add(background);

            Children.Add(provinceNameBackground);
            Children.Add(provinceNameBackgroundDecor);
            Children.Add(provinceNameText);
            Children.Add(factionFlag);

            Children.Add(resourceImage);
            Children.Add(resourceText);

            base.LoadContent();
        }

        /// <summary>
        /// Sets the province.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        public void SetProvince(string provinceId)
        {
            if (string.IsNullOrWhiteSpace(provinceId) ||
                ProvinceId == provinceId)
            {
                return;
            }

            ProvinceId = provinceId;

            holdingTexts.ForEach(w => w.Dispose());
            holdingImages.ForEach(w => w.Dispose());
            holdingTexts.Clear();
            holdingImages.Clear();

            List<Holding> holdings = game.GetProvinceHoldings(ProvinceId).OrderBy(h => h.Name).ToList();

            holdingImages = new List<GuiImage>();

            foreach (Holding holding in holdings)
            {
                GuiImage holdingImage = new GuiImage
                {
                    ContentFile = $"World/Assets/{game.GetWorld().Id}/holdings/generic",
                    SourceRectangle = new Rectangle2D(64 * ((int)holding.Type - 1), 0, 64, 64),
                    Location = new Point2D(Location.X + GameDefines.GUI_SPACING * (holdingImages.Count + 2) + 64 * (holdingImages.Count + 1),
                                           Location.Y + Size.Height - 64)
                };

                GuiText holdingText = new GuiText
                {
                    Location = new Point2D(holdingImage.Location.X - GameDefines.GUI_SPACING, Location.Y + 2),
                    Text = holding.Name,
                    Size = new Size2D(holdingImage.SourceRectangle.Width + GameDefines.GUI_SPACING * 2,
                                      Size.Height - holdingImage.SourceRectangle.Height + 10),
                    FontName = "ProvinceBarHoldingFont",
                    ForegroundColour = Colour.Black,
                    HorizontalAlignment = HorizontalAlignment.Top
                };

                holdingTexts.Add(holdingText);
                holdingText.LoadContent();
                Children.Add(holdingText);

                holdingImages.Add(holdingImage);
                holdingImage.LoadContent();
                Children.Add(holdingImage);
            }
        }

        // TODO: Handle this better
        /// <summary>
        /// Associates the game manager.
        /// </summary>
        /// <param name="game">Game.</param>
        public void AssociateGameManager(ref IGameManager game)
        {
            this.game = game;
        }

        protected override void SetChildrenProperties()
        {
            background.Location = Location;
            background.Size = Size;

            provinceNameBackground.Location = new Point2D(Location.X + (Size.Width - provinceNameText.ClientRectangle.Width) / 2,
                                                        Location.Y - provinceNameText.ClientRectangle.Height + (int)(provinceNameBackground.Size.Height * 0.1f));
            provinceNameBackgroundDecor.Location = provinceNameBackground.Location;
            provinceNameText.Location = provinceNameBackground.Location;
            provinceNameText.ForegroundColour = ForegroundColour;

            factionFlag.Location = new Point2D(provinceNameBackground.Location.X - factionFlag.ClientRectangle.Width / 2,
                                               provinceNameBackground.Location.Y);
            resourceImage.Location = new Point2D(Location.X + GameDefines.GUI_SPACING,
                                                 Location.Y + Size.Height - 64);
            resourceText.Location = new Point2D(Location.X, Location.Y + 2);
            resourceText.Size = new Size2D(64 + GameDefines.GUI_SPACING * 2,
                                           Size.Height - 74);

            if (string.IsNullOrWhiteSpace(ProvinceId))
            {
                return;
            }

            Province province = game.GetProvince(ProvinceId);
            Resource resource = game.GetResource(province.ResourceId);
            Faction faction = game.GetFaction(province.FactionId);
            Flag flag = game.GetFlag(faction.FlagId);

            provinceNameBackground.TintColour = faction.Colour.ToColour();
            provinceNameText.Text = province.Name;

            factionFlag.Layer1 = flag.Layer1;
            factionFlag.Layer2 = flag.Layer2;
            factionFlag.Emblem = flag.Emblem;
            factionFlag.Skin = flag.Skin;
            factionFlag.BackgroundColour = flag.BackgroundColour.ToColour();
            factionFlag.Layer1Colour = flag.Layer1Colour.ToColour();
            factionFlag.Layer2Colour = flag.Layer2Colour.ToColour();
            factionFlag.EmblemColour = flag.EmblemColour.ToColour();

            resourceImage.ContentFile = $"World/Assets/{game.GetWorld().Id}/resources/{province.ResourceId}_big";
            resourceText.Text = resource.Name;
        }
    }
}
