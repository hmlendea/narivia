using System.Collections.Generic;
using System.Linq;

using NuciXNA.Graphics.Enumerations;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Models;
using Narivia.Settings;

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

        GuiFactionFlag factionFlag;
        GuiText factionName;

        GuiImage resourceIcon;
        GuiText resourceName;

        GuiSimpleButton attackButton;
        GuiSimpleButton buildButton;

        List<GuiImage> holdings;
        List<GuiImage> holdingFrames;

        string currentProvinceId;

        public string ProvinceId { get; set; }

        public GuiProvincePanel(IGameManager game)
        {
            this.game = game;

            Size = new Size2D(274, 424);
        }

        public override void LoadContent()
        {
            string worldId = game.GetWorld().Id;

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
            paper = new GuiImage
            {
                ContentFile = "Interface/ProvincePanel/paper",
                Size = new Size2D(248, 80),
                Location = new Point2D(12, 68)
            };
            closeButton = new GuiSimpleButton
            {
                ContentFile = "Interface/ProvincePanel/close-button",
                Size = new Size2D(28, 28),
                Location = new Point2D(241, 0)
            };
            title = new GuiText
            {
                ForegroundColour = Colour.Gold,
                Size = new Size2D(125, 31),
                Location = new Point2D(76, 4),
                FontName = "ProvincePanelTitleFont",
                VerticalAlignment = VerticalAlignment.Centre,
                HorizontalAlignment = HorizontalAlignment.Centre,
                FontOutline = FontOutline.Around
            };

            factionFlag = new GuiFactionFlag
            {
                Size = new Size2D(28, 28),
                Location = new Point2D(paper.Location.X + 12, paper.Location.Y + 8)
            };
            factionName = new GuiText
            {
                Size = new Size2D(80, factionFlag.Size.Height),
                Location = new Point2D(factionFlag.ClientRectangle.Right + GameDefines.GuiSpacing, factionFlag.Location.Y),
                FontName = "ProvincePanelInfoFont",
                HorizontalAlignment = HorizontalAlignment.Centre,
                VerticalAlignment = VerticalAlignment.Left
            };

            resourceIcon = new GuiImage
            {
                ContentFile = $"World/Assets/{worldId}/resources/gold",
                Size = new Size2D(28, 28),
                Location = new Point2D(paper.Location.X + 12, paper.Location.Y + 44)
            };
            resourceName = new GuiText
            {
                Size = new Size2D(factionName.Size.Width, resourceIcon.Size.Height),
                Location = new Point2D(
                    resourceIcon.ClientRectangle.Right + GameDefines.GuiSpacing,
                    resourceIcon.Location.Y),
                FontName = "ProvincePanelInfoFont",
                HorizontalAlignment = HorizontalAlignment.Centre,
                VerticalAlignment = VerticalAlignment.Left
            };

            attackButton = new GuiSimpleButton
            {
                ContentFile = "Interface/ProvincePanel/attack-button",
                Size = new Size2D(28, 28),
                Location = new Point2D(
                    paper.Location.X + paper.Size.Width - 40,
                    factionFlag.Location.Y)
            };
            buildButton = new GuiSimpleButton
            {
                ContentFile = "Interface/ProvincePanel/build-button",
                Size = new Size2D(28, 28),
                Location = new Point2D(
                    paper.Location.X + paper.Size.Width - 40,
                    resourceIcon.Location.Y)
            };

            holdings = new List<GuiImage>();
            holdingFrames = new List<GuiImage>();

            Point2D holdingsStart = new Point2D(21, 165);
            Point2D holdingFramesStart = new Point2D(16, 161);

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

            AddChild(factionFlag);
            AddChild(factionName);

            AddChild(resourceIcon);
            AddChild(resourceName);

            AddChild(attackButton);
            AddChild(buildButton);

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
            Faction faction = game.GetFaction(province.FactionId);

            crystal.TintColour = faction.Colour.ToColour();
            title.Text = province.Name;

            factionFlag.Flag = game.GetFactionFlag(province.FactionId);
            factionName.Text = faction.Name;

            resourceIcon.ContentFile = $"World/Assets/{game.WorldId}/resources/{province.ResourceId}";
            resourceName.Text = game.GetResource(province.ResourceId).Name;

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
