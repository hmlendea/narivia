using System.Collections.Generic;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Relation repository interface.
    /// </summary>
    public interface IRelationRepository
    {
        /// <summary>
        /// Adds the specified relation.
        /// </summary>
        /// <param name="relationEntity">Border.</param>
        void Add(RelationEntity relationEntity);

        /// <summary>
        /// Get the relation with the specified faction identifiers.
        /// </summary>
        /// <returns>The border.</returns>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        RelationEntity Get(string sourceFactionId, string targetFactionId);

        /// <summary>
        /// Gets all the relations.
        /// </summary>
        /// <returns>The relations.</returns>
        IEnumerable<RelationEntity> GetAll();

        /// <summary>
        /// Removes the relation with the specified faction identifiers.
        /// </summary>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        void Remove(string sourceFactionId, string targetFactionId);
    }
}
