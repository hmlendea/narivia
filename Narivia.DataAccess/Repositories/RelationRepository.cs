using System;
using System.Collections.Generic;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Exceptions;
using Narivia.DataAccess.Repositories.Interfaces;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Border repository implementation.
    /// </summary>
    public class RelationRepository : IRelationRepository
    {
        /// <summary>
        /// Gets or sets the borders.
        /// </summary>
        /// <value>The borders.</value>
        readonly Dictionary<Tuple<string, string>, RelationEntity> relationEntitiesStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.BorderRepository"/> class.
        /// </summary>
        public RelationRepository()
        {
            relationEntitiesStore = new Dictionary<Tuple<string, string>, RelationEntity>();
        }

        /// <summary>
        /// Adds the specified relation.
        /// </summary>
        /// <param name="relationEntity">Border.</param>
        public void Add(RelationEntity relationEntity)
        {
            Tuple<string, string> key = new Tuple<string, string>(relationEntity.SourceFactionId, relationEntity.TargetFactionId);

            try
            {
                relationEntitiesStore.Add(key, relationEntity);
            }
            catch
            {
                throw new DuplicateEntityException(
                    $"{relationEntity.SourceFactionId}-{relationEntity.TargetFactionId}",
                    nameof(BorderEntity).Replace("Entity", ""));
            }
        }

        /// <summary>
        /// Get the relation with the specified faction identifiers.
        /// </summary>
        /// <returns>The border.</returns>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        public RelationEntity Get(string sourceFactionId, string targetFactionId)
        {
            Tuple<string, string> key = new Tuple<string, string>(sourceFactionId, targetFactionId);

            if (!relationEntitiesStore.ContainsKey(key))
            {
                return null;
            }

            RelationEntity relationEntity = relationEntitiesStore[key];

            if (relationEntity == null)
            {
                throw new EntityNotFoundException(
                    $"{sourceFactionId}-{targetFactionId}",
                    nameof(BorderEntity).Replace("Entity", ""));
            }

            return relationEntity;
        }

        /// <summary>
        /// Gets all the relations.
        /// </summary>
        /// <returns>The relations.</returns>
        public IEnumerable<RelationEntity> GetAll()
        {
            return relationEntitiesStore.Values;
        }

        /// <summary>
        /// Removes the relation with the specified faction identifiers.
        /// </summary>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        public void Remove(string sourceFactionId, string targetFactionId)
        {
            Tuple<string, string> key = new Tuple<string, string>(sourceFactionId, targetFactionId);

            try
            {
                relationEntitiesStore.Remove(key);
            }
            catch
            {
                throw new DuplicateEntityException(
                    $"{sourceFactionId}-{targetFactionId}",
                    nameof(RelationEntity).Replace("Entity", ""));
            }
        }
    }
}
