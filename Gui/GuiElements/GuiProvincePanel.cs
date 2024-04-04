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
    public class GuiProvincePanel : GuiPanel
    {
        readonly IGameManager gameManager;
        readonly IWorldManager worldManager;
        readonly IHoldingManager holdingManager;

        World world;

        GuiImage paper;

        GuiFactionFlag factionFlag;
        GuiText factionName;

        GuiImage resourceIcon;
        GuiText resourceName;

        GuiButton attackButton;
        GuiButton buildButton;

        List<GuiHoldingCard> holdingCards;

        string currentProvinceId;

        public string ProvinceId { get; set; }

        public event MouseButtonEventHandler AttackButtonClicked;

        public event MouseButtonEventHandler BuildButtonClicked;

        public event MouseButtonEventHandler HoldingCardClicked;

        public GuiProvincePanel(
            IGameManager gameManager,
            IWorldManager worldManager,
            IHoldingManager holdingManager)
        {
            this.gameManager = gameManager;
            this.worldManager = worldManager;
            this.holdingManager = holdingManager;
        }

        protected override void DoLoadContent()
        {
            base.DoLoadContent();

            world = gameManager.GetWorld();

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

            resourceIcon = new GuiImage
            {
                Id = $"{Id}_{nameof(resourceIcon)}",
                ContentFile = $"World/Assets/{world.AssetsPack}/resources/gold",
                Size = new Size2D(GameDefines.GuiIconSize),
                Location = new Point2D(factionFlag.Location.X, factionFlag.ClientRectangle.Bottom + GameDefines.GuiSpacing)
            };
            resourceName = new GuiText
            {
                Id = $"{Id}_{nameof(resourceName)}",
                Size = new Size2D(factionName.Size.Width, resourceIcon.Size.Height),
                Location = new Point2D(
                    resourceIcon.ClientRectangle.Right + GameDefines.GuiSpacing,
                    resourceIcon.Location.Y),
                FontName = "ProvincePanelInfoFont",
                HorizontalAlignment = Alignment.Beginning
            };

            attackButton = new GuiButton
            {
                Id = $"{Id}_{nameof(attackButton)}",
                ContentFile = "Interface/ProvincePanel/attack-button",
                Size = new Size2D(28, 28),
                Location = new Point2D(
                    paper.Location.X + paper.Size.Width - 40,
                    factionFlag.Location.Y)
            };
            buildButton = new GuiButton
            {
                Id = $"{Id}_{nameof(buildButton)}",
                ContentFile = "Interface/ProvincePanel/build-button",
                Size = attackButton.Size,
                Location = attackButton.Location
            };

            Point2D holdingCardsStart = new Point2D(16, 161);
            holdingCards = new List<GuiHoldingCard>();

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    GuiHoldingCard holdingCard = new GuiHoldingCard()
                    {
                        Id = $"{Id}_{nameof(holdingCard)}_{x}x{y}",
                        Location = new Point2D(
                            holdingCardsStart.X + x * 84,
                            holdingCardsStart.Y + y * 84)
                    };

                    holdingCards.Add(holdingCard);
                }
            }

            RegisterChild(paper);
            RegisterChildren(factionFlag, factionName);
            RegisterChildren(resourceIcon, resourceName);
            RegisterChildren(attackButton, buildButton);
            RegisterChildren(holdingCards);

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
            attackButton.Clicked += OnAttackButtonClicked;
            buildButton.Clicked += OnBuildButtonClicked;
            holdingCards.ForEach(x => x.Clicked += OnHoldingCardClicked);
        }

        void UnregisterEvents()
        {
            attackButton.Clicked -= OnAttackButtonClicked;
            buildButton.Clicked -= OnBuildButtonClicked;
            holdingCards.ForEach(x => x.Clicked -= OnHoldingCardClicked);
        }

        void SetChildrenProperties()
        {
            if (currentProvinceId == ProvinceId)
            {
                if (factionFlag.Flag == null)
                {
                    factionFlag.Flag = gameManager.GetFactionFlag("f_gaia"); // TODO: Tempfix
                }

                return;
            }

            currentProvinceId = ProvinceId;

            Province province = worldManager.GetProvince(ProvinceId);
            Faction faction = worldManager.GetFaction(province.FactionId);

            CrystalColour = faction.Colour;
            Title = province.Name;

            factionFlag.Flag = gameManager.GetFactionFlag(province.FactionId);
            factionName.Text = faction.Name;

            resourceIcon.ContentFile = $"World/Assets/{world.AssetsPack}/resources/{province.ResourceId}";
            resourceName.Text = worldManager.GetResource(province.ResourceId).Name;

            if (province.FactionId.Equals(gameManager.PlayerFactionId))
            {
                attackButton.Hide();
                buildButton.Show();
            }
            else
            {
                attackButton.Show();
                buildButton.Hide();
            }

            List<Holding> holdings = holdingManager.GetProvinceHoldings(ProvinceId).ToList();
            for (int i = 0; i < 9; i++)
            {
                if (i < holdings.Count)
                {
                    holdingCards[i].SetHoldingProperties(holdings[i]);
                    holdingCards[i].CultureId = faction.CultureId;
                    holdingCards[i].Show();
                }
                else
                {
                    holdingCards[i].Hide();
                }
            }
        }

        void OnAttackButtonClicked(object sender, MouseButtonEventArgs e)
        {
            AttackButtonClicked?.Invoke(this, e);
        }

        void OnBuildButtonClicked(object sender, MouseButtonEventArgs e)
        {
            BuildButtonClicked?.Invoke(this, e);
        }

        void OnHoldingCardClicked(object sender, MouseButtonEventArgs e)
        {
            foreach (GuiHoldingCard holdingCard in holdingCards)
            {
                Rectangle2D screenClientRectangle = new Rectangle2D(
                    Location + holdingCard.Location,
                    holdingCard.Size);

                if (screenClientRectangle.Contains(e.Location))
                {
                    HoldingCardClicked?.Invoke(holdingCard, e);
                    break;
                }
            }
        }
    }
}
