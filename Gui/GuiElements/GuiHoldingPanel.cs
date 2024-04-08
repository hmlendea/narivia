using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.Controls;
using NuciXNA.Input;
using NuciXNA.Primitives;

using Narivia.GameLogic.GameManagers;
using Narivia.Models;
using Narivia.Settings;

namespace Narivia.Gui.Controls
{
    public class GuiHoldingPanel : GuiPanel
    {
        readonly IGameManager gameManager;
        readonly IWorldManager worldManager;
        readonly IBuildingManager buildingManager;
        readonly IHoldingManager holdingManager;

        GuiImage paper;

        GuiFactionFlag factionFlag;
        GuiText factionName;

        GuiImage provinceIcon;
        GuiText provinceName;

        GuiButton buildButton;

        List<GuiBuildingCard> cards;

        string currentHoldingId;

        public string HoldingId { get; set; }

        public event MouseButtonEventHandler BuildButtonClicked;

        public GuiHoldingPanel(
            IGameManager gameManager,
            IWorldManager worldManager,
            IBuildingManager buildingManager,
            IHoldingManager holdingManager)
        {
            this.gameManager = gameManager;
            this.worldManager = worldManager;
            this.buildingManager = buildingManager;
            this.holdingManager = holdingManager;
        }

        public override void Close()
        {
            base.Close();

            HoldingId = string.Empty;
            currentHoldingId = string.Empty;
        }

        protected override void DoLoadContent()
        {
            base.DoLoadContent();

            paper = new GuiImage
            {
                Id = $"{Id}_{nameof(paper)}",
                ContentFile = "Interface/ProvincePanel/paper",
                Size = new Size2D(248, 80),
                Location = new Point2D(12, 68)
            };

            factionFlag = new GuiFactionFlag
            {
                Id = $"{Id}_{nameof(factionFlag)}",
                Size = new Size2D(GameDefines.GuiIconSize),
                Location = new Point2D(paper.Location.X  + GameDefines.GuiSpacing * 2, paper.Location.Y + GameDefines.GuiSpacing * 2)
            };
            factionName = new GuiText
            {
                Id = $"{Id}_{nameof(factionName)}",
                Size = new Size2D(80, factionFlag.Size.Height),
                Location = new Point2D(factionFlag.ClientRectangle.Right + GameDefines.GuiSpacing, factionFlag.Location.Y),
                FontName = "ProvincePanelInfoFont",
                HorizontalAlignment = Alignment.Beginning
            };

            provinceIcon = new GuiImage
            {
                Id = $"{Id}_{nameof(provinceIcon)}",
                ContentFile = "Interface/Icons/province",
                Size = new Size2D(GameDefines.GuiIconSize),
                Location = new Point2D(factionFlag.Location.X, factionFlag.ClientRectangle.Bottom + GameDefines.GuiSpacing)
            };
            provinceName = new GuiText
            {
                Id = $"{Id}_{nameof(provinceName)}",
                Size = new Size2D(80, provinceIcon.Size.Height),
                Location = new Point2D(provinceIcon.ClientRectangle.Right + GameDefines.GuiSpacing, provinceIcon.Location.Y),
                FontName = "ProvincePanelInfoFont",
                HorizontalAlignment = Alignment.Beginning
            };

            buildButton = new GuiButton
            {
                Id = $"{Id}_{nameof(buildButton)}",
                ContentFile = "Interface/ProvincePanel/build-button",
                Size = new Size2D(28, 28),
                Location = new Point2D(
                    paper.Location.X + paper.Size.Width - 40,
                    factionFlag.Location.Y)
            };

            PopulateBuildingCards();

            RegisterChild(paper);
            RegisterChildren(factionFlag, factionName);
            RegisterChildren(provinceIcon, provinceName);
            RegisterChildren(buildButton);

            RegisterEvents();
            SetChildrenProperties();
        }

        protected override void DoUnloadContent()
        {
            base.DoUnloadContent();

            UnregisterEvents();
        }

        protected override void DoUpdate(GameTime gameTime)
        {
            base.DoUpdate(gameTime);

            SetChildrenProperties();
        }

        void RegisterEvents()
        {
            buildButton.Clicked += OnBuildButtonClicked;
        }

        void UnregisterEvents()
        {
            buildButton.Clicked -= OnBuildButtonClicked;
        }

        void SetChildrenProperties()
        {
            if (currentHoldingId == HoldingId)
            {
                if (factionFlag.Flag == null)
                {
                    factionFlag.Flag = gameManager.GetFactionFlag("f_gaia"); // TODO: Tempfix
                }

                return;
            }

            currentHoldingId = HoldingId;

            Holding holding = holdingManager.GetHolding(HoldingId);
            Province province = worldManager.GetProvince(holding.ProvinceId);
            Faction faction = worldManager.GetFaction(province.FactionId);

            CrystalColour = faction.Colour;
            Title = holding.Name;

            factionFlag.Flag = gameManager.GetFactionFlag(province.FactionId);
            factionName.Text = faction.Name;

            provinceName.Text = province.Name;

            if (province.FactionId.Equals(gameManager.PlayerFactionId))
            {
                buildButton.Show();
            }
            else
            {
                buildButton.Hide();
            }

            List<Building> buildings = buildingManager.GetHoldingBuildings(holding.Id).ToList();
            for (int i = 0; i < 9; i++)
            {
                if (i < buildings.Count)
                {
                    cards[i].SetHoldingProperties(buildings[i]);
                    cards[i].CultureId = faction.CultureId;
                    cards[i].Show();
                }
                else
                {
                    cards[i].Hide();
                }
            }
        }

        void OnBuildButtonClicked(object sender, MouseButtonEventArgs e)
        {
            BuildButtonClicked?.Invoke(this, e);
            Close();
        }

        void PopulateBuildingCards()
        {
            Point2D cardsStart = new Point2D(16, 161);

            cards = [];

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    GuiBuildingCard card = new ()
                    {
                        Id = $"{Id}_{nameof(card)}_{x}x{y}",
                        Location = cardsStart + new Point2D(x * 84, y * 84)
                    };

                    cards.Add(card);
                }
            }

            RegisterChildren(cards);
        }
    }
}
