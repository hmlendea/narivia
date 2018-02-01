using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.GameLogic.GameManagers.Interfaces
{
    public interface IDiplomacyManager
    {
        void LoadContent();

        void UnloadContent();

        /// <summary>
        /// Gets the relations.
        /// </summary>
        /// <returns>The relations.</returns>
        IEnumerable<Relation> GetRelations();

        /// <summary>
        /// Gets the relations of a faction.
        /// </summary>
        /// <returns>The relations of a faction.</returns>
        /// <param name="factionId">Faction identifier.</param>
        IEnumerable<Relation> GetFactionRelations(string factionId);

        /// <summary>
        /// Changes the relations between two factions.
        /// </summary>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        /// <param name="delta">Relations value delta.</param>
        void ChangeRelations(string sourceFactionId, string targetFactionId, int delta);

        /// <summary>
        /// Sets the relations between two factions.
        /// </summary>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        /// <param name="value">Relations value.</param>
        void SetRelations(string sourceFactionId, string targetFactionId, int value);
    }
}
