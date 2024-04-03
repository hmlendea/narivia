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
using System.Drawing;
using NuciXNA.Graphics.Drawing;

namespace Narivia.Gui.Screens
{
    /// <summary>
    /// New game screen.
    /// </summary>
    public class NewGameScreen : NariviaMenuScreen
    {
        GuiText worldName;
        GuiButton previousWorldButton;
        GuiButton nextWorldButton;

        GuiText factionName;
        GuiFactionFlag factionFlag;
        GuiButton previousFactionButton;
        GuiButton nextFactionButton;
        GuiText factionDescription;

        GuiDynamicButton startButton;
        GuiDynamicButton backButton;

        IWorldManager worldManager;

        int selectedWorldIndex = 0;
        IList<World> worlds;

        int selectedFactionIndex = 0;
        IList<Faction> factions;

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            base.DoLoadContent();

            worldName = new GuiText
            {
                Id = $"{Id}_{nameof(worldName)}",
                FontName = "Button/Menu",
                ForegroundColour = Colour.Gold,
                Size = new Size2D(512, 48)
            };
            previousWorldButton = new GuiButton
            {
                Id = $"{Id}_{nameof(previousWorldButton)}",
                ContentFile = "Interface/Buttons/button-minus",
                Size = new Size2D(24, 24)
            };
            nextWorldButton = new GuiButton
            {
                Id = $"{Id}_{nameof(nextWorldButton)}",
                ContentFile = "Interface/Buttons/button-plus",
                Size = new Size2D(24, 24)
            };

            factionName = new GuiText
            {
                Id = $"{Id}_{nameof(factionName)}",
                FontName = "Button/Menu",
                ForegroundColour = Colour.Gold,
                Size = new Size2D(512, 48)
            };
            factionFlag = new GuiFactionFlag
            {
                Id = $"{Id}_{nameof(factionFlag)}",
                Size = new Size2D(76, 76),
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
            factionDescription = new GuiText
            {
                Id = $"{Id}_{nameof(factionDescription)}",
                FontName = "FactionDescriptionFont",
                ForegroundColour = Colour.Gold,
                HorizontalAlignment = Alignment.Beginning,
                VerticalAlignment = Alignment.Beginning
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

            GuiManager.Instance.RegisterControls(
                worldName,
                previousWorldButton,
                nextWorldButton,
                factionName,
                factionFlag,
                previousFactionButton,
                nextFactionButton,
                factionDescription,
                startButton,
                backButton);

            RegisterEvents();
            LoadSelectedWorld();
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

            LoadSelectedWorld();
            SetChildrenProperties();
        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        void RegisterEvents()
        {
            previousWorldButton.Clicked += OnPreviousWorldButtonClicked;
            nextWorldButton.Clicked += OnNextWorldButtonClicked;
            previousFactionButton.Clicked += OnPreviousFactionButtonClicked;
            nextFactionButton.Clicked += OnNextFactionButtonClicked;
            startButton.Clicked += OnStartButtonClicked;
            backButton.Clicked += OnBackButtonClicked;
        }

        void SetChildrenProperties()
        {
            worldName.Text = worlds[selectedWorldIndex].Name;
            worldName.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - worldName.Size.Width) / 2,
                GameDefines.GuiSpacing);

            previousWorldButton.Location = new Point2D(
                worldName.Location.X - previousWorldButton.Size.Width - 8,
                worldName.Location.Y + worldName.Size.Height / 2 - previousWorldButton.Size.Height / 2);

            nextWorldButton.Location = new Point2D(
                worldName.Location.X + worldName.Size.Width + 8,
                worldName.Location.Y + worldName.Size.Height / 2 - nextWorldButton.Size.Height / 2);

            factionName.Text = factions[selectedFactionIndex].Name;
            factionName.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - factionName.Size.Width) / 2,
                worldName.Location.Y + factionName.Size.Height + GameDefines.GuiSpacing);

            factionFlag.Flag = worldManager.GetFlag(factions[selectedFactionIndex].FlagId);
            factionFlag.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - factionFlag.Size.Width) / 2,
                factionName.Location.Y + factionName.Size.Height + GameDefines.GuiSpacing);

            previousFactionButton.Location = new Point2D(
                factionName.Location.X - previousFactionButton.Size.Width - 8,
                factionName.Location.Y + factionName.Size.Height / 2 - previousFactionButton.Size.Height / 2);

            nextFactionButton.Location = new Point2D(
                factionName.Location.X + factionName.Size.Width + 8,
                factionName.Location.Y + factionName.Size.Height / 2 - nextFactionButton.Size.Height / 2);

            startButton.Location = new Point2D(
                ScreenManager.Instance.Size.Width - GameDefines.GuiButtonSpacing - startButton.Size.Width,
                ScreenManager.Instance.Size.Height - GameDefines.GuiButtonSpacing - startButton.Size.Height);

            backButton.Location = new Point2D(
                GameDefines.GuiButtonSpacing,
                ScreenManager.Instance.Size.Height - GameDefines.GuiButtonSpacing - backButton.Size.Height);

            factionDescription.Text = factions[selectedFactionIndex].Description;
            factionDescription.Size = new Size2D(
                ScreenManager.Instance.Size.Width - GameDefines.GuiSpacing * 2,
                ScreenManager.Instance.Size.Height - factionFlag.Location.Y - factionFlag.Size.Height - ScreenManager.Instance.Size.Height + startButton.Location.Y - GameDefines.GuiSpacing * 2);
            factionDescription.Location = new Point2D(
                (ScreenManager.Instance.Size.Width - factionDescription.Size.Width) / 2,
                factionFlag.Location.Y + factionFlag.Size.Height + GameDefines.GuiSpacing);
        }

        void LoadSelectedWorld()
        {
            if (worldManager is not null)
            {
                if (worldManager.GetWorld().Id == worlds[selectedWorldIndex].Id)
                {
                    return;
                }

                worldManager.UnloadContent();
            }

            worldManager = new WorldManager(worlds[selectedWorldIndex].Id);
            worldManager.LoadContent();

            factions = worldManager.GetFactions().ToList();
            selectedFactionIndex = 0;
        }

        void OnPreviousWorldButtonClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.Button.Equals(MouseButton.Left))
            {
                selectedWorldIndex -= 1;

                if (selectedWorldIndex < 0)
                {
                    selectedWorldIndex = worlds.Count - 1;
                }
            }
        }

        void OnNextWorldButtonClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.Button.Equals(MouseButton.Left))
            {
                selectedWorldIndex += 1;

                if (selectedWorldIndex >= worlds.Count)
                {
                    selectedWorldIndex = 0;
                }
            }
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
