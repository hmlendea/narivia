using System;
using System.Collections.Generic;
using System.Linq;
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

        IAttackManager attackManager;
        IDiplomacyManager diplomacyManager;
        IEconomyManager economyManager;
        IHoldingManager holdingManager;
        IMilitaryManager militaryManager;
        IWorldManager worldManager;
        IGameManager gameManager;

        List<World> worlds;

        string selectedWorldId;
        string selectedFactionId;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            selectedWorldId = "narivia";
            selectedFactionId = "f_alpalet";

            startLink = new GuiMenuLink
            {
                Id = "start",
                Text = "Start",
                TargetScreen = typeof(GameplayScreen)
            };
            worldSelector = new GuiMenuListSelector
            {
                Id = "worldSelector",
                Text = "World"
            };
            factionSelector = new GuiMenuListSelector
            {
                Id = "factionSelector",
                Text = "Faction"
            };
            backLink = new GuiMenuLink
            {
                Id = "back",
                Text = "Back",
                TargetScreen = typeof(TitleScreen)
            };

            Items.Add(startLink);
            Items.Add(worldSelector);
            Items.Add(factionSelector);
            Items.Add(backLink);

            worldSelector.SelectedIndexChanged += OnWorldSelectorSelectedIndexChanged;
            factionSelector.SelectedIndexChanged += OnFactionSelectorSelectedIndexChanged;

            // TODO: Do not access the repository directly from here
            WorldRepository worldRepository = new WorldRepository(ApplicationPaths.WorldsDirectory);

            // TODO: Don't load everything unnecessarily
            worlds = worldRepository.GetAll().ToDomainModels().ToList();

            worldSelector.Values.AddRange(worlds.Select(f => f.Name));
            worldSelector.SelectedIndex = 0;
            OnWorldSelectorSelectedIndexChanged(this, null); // TODO: This is a hack

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            worldManager.UnloadContent();
            diplomacyManager.UnloadContent();
            holdingManager.UnloadContent();
            militaryManager.UnloadContent();
            economyManager.UnloadContent();
            attackManager.UnloadContent();
            gameManager.UnloadContent();

            base.UnloadContent();
        }

        protected override void SetChildrenProperties()
        {
            startLink.LinkArgs = $"{selectedWorldId} {selectedFactionId}";

            base.SetChildrenProperties();
        }

        void OnWorldSelectorSelectedIndexChanged(object sender, EventArgs e)
        {
            selectedWorldId = worlds[worldSelector.SelectedIndex].Id;

            worldManager = new WorldManager(selectedWorldId);
            diplomacyManager = new DiplomacyManager(worldManager);
            holdingManager = new HoldingManager(selectedWorldId, worldManager);
            militaryManager = new MilitaryManager(holdingManager, worldManager);
            economyManager = new EconomyManager(holdingManager, militaryManager, worldManager);
            attackManager = new AttackManager(diplomacyManager, holdingManager, militaryManager, worldManager);
            gameManager = new GameManager(attackManager, diplomacyManager, economyManager, holdingManager, militaryManager, worldManager);

            worldManager.LoadContent();
            diplomacyManager.LoadContent();
            holdingManager.LoadContent();
            militaryManager.LoadContent();
            economyManager.LoadContent();
            attackManager.LoadContent();
            gameManager.LoadContent(selectedWorldId, selectedFactionId);

            List<Faction> factions = worldManager.GetFactions()
                .Where(f => f.Type.IsActive)
                .OrderBy(f => f.Name)
                .ToList();

            factionSelector.Values.AddRange(factions.Select(f => f.Name));
            factionSelector.SelectedIndex = 0;

            // TODO: Remove this default selection, leave it at 0
            if (factionSelector.Values.Contains("Alpalet"))
            {
                factionSelector.SelectedIndex = factionSelector.Values.IndexOf("Alpalet");
            }

            // TODO: Remove this
            OnFactionSelectorSelectedIndexChanged(this, null);
        }

        void OnFactionSelectorSelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO: Optimise this
            selectedFactionId = worldManager.GetFactions().FirstOrDefault(x => x.Name == factionSelector.SelectedValue).Id;
        }
    }
}
