using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using NuciXNA.Gui.Controls;
using NuciXNA.Gui.Screens;

using Narivia.DataAccess.Repositories;
using Narivia.GameLogic.GameManagers;
using Narivia.GameLogic.Mapping;
using Narivia.Models;
using Narivia.Settings;
using Narivia.Gui.Controls;
using NuciXNA.Primitives;
using NuciXNA.Gui;
using NuciXNA.Input;

namespace Narivia.Gui.Screens
{
    /// <summary>
    /// New game screen.
    /// </summary>
    public class NewGameScreen : NariviaMenuScreen
    {
        GuiFactionFlag factionFlag;
        GuiText factionName;
        GuiButton previousFactionButton;
        GuiButton nextFactionButton;

        GuiDynamicButton startButton;
        GuiDynamicButton backButton;

        IWorldManager worldManager;

        int selectedFactionIndex = 0;
        IList<Faction> factions;
        IList<World> worlds;

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            base.DoLoadContent();

            factionFlag = new GuiFactionFlag
            {
                Id = $"{Id}_{nameof(factionFlag)}",
                Size = new Size2D(76, 76)
            };
            factionName = new GuiText
            {
                Id = $"{Id}_{nameof(factionName)}",
                FontName = "Button/Menu",
                ForegroundColour = Colour.Gold,
                Size = new Size2D(512, 48)
            };
            previousFactionButton = new GuiButton
            {
                Id = $"{Id}_{nameof(previousFactionButton)}",
                ContentFile = "Interface/Buttons/button-minus",
                Size = new Size2D(24, 24)
            };
            nextFactionButton = new GuiButton
            {
                Id = $"{Id}_{nameof(nextFactionButton)}",
                ContentFile = "Interface/Buttons/button-plus",
                Size = new Size2D(24, 24)
            };

            startButton = new GuiDynamicButton
            {
                Id = $"{Id}_{nameof(startButton)}",
                ContentFile = "Interface/Buttons/Dynamic/red",
                FontName = "Button/Menu",
                ForegroundColour = Colour.Gold,
                Size = ButtonSize,
                Text = "Start"
            };
            backButton = new GuiDynamicButton
            {
                Id = $"{Id}_{nameof(backButton)}",
                ContentFile = "Interface/Buttons/Dynamic/red",
                FontName = "Button/Menu",
                ForegroundColour = Colour.Gold,
                Size = ButtonSize,
                Text = "Back"
            };

            // TODO: Do not access the repository directly from here
            WorldRepository worldRepository = new WorldRepository(ApplicationPaths.WorldsDirectory);

            // TODO: Don't load everything unnecessarily
            worlds = worldRepository.GetAll().ToDomainModels().ToList();

            LoadSelectedWorld();

            GuiManager.Instance.RegisterControls(
                factionFlag,
                factionName,
                previousFactionButton,
                nextFactionButton,
                startButton,
                backButton);

            RegisterEvents();
            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            base.DoUnloadContent();

            worldManager.UnloadContent();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            base.DoUpdate(gameTime);

            SetChildrenProperties();
        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        void RegisterEvents()
        {
            previousFactionButton.Clicked += OnPreviousFactionButtonClicked;
            nextFactionButton.Clicked += OnNextFactionButtonClicked;
            startButton.Clicked += OnStartButtonClicked;
            backButton.Clicked += OnBackButtonClicked;
        }

        void SetChildrenProperties()
        {
            factionFlag.Flag = worldManager.GetFlag(factions[selectedFactionIndex].FlagId);
            factionFlag.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - factionFlag.Size.Width) / 2,
                ControlSpacing);

            factionName.Text = factions[selectedFactionIndex].Name;
            factionName.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - factionName.Size.Width) / 2,
                factionFlag.Location.Y + factionFlag.Size.Height + ControlSpacing);

            previousFactionButton.Location = new Point2D(
                factionFlag.Location.X - previousFactionButton.Size.Width - 8,
                factionFlag.Location.Y + factionFlag.Size.Height / 2 - previousFactionButton.Size.Height / 2);

            nextFactionButton.Location = new Point2D(
                factionFlag.Location.X + factionFlag.Size.Width + 8,
                factionFlag.Location.Y + factionFlag.Size.Height / 2 - nextFactionButton.Size.Height / 2);

            startButton.Location = new Point2D(
                ScreenManager.Instance.Size.Width - ButtonSpacing - startButton.Size.Width,
                ScreenManager.Instance.Size.Height - ButtonSpacing - startButton.Size.Height);

            backButton.Location = new Point2D(
                ButtonSpacing,
                ScreenManager.Instance.Size.Height - ButtonSpacing - backButton.Size.Height);
        }

        void LoadSelectedWorld()
        {
            worldManager = new WorldManager(worlds.First().Id);

            worldManager.LoadContent();

            factions = worldManager.GetFactions().ToList();
            selectedFactionIndex = 0;
        }

        void OnPreviousFactionButtonClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.Button.Equals(MouseButton.Left))
            {
                selectedFactionIndex -= 1;

                if (selectedFactionIndex < 0)
                {
                    selectedFactionIndex = factions.Count - 1;
                }
            }
        }

        void OnNextFactionButtonClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.Button.Equals(MouseButton.Left))
            {
                selectedFactionIndex += 1;

                if (selectedFactionIndex >= factions.Count)
                {
                    selectedFactionIndex = 0;
                }
            }
        }

        void OnStartButtonClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.Button.Equals(MouseButton.Left))
            {
                ScreenManager.Instance.ChangeScreens<GameplayScreen>(worlds.First().Id, factions[selectedFactionIndex].Id);
            }
        }

        void OnBackButtonClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.Button.Equals(MouseButton.Left))
            {
                ScreenManager.Instance.ChangeScreens<TitleScreen>();
            }

        }
    }
}
