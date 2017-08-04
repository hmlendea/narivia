using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics.Enumerations;
using Narivia.Input.Events;
using Narivia.Models;
using Narivia.Settings;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Unit recruitment dialog GUI element.
    /// </summary>
    public class GuiRecruitmentDialog : GuiElement
    {
        /// <summary>
        /// Gets or sets the text colour.
        /// </summary>
        /// <value>The text colour.</value>
        public Color TextColour { get; set; }

        IGameManager game;

        GuiImage background;
        GuiImage unitBackground;
        GuiImage unitImage;
        GuiText unitText;
        GuiText troopsText;

        GuiImage healthIcon;
        GuiImage powerIcon;
        GuiImage priceIcon;
        GuiImage maintenanceIcon;

        GuiText healthText;
        GuiText powerText;
        GuiText priceText;
        GuiText maintenanceText;

        GuiButton previousButton;
        GuiButton nextButton;
        GuiButton plusButton;
        GuiButton minusButton;

        GuiButton recruitButton;
        GuiButton cancelButton;

        List<Unit> units;

        int currentUnitIndex;
        int troopsAmount;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiRecruitmentDialog"/> class.
        /// </summary>
        public GuiRecruitmentDialog()
        {
            TextColour = Color.Gold;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            units = game.GetUnits().OrderBy(u => u.Price).ToList();

            background = new GuiImage
            {
                ContentFile = "Interface/Backgrounds/stone-bricks",
                FillMode = TextureFillMode.Tile
            };
            unitBackground = new GuiImage
            {
                ContentFile = "ScreenManager/FillImage",
                FillMode = TextureFillMode.Tile,
                Size = new Point(100, 100),
                TintColour = Color.Black
            };
            unitImage = new GuiImage
            {
                ContentFile = $"World/Assets/{game.GetWorld().Id}/units/{units[currentUnitIndex].Id}",
                SourceRectangle = new Rectangle(0, 0, 64, 64)
            };

            unitText = new GuiText
            {
                Size = new Point(unitBackground.Size.X, 18),
                FontName = "InfoBarFont"
            };

            healthIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle(64, 0, 16, 16)
            };
            powerIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle(80, 0, 16, 16)
            };
            priceIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle(16, 0, 16, 16)
            };
            maintenanceIcon = new GuiImage
            {
                ContentFile = "Interface/game_icons",
                SourceRectangle = new Rectangle(96, 0, 16, 16)
            };

            healthText = new GuiText
            {
                Size = new Point(healthIcon.SourceRectangle.Width * 2, healthIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };
            powerText = new GuiText
            {
                Size = new Point(powerIcon.SourceRectangle.Width * 2, powerIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };
            priceText = new GuiText
            {
                Size = new Point(priceIcon.SourceRectangle.Width * 2, priceIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };
            maintenanceText = new GuiText
            {
                Size = new Point(maintenanceIcon.SourceRectangle.Width * 2, maintenanceIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };
            troopsText = new GuiText
            {
                Size = new Point(unitBackground.Size.Y, 18),
                FontName = "InfoBarFont"
            };

            nextButton = new GuiButton
            {
                Text = ">",
                Size = new Point(GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };
            previousButton = new GuiButton
            {
                Text = "<",
                Size = new Point(GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };
            plusButton = new GuiButton
            {
                Text = "+",
                Size = new Point(GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };
            minusButton = new GuiButton
            {
                Text = "-",
                Size = new Point(GameDefines.GUI_TILE_SIZE, GameDefines.GUI_TILE_SIZE)
            };
            recruitButton = new GuiButton
            {
                Text = "Recruit",
                Size = new Point(GameDefines.GUI_TILE_SIZE * 4, GameDefines.GUI_TILE_SIZE)
            };
            cancelButton = new GuiButton
            {
                Text = "Cancel",
                Size = new Point(GameDefines.GUI_TILE_SIZE * 2, GameDefines.GUI_TILE_SIZE)
            };

            Children.Add(background);
            Children.Add(unitBackground);
            Children.Add(unitImage);

            Children.Add(unitText);
            Children.Add(troopsText);

            Children.Add(healthIcon);
            Children.Add(powerIcon);
            Children.Add(priceIcon);
            Children.Add(maintenanceIcon);

            Children.Add(healthText);
            Children.Add(powerText);
            Children.Add(priceText);
            Children.Add(maintenanceText);

            Children.Add(nextButton);
            Children.Add(previousButton);
            Children.Add(plusButton);
            Children.Add(minusButton);
            Children.Add(recruitButton);
            Children.Add(cancelButton);

            base.LoadContent();

            previousButton.Clicked += previousButton_OnClicked;
            nextButton.Clicked += nextButton_OnClicked;

            plusButton.Clicked += plusButton_OnClicked;
            minusButton.Clicked += minus_OnClicked;

            recruitButton.Clicked += recruitButton_OnClicked;
            cancelButton.Clicked += cancelButton_OnClicked;
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
            base.SetChildrenProperties();

            background.Position = Position;
            background.Size = Size;

            unitBackground.Position = new Point(Position.X + (Size.X - unitBackground.Size.X) / 2,
                                                Position.Y + unitText.Size.Y + GameDefines.GUI_SPACING);

            unitText.Position = unitBackground.Position;
            unitText.Text = units[currentUnitIndex].Name;
            unitText.TextColour = TextColour;

            troopsText.Position = new Point(unitBackground.ScreenArea.Left, unitBackground.ScreenArea.Bottom - troopsText.ScreenArea.Height);
            troopsText.Text = $"x{troopsAmount}";
            troopsText.TextColour = TextColour;

            healthIcon.Position = new Point(unitBackground.ScreenArea.Left,
                                            unitBackground.ScreenArea.Bottom + GameDefines.GUI_SPACING);
            powerIcon.Position = new Point(healthIcon.ScreenArea.Left,
                                           healthIcon.ScreenArea.Bottom + GameDefines.GUI_SPACING);
            priceIcon.Position = new Point(powerIcon.ScreenArea.Left,
                                           powerIcon.ScreenArea.Bottom + GameDefines.GUI_SPACING);
            maintenanceIcon.Position = new Point(priceIcon.ScreenArea.Left,
                                                 priceIcon.ScreenArea.Bottom + GameDefines.GUI_SPACING);

            healthText.Position = new Point(healthIcon.ScreenArea.Right + GameDefines.GUI_SPACING,
                                            healthIcon.ScreenArea.Top);
            healthText.Text = units[currentUnitIndex].Health.ToString();
            healthText.TextColour = TextColour;

            powerText.Position = new Point(powerIcon.ScreenArea.Right + GameDefines.GUI_SPACING,
                                           powerIcon.ScreenArea.Top);
            powerText.Text = units[currentUnitIndex].Power.ToString();
            powerText.TextColour = TextColour;

            priceText.Position = new Point(priceIcon.ScreenArea.Right + GameDefines.GUI_SPACING,
                                           priceIcon.ScreenArea.Top);
            priceText.Text = units[currentUnitIndex].Price.ToString();
            priceText.TextColour = TextColour;

            maintenanceText.Position = new Point(maintenanceIcon.ScreenArea.Right + GameDefines.GUI_SPACING,
                                                 maintenanceIcon.ScreenArea.Top);
            maintenanceText.Text = units[currentUnitIndex].Maintenance.ToString();
            maintenanceText.TextColour = TextColour;

            previousButton.Position = new Point(unitBackground.ScreenArea.Left - previousButton.ScreenArea.Width - GameDefines.GUI_SPACING,
                                                unitBackground.ScreenArea.Top);
            previousButton.TextColour = TextColour;

            nextButton.Position = new Point(unitBackground.ScreenArea.Right + GameDefines.GUI_SPACING,
                                            unitBackground.ScreenArea.Top);
            nextButton.TextColour = TextColour;

            plusButton.Position = new Point(unitBackground.ScreenArea.Right + GameDefines.GUI_SPACING,
                                            unitBackground.ScreenArea.Bottom - plusButton.ScreenArea.Height);
            plusButton.TextColour = TextColour;

            minusButton.Position = new Point(unitBackground.ScreenArea.Left - minusButton.ScreenArea.Width - GameDefines.GUI_SPACING,
                                             unitBackground.ScreenArea.Bottom - minusButton.ScreenArea.Height);
            minusButton.TextColour = TextColour;

            recruitButton.Text = $"Recruit ({units[currentUnitIndex].Price * troopsAmount}g)";
            recruitButton.Position = new Point(Position.X + GameDefines.GUI_SPACING,
                                               Position.Y + Size.Y - recruitButton.Size.Y - GameDefines.GUI_SPACING);
            cancelButton.Position = new Point(Position.X + Size.X - cancelButton.Size.X - GameDefines.GUI_SPACING,
                                              Position.Y + Size.Y - recruitButton.Size.Y - GameDefines.GUI_SPACING);

            unitImage.ContentFile = $"World/Assets/{game.GetWorld().Id}/units/{units[currentUnitIndex].Id}";
            unitImage.Position = new Point(unitBackground.Position.X + (unitBackground.Size.X - unitImage.SourceRectangle.Width) / 2,
                                           unitBackground.Position.Y + (unitBackground.Size.Y - unitImage.SourceRectangle.Height) / 2);
        }

        void SelectUnit(int index)
        {
            if (index > units.Count - 1)
            {
                currentUnitIndex = 0;
            }
            else if (index < 0)
            {
                currentUnitIndex = units.Count - 1;
            }
            else
            {
                currentUnitIndex = index;
            }
        }

        void AddTroops(int delta)
        {
            int wealth = game.GetFaction(game.PlayerFactionId).Wealth;

            troopsAmount += delta;

            if (troopsAmount < 0)
            {
                troopsAmount = 0;
            }
            else if (wealth < units[currentUnitIndex].Price * troopsAmount)
            {
                troopsAmount = wealth / units[currentUnitIndex].Price;
            }
        }

        void previousButton_OnClicked(object sender, MouseButtonEventArgs e)
        {
            SelectUnit(currentUnitIndex - 1);
        }

        void nextButton_OnClicked(object sender, MouseButtonEventArgs e)
        {
            SelectUnit(currentUnitIndex + 1);
        }

        void plusButton_OnClicked(object sender, MouseButtonEventArgs e)
        {
            AddTroops(1);
        }

        void minus_OnClicked(object sender, MouseButtonEventArgs e)
        {
            AddTroops(-1);
        }

        void recruitButton_OnClicked(object sender, MouseButtonEventArgs e)
        {
            game.RecruitUnits(game.PlayerFactionId, units[currentUnitIndex].Id, troopsAmount);
        }

        void cancelButton_OnClicked(object sender, MouseButtonEventArgs e)
        {
            Hide();
            SelectUnit(0);
            troopsAmount = 0;
        }
    }
}
