using Microsoft.Xna.Framework;
using Narivia.Gui.Controls;
using Narivia.Settings;
using NuciXNA.Gui;
using NuciXNA.Gui.Screens;
using NuciXNA.Input;
using NuciXNA.Primitives;

namespace Narivia.Gui.Screens
{
    /// <summary>
    /// Title screen.
    /// </summary>
    public class TitleScreen : NariviaMenuScreen
    {
        GuiDynamicButton newGameButton;
        GuiDynamicButton settingsButton;
        GuiDynamicButton exitButton;

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            base.DoLoadContent();

            newGameButton = new GuiDynamicButton
            {
                Id = $"{Id}_{nameof(newGameButton)}",
                ContentFile = "Interface/Buttons/Dynamic/red",
                FontName = "Button/Menu",
                ForegroundColour = Colour.Gold,
                Size = ButtonSize,
                Text = "New Game"
            };
            settingsButton = new GuiDynamicButton
            {
                Id = $"{Id}_{nameof(settingsButton)}",
                ContentFile = "Interface/Buttons/Dynamic/red",
                FontName = "Button/Menu",
                ForegroundColour = Colour.Gold,
                Size = ButtonSize,
                Text = "Settings"
            };
            exitButton = new GuiDynamicButton
            {
                Id = $"{Id}_{nameof(exitButton)}",
                ContentFile = "Interface/Buttons/Dynamic/red",
                FontName = "Button/Menu",
                ForegroundColour = Colour.Gold,
                Size = ButtonSize,
                Text = "Exit"
            };

            newGameButton.Clicked += OnNewGameButtonClicked;
            settingsButton.Clicked += OnSettingsButtonClicked;
            exitButton.Clicked += OnExitButtonClicked;

            SetChildrenProperties();

            GuiManager.Instance.RegisterControls(
                newGameButton,
                settingsButton,
                exitButton);
        }

        protected override void DoUpdate(GameTime gameTime)
        {
            base.DoUpdate(gameTime);

            SetChildrenProperties();
        }

        void SetChildrenProperties()
        {
            newGameButton.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - newGameButton.Size.Width) / 2,
                (ScreenManager.Instance.Size.Height - settingsButton.Size.Height) / 2 - GameDefines.GuiButtonSpacing - newGameButton.Size.Height);

            settingsButton.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - settingsButton.Size.Width) / 2,
                (ScreenManager.Instance.Size.Height - settingsButton.Size.Height) / 2);

            exitButton.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - exitButton.Size.Width) / 2,
                (ScreenManager.Instance.Size.Height - settingsButton.Size.Height) / 2 + GameDefines.GuiButtonSpacing + exitButton.Size.Height);
        }

        void OnNewGameButtonClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.Button.Equals(MouseButton.Left))
            {
                ScreenManager.Instance.ChangeScreens<NewGameScreen>();
            }
        }

        void OnSettingsButtonClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.Button.Equals(MouseButton.Left))
            {
                ScreenManager.Instance.ChangeScreens<SettingsScreen>();
            }
        }

        void OnExitButtonClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.Button.Equals(MouseButton.Left))
            {
                GameWindow.Quit();
            }
        }
    }
}
