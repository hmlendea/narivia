using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.Audio;
using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics;
using Narivia.Input;
using Narivia.Input.Enumerations;
using Narivia.Input.Events;
using Narivia.Interface.Widgets.Enumerations;
using Narivia.Models;

namespace Narivia.Interface.Widgets
{
    /// <summary>
    /// Unit recruitment dialog.
    /// </summary>
    public class RecruitmentDialog : Widget
    {
        /// <summary>
        /// Gets or sets the text colour.
        /// </summary>
        /// <value>The text colour.</value>
        public Color TextColour { get; set; }

        IGameManager game;

        Image background;
        Image unitBackground;
        Image unitText;
        List<Image> unitImages;

        Image healthIcon;
        Image powerIcon;
        Image priceIcon;

        Image healthText;
        Image powerText;
        Image priceText;
        Image troopsText;

        Button previousButton;
        Button nextButton;
        Button plusButton;
        Button minusButton;

        Button recruitButton;
        Button cancelButton;

        List<Unit> units;

        int currentUnitIndex;
        int troopsAmount;

        const int SPACING = 8;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecruitmentDialog"/> class.
        /// </summary>
        public RecruitmentDialog()
        {
            TextColour = Color.Gold;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            unitImages = new List<Image>();
            units = game.GetUnits().ToList();

            currentUnitIndex = 0;
            troopsAmount = 0;

            background = new Image
            {
                ImagePath = "Interface/backgrounds",
                SourceRectangle = new Rectangle(0, 0, 32, 32),
                Position = Position,
                TextureFillMode = TextureFillMode.Tile,
                Scale = Size / 32
            };
            unitBackground = new Image
            {
                ImagePath = "ScreenManager/FillImage",
                SourceRectangle = new Rectangle(0, 0, 1, 1),
                Position = Position,
                Scale = new Vector2(100, 100),
                Tint = Color.Black
            };

            unitText = new Image
            {
                Text = "Militia",
                SpriteSize = new Vector2(unitBackground.Scale.X, 18),
                FontName = "InfoBarFont",
                TextVerticalAlignment = VerticalAlignment.Center,
                TextHorizontalAlignment = HorizontalAlignment.Center,
                Tint = TextColour
            };

            healthIcon = new Image
            {
                ImagePath = "Interface/game_icons",
                SourceRectangle = new Rectangle(64, 0, 16, 16)
            };
            powerIcon = new Image
            {
                ImagePath = "Interface/game_icons",
                SourceRectangle = new Rectangle(80, 0, 16, 16)
            };
            priceIcon = new Image
            {
                ImagePath = "Interface/game_icons",
                SourceRectangle = new Rectangle(16, 0, 16, 16)
            };

            healthText = new Image
            {
                Text = "0",
                SpriteSize = new Vector2(healthIcon.SourceRectangle.Width * 2, healthIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                TextVerticalAlignment = VerticalAlignment.Left,
                TextHorizontalAlignment = HorizontalAlignment.Center,
                Tint = TextColour
            };
            powerText = new Image
            {
                Text = "0",
                SpriteSize = new Vector2(powerIcon.SourceRectangle.Width * 2, powerIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                TextVerticalAlignment = VerticalAlignment.Left,
                TextHorizontalAlignment = HorizontalAlignment.Center,
                Tint = TextColour
            };
            priceText = new Image
            {
                Text = "0",
                SpriteSize = new Vector2(priceIcon.SourceRectangle.Width * 2, priceIcon.SourceRectangle.Height),
                FontName = "InfoBarFont",
                TextVerticalAlignment = VerticalAlignment.Left,
                TextHorizontalAlignment = HorizontalAlignment.Center,
                Tint = TextColour
            };
            troopsText = new Image
            {
                Text = "x1",
                SpriteSize = new Vector2(unitBackground.Scale.Y, 18),
                FontName = "InfoBarFont",
                TextVerticalAlignment = VerticalAlignment.Center,
                TextHorizontalAlignment = HorizontalAlignment.Center,
                Tint = TextColour
            };

            nextButton = new Button
            {
                Text = ">",
                TextColour = TextColour,
                Size = new Vector2(32, 32)
            };
            previousButton = new Button
            {
                Text = "<",
                TextColour = TextColour,
                Size = new Vector2(32, 32)
            };
            plusButton = new Button
            {
                Text = "+",
                TextColour = TextColour,
                Size = new Vector2(32, 32)
            };
            minusButton = new Button
            {
                Text = "-",
                TextColour = TextColour,
                Size = new Vector2(32, 32)
            };
            recruitButton = new Button
            {
                Text = "Recruit",
                TextColour = TextColour,
                Size = new Vector2(128, 32)
            };
            cancelButton = new Button
            {
                Text = "Cancel",
                TextColour = TextColour,
                Size = new Vector2(64, 32)
            };

            foreach (Unit unit in units)
            {
                Image unitImage = new Image
                {
                    ImagePath = $"World/Assets/{game.WorldId}/units/{unit.Id}",
                    SourceRectangle = new Rectangle(0, 0, 64, 64)
                };

                unitImages.Add(unitImage);
                unitImage.LoadContent();
            }

            SetChildrenPositions();

            background.LoadContent();
            unitBackground.LoadContent();
            unitText.LoadContent();

            healthIcon.LoadContent();
            powerIcon.LoadContent();
            priceIcon.LoadContent();

            healthText.LoadContent();
            powerText.LoadContent();
            priceText.LoadContent();
            troopsText.LoadContent();

            nextButton.LoadContent();
            previousButton.LoadContent();
            plusButton.LoadContent();
            minusButton.LoadContent();
            recruitButton.LoadContent();
            cancelButton.LoadContent();

            base.LoadContent();

            InputManager.Instance.MouseButtonPressed += InputManager_OnMouseButtonPressed;
            InputManager.Instance.MouseMoved += InputManager_OnMouseMoved;

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
            background.UnloadContent();
            unitBackground.UnloadContent();
            unitText.UnloadContent();

            healthIcon.UnloadContent();
            powerIcon.UnloadContent();
            priceIcon.UnloadContent();

            healthText.UnloadContent();
            powerText.UnloadContent();
            priceText.UnloadContent();
            troopsText.UnloadContent();

            nextButton.UnloadContent();
            previousButton.UnloadContent();
            plusButton.UnloadContent();
            minusButton.UnloadContent();
            recruitButton.UnloadContent();
            cancelButton.UnloadContent();

            unitImages.ForEach(i => i.UnloadContent());

            base.UnloadContent();

            InputManager.Instance.MouseButtonPressed -= InputManager_OnMouseButtonPressed;
            InputManager.Instance.MouseMoved -= InputManager_OnMouseMoved;
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enabled)
            {
                return;
            }

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

            SetChildrenPositions();

            background.Update(gameTime);
            unitBackground.Update(gameTime);
            unitText.Update(gameTime);

            healthIcon.Update(gameTime);
            powerIcon.Update(gameTime);
            priceIcon.Update(gameTime);

            healthText.Update(gameTime);
            powerText.Update(gameTime);
            priceText.Update(gameTime);
            troopsText.Update(gameTime);

            nextButton.Update(gameTime);
            previousButton.Update(gameTime);
            plusButton.Update(gameTime);
            minusButton.Update(gameTime);
            recruitButton.Update(gameTime);
            cancelButton.Update(gameTime);

            unitImages[currentUnitIndex].Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }

            background.Draw(spriteBatch);
            unitBackground.Draw(spriteBatch);
            unitText.Draw(spriteBatch);

            healthIcon.Draw(spriteBatch);
            powerIcon.Draw(spriteBatch);
            priceIcon.Draw(spriteBatch);

            healthText.Draw(spriteBatch);
            powerText.Draw(spriteBatch);
            priceText.Draw(spriteBatch);
            troopsText.Draw(spriteBatch);

            nextButton.Draw(spriteBatch);
            previousButton.Draw(spriteBatch);
            plusButton.Draw(spriteBatch);
            minusButton.Draw(spriteBatch);
            recruitButton.Draw(spriteBatch);
            cancelButton.Draw(spriteBatch);

            unitImages[currentUnitIndex].Draw(spriteBatch);

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

        void SetChildrenPositions()
        {
            background.Position = Position;
            unitBackground.Position = new Vector2(Position.X + (Size.X - unitBackground.Scale.X) / 2, Position.Y + unitText.SpriteSize.Y + SPACING);
            unitText.Position = unitBackground.Position;

            healthIcon.Position = new Vector2(unitBackground.Position.X, unitBackground.ScreenArea.Bottom + SPACING);
            powerIcon.Position = new Vector2(healthIcon.Position.X, healthIcon.ScreenArea.Bottom + SPACING);
            priceIcon.Position = new Vector2(powerIcon.Position.X, powerIcon.ScreenArea.Bottom + SPACING);

            healthText.Position = new Vector2(healthIcon.ScreenArea.Right + SPACING, healthIcon.Position.Y);
            powerText.Position = new Vector2(powerIcon.ScreenArea.Right + SPACING, powerIcon.Position.Y);
            priceText.Position = new Vector2(priceIcon.ScreenArea.Right + SPACING, priceIcon.Position.Y);
            troopsText.Position = new Vector2(unitBackground.ScreenArea.Left, unitBackground.ScreenArea.Bottom - troopsText.ScreenArea.Height);

            previousButton.Position = new Vector2(unitBackground.ScreenArea.Left - previousButton.ScreenArea.Width - SPACING, unitBackground.ScreenArea.Top);
            nextButton.Position = new Vector2(unitBackground.ScreenArea.Right + SPACING, unitBackground.ScreenArea.Top);
            plusButton.Position = new Vector2(unitBackground.ScreenArea.Right + SPACING, unitBackground.ScreenArea.Bottom - plusButton.ScreenArea.Height);
            minusButton.Position = new Vector2(unitBackground.ScreenArea.Left - minusButton.ScreenArea.Width - SPACING, unitBackground.ScreenArea.Bottom - minusButton.ScreenArea.Height);

            recruitButton.Position = new Vector2(Position.X + SPACING, Position.Y + Size.Y - recruitButton.Size.Y - SPACING);
            cancelButton.Position = new Vector2(Position.X + Size.X - cancelButton.Size.X - SPACING, Position.Y + Size.Y - recruitButton.Size.Y - SPACING);

            unitImages.ForEach(i => i.Position = new Vector2(unitBackground.Position.X + (unitBackground.Scale.X - i.SourceRectangle.Width) / 2,
                                                             unitBackground.Position.Y + (unitBackground.Scale.Y - i.SourceRectangle.Height) / 2));
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
        }

        void InputManager_OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (!ScreenArea.Contains(e.MousePosition) || e.Button != MouseButton.LeftButton)
            {
                return;
            }

            AudioManager.Instance.PlaySound("Interface/click");
        }

        void InputManager_OnMouseMoved(object sender, MouseEventArgs e)
        {
            if (ScreenArea.Contains(e.CurrentMousePosition) && !ScreenArea.Contains(e.PreviousMousePosition))
            {
                AudioManager.Instance.PlaySound("Interface/select");
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
