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

        GameManager game;
        List<World> worlds;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
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

        void OnWorldSelectorSelectedIndexChanged(object sender, EventArgs e)
        {
            game = new GameManager(worlds[worldSelector.SelectedIndex].Id);
            game.LoadContent();

            List<Faction> factions = game.GetFactions()
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
        }

        void OnFactionSelectorSelectedIndexChanged(object sender, EventArgs e)
        {
            string factionId = game.GetFactions().ToList()[factionSelector.SelectedIndex].Id;

            startLink.LinkArgs = $"narivia {factionId}";
        }
    }
}
