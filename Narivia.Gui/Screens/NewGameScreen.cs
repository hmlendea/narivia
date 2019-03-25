using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.GuiElements;
using NuciXNA.Gui.Screens;

using Narivia.DataAccess.Repositories;
using Narivia.GameLogic.GameManagers;
using Narivia.GameLogic.Mapping;
using Narivia.Models;
using Narivia.Settings;

namespace Narivia.Gui.Screens
{
    /// <summary>
    /// New game screen.
    /// </summary>
    public class NewGameScreen : MenuScreen
    {
        GuiMenuLink startLink;
        GuiMenuListSelector worldSelector;
        GuiMenuListSelector factionSelector;
        GuiMenuLink backLink;

        IWorldManager worldManager;

        List<World> worlds;

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            startLink = new GuiMenuLink
            {
                Id = nameof(startLink),
                Text = "Start",
                TargetScreen = typeof(GameplayScreen)
            };
            worldSelector = new GuiMenuListSelector
            {
                Id = nameof(worldSelector),
                Text = "World"
            };
            factionSelector = new GuiMenuListSelector
            {
                Id = nameof(factionSelector),
                Text = "Faction"
            };
            backLink = new GuiMenuLink
            {
                Id = nameof(backLink),
                Text = "Back",
                TargetScreen = typeof(TitleScreen)
            };

            // TODO: Do not access the repository directly from here
            WorldRepository worldRepository = new WorldRepository(ApplicationPaths.WorldsDirectory);

            // TODO: Don't load everything unnecessarily
            worlds = worldRepository.GetAll().ToDomainModels().ToList();

            worldSelector.SetItems(worlds.ToDictionary(x => x.Id, x => x.Name));
            
            LoadSelectedWorld();

            RegisterChildren();
            RegisterEvents();
            SetChildrenProperties();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            worldSelector.SelectedItemChanged -= OnWorldSelectorSelectedItemChanged;
            worldManager.UnloadContent();

            UnregisterEvents();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {
            SetChildrenProperties();
        }

        /// <summary>
        /// Draw the content on the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }

        /// <summary>
        /// Registers the children.
        /// </summary>
        void RegisterChildren()
        {
            Items.Add(startLink);
            Items.Add(worldSelector);
            Items.Add(factionSelector);
            Items.Add(backLink);
        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        void RegisterEvents()
        {
            worldSelector.SelectedItemChanged += OnWorldSelectorSelectedItemChanged;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        void UnregisterEvents()
        {
            worldSelector.SelectedItemChanged -= OnWorldSelectorSelectedItemChanged;
        }

        void SetChildrenProperties()
        {
            startLink.Parameters = new object[] { worldSelector.SelectedKey, factionSelector.SelectedKey };
        }

        void LoadSelectedWorld()
        {
            worldManager = new WorldManager(worldSelector.SelectedKey);

            worldManager.LoadContent();
            
            UpdateFactionSelectorItems();
        }

        void UpdateFactionSelectorItems()
        {
            List<Faction> factions = worldManager.GetFactions()
                .Where(f => f.Type.IsActive)
                .OrderBy(f => f.Name)
                .ToList();
            
            factionSelector.SetItems(factions.ToDictionary(x => x.Id, x => x.Name));

            // TODO: Remove this default selection, leave it at 0
            factionSelector.TrySelectItem("Alpalet");
        }

        void OnWorldSelectorSelectedItemChanged(object sender, EventArgs e)
        {
            LoadSelectedWorld();
        }
    }
}
