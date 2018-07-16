using System.Collections.Generic;

using Narivia.GameLogic.Enumerations;
using Narivia.GameLogic.Events;
using Narivia.Models;
using Narivia.Models.Enumerations;

namespace Narivia.GameLogic.GameManagers
{
    /// <summary>
    /// Game manager interface.
    /// </summary>
    public interface IGameManager
    {
        /// <summary>
        /// Occurs when a player province is attacked.
        /// </summary>
        event BattleEventHandler PlayerProvinceAttacked;

        /// <summary>
        /// Occurs when a faction died.
        /// </summary>
        event FactionEventHandler FactionDestroyed;

        /// <summary>
        /// Occurs when a faction was revived.
        /// </summary>
        event FactionEventHandler FactionRevived;

        /// <summary>
        /// Occurs a faction won the game.
        /// </summary>
        event FactionEventHandler FactionWon;

        /// <summary>
        /// Gets the world identifier.
        /// </summary>
        /// <value>The world identifier.</value>
        string WorldId { get; }

        /// <summary>
        /// Gets the player faction identifier.
        /// </summary>
        /// <value>The player faction identifier.</value>
        string PlayerFactionId { get; }

        /// <summary>
        /// Gets the turn.
        /// </summary>
        /// <value>The turn.</value>
        int Turn { get; }

        void LoadContent(string worldId, string factionId);

        void UnloadContent();

        /// <summary>
        /// Advances the game by one turn.
        /// </summary>
        void NextTurn();

        /// <summary>
        /// Checks wether the specified provinces share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified provinces share a border, <c>false</c> otherwise.</returns>
        /// <param name="sourceProvinceId">Source province identifier.</param>
        /// <param name="targetProvinceId">Target province identifier.</param>
        bool ProvinceBordersProvince(string sourceProvinceId, string targetProvinceId);

        /// <summary>
        /// Checks wether the specified factions share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified factions share a border, <c>false</c> otherwise.</returns>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        bool FactionBordersFaction(string sourceFactionId, string targetFactionId);

        /// <summary>
        /// Transfers the specified province to the specified faction.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        void TransferProvince(string provinceId, string factionId);

        /// <summary>
        /// Gets the culture of a faction.
        /// </summary>
        /// <returns>The culture.</returns>
        /// <param name="factionId">Faction identifier.</param>
        Culture GetFactionCulture(string factionId);

        /// <summary>
        /// Gets the flag of a factions.
        /// </summary>
        /// <returns>The faction flag.</returns>
        /// <param name="factionId">Faction identifier.</param>
        Flag GetFactionFlag(string factionId);

        /// <summary>
        /// Gets the faction capital.
        /// </summary>
        /// <returns>The faction capital province.</returns>
        /// <param name="factionId">Faction identifier.</param>
        Province GetFactionCapital(string factionId);

        /// <summary>
        /// Gets or sets the X map coordinate of the centre of the faction territoriy.
        /// </summary>
        /// <value>The X coordinate.</value>
        /// <param name="factionId">Faction identifier.</param>
        int GetFactionCentreX(string factionId);

        /// <summary>
        /// Gets or sets the Y map coordinate of the centre of the faction territoriy.
        /// </summary>
        /// <value>The Y coordinate.</value>
        /// <param name="factionId">Faction identifier.</param>
        int GetFactionCentreY(string factionId);

        /// <summary>
        /// Gets the flag.
        /// </summary>
        /// <returns>The flag.</returns>
        /// <param name="flagId">Flag identifier.</param>
        Flag GetFlag(string flagId);

        /// <summary>
        /// Gets the holding.
        /// </summary>
        /// <returns>The holding.</returns>
        /// <param name="holdingId">Holding identifier.</param>
        Holding GetHolding(string holdingId);

        /// <summary>
        /// Gets the terrain.
        /// </summary>
        /// <returns>The terrain.</returns>
        /// <param name="terrainId">Terrain identifier.</param>
        Terrain GetTerrain(string terrainId);

        /// <summary>
        /// Gets the world.
        /// </summary>
        /// <returns>The world.</returns>
        World GetWorld();

        /// <summary>
        /// The player faction will attack the specified province.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        BattleResult PlayerAttackProvince(string provinceId);
    }
}
