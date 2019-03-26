using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;
using NuciXNA.Gui.Screens;

namespace Narivia.Gui.Screens
{
    /// <summary>
    /// Title screen.
    /// </summary>
    public class TitleScreen : MenuScreen
    {
        GuiMenuLink newGameLink;
        GuiMenuLink settingsLink;
        GuiMenuItem extiAction;

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            newGameLink = new GuiMenuLink
            {
                Id = nameof(newGameLink),
                Text = "New Game",
                TargetScreen = typeof(NewGameScreen)
            };
            settingsLink = new GuiMenuLink
            {
                Id = nameof(settingsLink),
                Text = "Settings",
                TargetScreen = typeof(SettingsScreen)
            };
            extiAction = new GuiMenuItem
            {
                Id = nameof(extiAction),
                Text = "Exit"
            };

            Items.Add(newGameLink);
            Items.Add(settingsLink);
            Items.Add(extiAction);

            base.DoLoadContent();
        }
    }
}
