using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics;
using Narivia.Input.Events;
using Narivia.Models;

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
        GuiText unitText;
        GuiText troopsText;
        List<GuiImage> unitImages;

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

        const int SPACING = 8;

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
            unitImages = new List<GuiImage>();
            units = game.GetUnits().ToList();

            currentUnitIndex = 0;
            troopsAmount = 0;

            background = new GuiImage
            {
                ContentFile = "Interface/backgrounds",
                SourceRectangle = new Rectangle(0, 0, 32, 32),
                FillMode = TextureFillMode.Tile,
                Scale = Size / 32
            };
            unitBackground = new GuiImage
            {
                ContentFile = "ScreenManager/FillImage",
                SourceRectangle = new Rectangle(0, 0, 1, 1),
                Scale = new Vector2(100, 100),
                TintColour = Color.Black
            };

            unitText = new GuiText
            {
                Text = "Militia",
                Size = new Vector2(unitBackground.Scale.X, 18),
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
                Text = "0",
                Size = new Vector2(healthIcon.SourceRectangle.Width * 2, healthIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };
            powerText = new GuiText
            {
                Text = "0",
                Size = new Vector2(powerIcon.SourceRectangle.Width * 2, powerIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };
            priceText = new GuiText
            {
                Text = "0",
                Size = new Vector2(priceIcon.SourceRectangle.Width * 2, priceIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };
            maintenanceText = new GuiText
            {
                Text = "0",
                Size = new Vector2(maintenanceIcon.SourceRectangle.Width * 2, maintenanceIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                VerticalAlignment = VerticalAlignment.Left
            };
            troopsText = new GuiText
            {
                Text = "x1",
                Size = new Vector2(unitBackground.Scale.Y, 18),
                FontName = "InfoBarFont"
            };

            nextButton = new GuiButton
            {
                Text = ">",
                TextColour = TextColour,
                Size = new Vector2(32, 32)
            };
            previousButton = new GuiButton
            {
                Text = "<",
                TextColour = TextColour,
                Size = new Vector2(32, 32)
            };
            plusButton = new GuiButton
            {
                Text = "+",
                TextColour = TextColour,
                Size = new Vector2(32, 32)
            };
            minusButton = new GuiButton
            {
                Text = "-",
                TextColour = TextColour,
                Size = new Vector2(32, 32)
            };
            recruitButton = new GuiButton
            {
                Text = "Recruit",
                TextColour = TextColour,
                Size = new Vector2(128, 32)
            };
            cancelButton = new GuiButton
            {
                Text = "Cancel",
                TextColour = TextColour,
                Size = new Vector2(64, 32)
            };

            foreach (Unit unit in units)
            {
                GuiImage unitImage = new GuiImage
                {
                    ContentFile = $"World/Assets/{game.WorldId}/units/{unit.Id}",
                    SourceRectangle = new Rectangle(0, 0, 64, 64),
                    Visible = false
                };

                unitImages.Add(unitImage);
            }
            unitImages[currentUnitIndex].Visible = true;

            SetChildrenProperties();

            Children.Add(background);
            Children.Add(unitBackground);

            Children.AddRange(unitImages);

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

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            int wealth = game.GetFactionWealth(game.PlayerFactionId);

            if (troopsAmount < 0)
            {
                troopsAmount = 0;
            }
            else if (wealth < units[currentUnitIndex].Price * troopsAmount)
            {
                troopsAmount = wealth / units[currentUnitIndex].Price;
            }

            troopsText.Text = $"x{troopsAmount}";
            recruitButton.Text = $"Recruit ({units[currentUnitIndex].Price * troopsAmount}g)";

            SetChildrenProperties();

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
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

        void SetChildrenProperties()
        {
            background.Position = Position;
            unitBackground.Position = new Vector2(Position.X + (Size.X - unitBackground.Scale.X) / 2, Position.Y + unitText.Size.Y + SPACING);
            unitText.Position = unitBackground.Position;
            troopsText.Position = new Vector2(unitBackground.ScreenArea.Left, unitBackground.ScreenArea.Bottom - troopsText.ScreenArea.Height);

            healthIcon.Position = new Vector2(unitBackground.ScreenArea.Left, unitBackground.ScreenArea.Bottom + SPACING);
            powerIcon.Position = new Vector2(healthIcon.ScreenArea.Left, healthIcon.ScreenArea.Bottom + SPACING);
            priceIcon.Position = new Vector2(powerIcon.ScreenArea.Left, powerIcon.ScreenArea.Bottom + SPACING);
            maintenanceIcon.Position = new Vector2(priceIcon.ScreenArea.Left, priceIcon.ScreenArea.Bottom + SPACING);

            healthText.Position = new Vector2(healthIcon.ScreenArea.Right + SPACING, healthIcon.ScreenArea.Top);
            powerText.Position = new Vector2(powerIcon.ScreenArea.Right + SPACING, powerIcon.ScreenArea.Top);
            priceText.Position = new Vector2(priceIcon.ScreenArea.Right + SPACING, priceIcon.ScreenArea.Top);
            maintenanceText.Position = new Vector2(maintenanceIcon.ScreenArea.Right + SPACING, maintenanceIcon.ScreenArea.Top);

            previousButton.Position = new Vector2(unitBackground.ScreenArea.Left - previousButton.ScreenArea.Width - SPACING, unitBackground.ScreenArea.Top);
            nextButton.Position = new Vector2(unitBackground.ScreenArea.Right + SPACING, unitBackground.ScreenArea.Top);
            plusButton.Position = new Vector2(unitBackground.ScreenArea.Right + SPACING, unitBackground.ScreenArea.Bottom - plusButton.ScreenArea.Height);
            minusButton.Position = new Vector2(unitBackground.ScreenArea.Left - minusButton.ScreenArea.Width - SPACING, unitBackground.ScreenArea.Bottom - minusButton.ScreenArea.Height);

            recruitButton.Position = new Vector2(Position.X + SPACING, Position.Y + Size.Y - recruitButton.Size.Y - SPACING);
            cancelButton.Position = new Vector2(Position.X + Size.X - cancelButton.Size.X - SPACING, Position.Y + Size.Y - recruitButton.Size.Y - SPACING);

            unitImages.ForEach(i => i.Position = new Vector2(unitBackground.Position.X + (unitBackground.Scale.X - i.SourceRectangle.Width) / 2,
                                                             unitBackground.Position.Y + (unitBackground.Scale.Y - i.SourceRectangle.Height) / 2));

            unitText.TextColour = TextColour;
            troopsText.TextColour = TextColour;
            healthText.TextColour = TextColour;
            powerText.TextColour = TextColour;
            priceText.TextColour = TextColour;
            maintenanceText.TextColour = TextColour;
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

            unitText.Text = units[currentUnitIndex].Name;
            healthText.Text = units[currentUnitIndex].Health.ToString();
            powerText.Text = units[currentUnitIndex].Power.ToString();
            priceText.Text = units[currentUnitIndex].Price.ToString();
            maintenanceText.Text = units[currentUnitIndex].Maintenance.ToString();

            unitImages.ForEach(i => i.Visible = false);
            unitImages[currentUnitIndex].Visible = true;
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
            troopsAmount += 1;
        }

        void minus_OnClicked(object sender, MouseButtonEventArgs e)
        {
            troopsAmount -= 1;
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
