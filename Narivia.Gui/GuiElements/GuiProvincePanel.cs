using System.Collections.Generic;
using System.Linq;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Input;
using NuciXNA.Primitives;
using NuciXNA.Primitives.Mapping;

using Narivia.GameLogic.GameManagers;
using Narivia.Models;
using Narivia.Settings;

namespace Narivia.Gui.GuiElements
{
    public class GuiProvincePanel : GuiPanel
    {
        readonly IGameManager game;
        
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

        public GuiProvincePanel(IGameManager game)
        {
            this.game = game;
        }

        public override void LoadContent()
        {
            string worldId = game.GetWorld().Id;
            
            paper = new GuiImage
            {
                ContentFile = "Interface/ProvincePanel/paper",
                Size = new Size2D(248, 80),
                Location = new Point2D(12, 68)
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
                HorizontalAlignment = HorizontalAlignment.Left
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
                HorizontalAlignment = HorizontalAlignment.Left
            };

            attackButton = new GuiButton
            {
                ContentFile = "Interface/ProvincePanel/attack-button",
                Size = new Size2D(28, 28),
                Location = new Point2D(
                    paper.Location.X + paper.Size.Width - 40,
                    factionFlag.Location.Y)
            };
            buildButton = new GuiButton
            {
                ContentFile = "Interface/ProvincePanel/build-button",
                Size = new Size2D(28, 28),
                Location = new Point2D(
                    paper.Location.X + paper.Size.Width - 40,
                    resourceIcon.Location.Y)
            };

            Point2D holdingCardsStart = new Point2D(16, 161);
            holdingCards = new List<GuiHoldingCard>();

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    GuiHoldingCard holdingCard = new GuiHoldingCard(game)
                    {
                        Location = new Point2D(
                            holdingCardsStart.X + x * 84,
                            holdingCardsStart.Y + y * 84),
                    };

                    holdingCards.Add(holdingCard);
                }
            }

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void RegisterChildren()
        {
            base.RegisterChildren();
            
            AddChild(paper);

            AddChild(factionFlag);
            AddChild(factionName);

            AddChild(resourceIcon);
            AddChild(resourceName);

            AddChild(attackButton);
            AddChild(buildButton);

            holdingCards.ForEach(AddChild);
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();

            attackButton.Clicked += AttackButton_Clicked;
            buildButton.Clicked += BuildButton_Clicked;
        }

        protected override void UnregisterEvents()
        {
            base.UnregisterEvents();

            attackButton.Clicked -= AttackButton_Clicked;
            buildButton.Clicked -= BuildButton_Clicked;
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

            CrystalColour = faction.Colour.ToColour();
            Title = province.Name;

            factionFlag.Flag = game.GetFactionFlag(province.FactionId);
            factionName.Text = faction.Name;

            resourceIcon.ContentFile = $"World/Assets/{game.WorldId}/resources/{province.ResourceId}";
            resourceName.Text = game.GetResource(province.ResourceId).Name;

            List<Holding> holdings = game.GetProvinceHoldings(ProvinceId).ToList();
            for (int i = 0; i < 9; i++)
            {
                if (i < holdings.Count)
                {
                    holdingCards[i].HoldingId = holdings[i].Id;
                    holdingCards[i].CultureId = faction.CultureId;
                    holdingCards[i].Visible = true;
                }
                else
                {
                    holdingCards[i].Visible = false;
                }
            }
        }

        void AttackButton_Clicked(object sender, MouseButtonEventArgs e)
        {
            AttackButtonClicked?.Invoke(this, e);
        }

        void BuildButton_Clicked(object sender, MouseButtonEventArgs e)
        {
            BuildButtonClicked?.Invoke(this, e);
        }
    }
}
