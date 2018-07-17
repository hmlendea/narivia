using Narivia.GameLogic.Enumerations;
using Narivia.GameLogic.Events;
using Narivia.Models;

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
