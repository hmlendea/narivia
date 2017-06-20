using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.GameLogic.GameManagers;
using Narivia.Gui;
using Narivia.Gui.GuiElements;
using Narivia.Models;

namespace Narivia.Screens
{
    /// <summary>
    /// New game screen.
    /// </summary>
    public class NewGameScreen : MenuScreen
    {
        GameManager game = new GameManager();
        GuiMenuListSelector factionSelector;
        GuiMenuLink startLink;

        string selectedFactionName;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            // TODO: Don't load everything unnecessarily
            game.NewGame("narivia", "alpalet");

            List<Faction> factions = game.GetFactions().Where(f => f.Id != "gaia").ToList();

            // TODO: Identify and retrieve the items properly
            factionSelector = ListSelectors.FirstOrDefault(x => x.Text == "Faction");
            startLink = Links.FirstOrDefault(x => x.Text == "Start");

            factionSelector.Values.AddRange(factions.Select(f => f.Name));
            factionSelector.SelectedIndex = 0;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (selectedFactionName != factionSelector.SelectedValue)
            {
                selectedFactionName = factionSelector.SelectedValue;

                string factionId = game.GetFactions().FirstOrDefault(f => f.Name == selectedFactionName).Id;

                startLink.LinkArgs = $"narivia {factionId}";
            }
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
