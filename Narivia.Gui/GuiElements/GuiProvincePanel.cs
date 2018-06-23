using System.Collections.Generic;
using System.Linq;

using NuciXNA.Graphics.Enumerations;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Models;

namespace Narivia.Gui.GuiElements
{
    public class GuiProvincePanel : GuiElement
    {
        readonly IGameManager game;

        GuiImage panel;
        GuiImage crystal;
        GuiImage crystalOverlay;
        GuiImage paper;
        GuiSimpleButton closeButton;
        GuiText title;

        List<GuiImage> holdings;
        List<GuiImage> holdingFrames;

        string currentProvinceId;

        public string ProvinceId { get; set; }

        public GuiProvincePanel(IGameManager game)
        {
            this.game = game;

            Size = new Size2D(274, 424);
            ForegroundColour = Colour.Gold;
        }

        public override void LoadContent()
        {
            panel = new GuiImage
            {
                ContentFile = "Interface/ProvincePanel/panel"
            };
            crystal = new GuiImage
            {
                ContentFile = "Interface/ProvincePanel/crystal",
                Size = new Size2D(125, 31),
                Location = new Point2D(76, 4)
            };
            crystalOverlay = new GuiImage
            {
                ContentFile = "Interface/ProvincePanel/crystal_overlay",
                Size = new Size2D(163, 43),
                Location = new Point2D(55, 0)
            };
            closeButton = new GuiSimpleButton
            {
                ContentFile = "Interface/ProvincePanel/close-button",
                Size = new Size2D(28, 28),
                Location = new Point2D(241, 0)
            };
            paper = new GuiImage
            {
                ContentFile = "Interface/ProvincePanel/paper",
                Size = new Size2D(248, 80),
                Location = new Point2D(12, 68)
            };
            title = new GuiText
            {
                Size = new Size2D(125, 31),
                Location = new Point2D(76, 4),
                FontName = "ProvincePanelTitleFont",
                VerticalAlignment = VerticalAlignment.Centre,
                HorizontalAlignment = HorizontalAlignment.Centre,
                FontOutline = FontOutline.Around
            };

            holdings = new List<GuiImage>();
            holdingFrames = new List<GuiImage>();

            Point2D holdingsStart = new Point2D(21, 165);
            Point2D holdingFramesStart = new Point2D(16, 161);
            string worldId = game.GetWorld().Id;

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    GuiImage holding = new GuiImage
                    {
                        ContentFile = $"World/Assets/{worldId}/holdings/generic",
                        Size = new Size2D(64, 64),
                        Location = new Point2D(
                            holdingsStart.X + x * 84,
                            holdingsStart.Y + y * 84),
                        SourceRectangle = new Rectangle2D(0, 0, 64, 64),
                        Visible = false
                    };

                    GuiImage holdingFrame = new GuiImage
                    {
                        ContentFile = "Interface/ProvincePanel/holding-frame",
                        Size = new Size2D(74, 74),
                        Location = new Point2D(
                            holdingFramesStart.X + x * 84,
                            holdingFramesStart.Y + y * 84),
                        Visible = false
                    };

                    holdings.Add(holding);
                    holdingFrames.Add(holdingFrame);
                }
            }

            AddChild(panel);
            AddChild(crystal);
            AddChild(crystalOverlay);
            AddChild(paper);
            AddChild(closeButton);
            AddChild(title);

            holdings.ForEach(AddChild);
            holdingFrames.ForEach(AddChild);

            base.LoadContent();
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            if (currentProvinceId == ProvinceId)
            {
                return;
            }

            currentProvinceId = ProvinceId;

            Province province = game.GetProvince(ProvinceId);
            string factionId = game.GetProvince(ProvinceId).FactionId;

            crystal.TintColour = game.GetFaction(factionId).Colour.ToColour();
            title.Text = province.Name;

            List<Holding> provinceHoldings = game.GetProvinceHoldings(ProvinceId).ToList();
            for (int i = 0; i < 9; i++)
            {
                if (i >= provinceHoldings.Count)
                {
                    holdings[i].Visible = false;
                    holdingFrames[i].Visible = false;
                }
                else
                {
                    holdings[i].SourceRectangle = new Rectangle2D(64 * ((int)provinceHoldings[i].Type - 1), 0, 64, 64);
                    holdings[i].Visible = true;
                    holdingFrames[i].Visible = true;
                }
            }
        }
    }
}
