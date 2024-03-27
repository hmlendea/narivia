
using Microsoft.Xna.Framework;

using Narivia.Settings;
using Narivia.Gui.Controls;
using NuciXNA.Primitives;
using NuciXNA.Gui;
using NuciXNA.Input;
using NuciXNA.Gui.Screens;

namespace Narivia.Gui.Screens
{
    /// <summary>
    /// Settings screen.
    /// </summary>
    public class SettingsScreen : NariviaMenuScreen
    {
        GuiDynamicButton fullScreenButton;
        GuiDynamicButton debugModeButton;
        GuiDynamicButton backButton;

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            base.DoLoadContent();

            fullScreenButton = new GuiDynamicButton
            {
                Id = nameof(fullScreenButton),
                ContentFile = "Interface/Buttons/Dynamic/red",
                FontName = "Button/Menu",
                ForegroundColour = Colour.Gold,
                Size = ButtonSize
            };

            debugModeButton = new GuiDynamicButton
            {
                Id = nameof(debugModeButton),
                ContentFile = "Interface/Buttons/Dynamic/red",
                FontName = "Button/Menu",
                ForegroundColour = Colour.Gold,
                Size = ButtonSize
            };

            backButton = new GuiDynamicButton
            {
                Id = nameof(backButton),
                ContentFile = "Interface/Buttons/Dynamic/red",
                FontName = "Button/Menu",
                ForegroundColour = Colour.Gold,
                Size = ButtonSize,
                Text = "Back"
            };

            RegisterEvents();
            SetChildrenProperties();

            GuiManager.Instance.RegisterControls(
                fullScreenButton,
                debugModeButton,
                backButton);
        }

        protected override void DoUnloadContent()
        {
            SettingsManager.Instance.SaveContent();

            UnregisterEvents();

            base.DoUnloadContent();
        }

        protected override void DoUpdate(GameTime gameTime)
        {
            SetChildrenProperties();

            base.DoUpdate(gameTime);
        }

        void RegisterEvents()
        {
            fullScreenButton.Clicked += OnFullscreenButtonClicked;
            debugModeButton.Clicked += OnDebugModeButtonClicked;
            backButton.Clicked += OnBackButtonClicked;
        }

        void UnregisterEvents()
        {
            fullScreenButton.Clicked -= OnFullscreenButtonClicked;
            debugModeButton.Clicked -= OnDebugModeButtonClicked;
            backButton.Clicked -= OnBackButtonClicked;
        }

        void SetChildrenProperties()
        {
            fullScreenButton.Text = $"Fullscreen: {SettingsManager.Instance.GraphicsSettings.Fullscreen}";
            debugModeButton.Text = $"Debug Mode: {SettingsManager.Instance.DebugMode}";

            fullScreenButton.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - fullScreenButton.Size.Width) / 2,
                (ScreenManager.Instance.Size.Height - debugModeButton.Size.Height) / 2 - ButtonSpacing - fullScreenButton.Size.Height);

            debugModeButton.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - debugModeButton.Size.Width) / 2,
                (ScreenManager.Instance.Size.Height - debugModeButton.Size.Height) / 2);

            backButton.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - backButton.Size.Width) / 2,
                (ScreenManager.Instance.Size.Height - debugModeButton.Size.Height) / 2 + ButtonSpacing + backButton.Size.Height);
        }

        void OnFullscreenButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SettingsManager.Instance.GraphicsSettings.Fullscreen = !SettingsManager.Instance.GraphicsSettings.Fullscreen;
        }

        void OnDebugModeButtonClicked(object sender, MouseButtonEventArgs e)
        {
            SettingsManager.Instance.DebugMode = !SettingsManager.Instance.DebugMode;
        }

        void OnBackButtonClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.Button != MouseButton.Left)
            {
                return;
            }

            ScreenManager.Instance.ChangeScreens<TitleScreen>();
        }
    }
}
