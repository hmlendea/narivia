using System.Collections.Generic;

using NuciXNA.DataAccess.Exceptions;
using NuciXNA.DataAccess.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Border repository implementation.
    /// </summary>
    public class RelationRepository : IRepository<string, RelationEntity>
    {
        /// <summary>
        /// Gets or sets the borders.
        /// </summary>
        /// <value>The borders.</value>
        readonly Dictionary<string, RelationEntity> relationEntitiesStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="BorderRepository"/> class.
        /// </summary>
        public RelationRepository()
        {
            relationEntitiesStore = new Dictionary<string, RelationEntity>();
        }

        /// <summary>
        /// Adds the specified relation.
        /// </summary>
        /// <param name="relationEntity">Border.</param>
        public void Add(RelationEntity relationEntity)
        {
            try
            {
                relationEntitiesStore.Add(relationEntity.Id, relationEntity);
            }
            catch
            {
                throw new DuplicateEntityException(relationEntity.Id, nameof(RelationEntity));
            }
        }

        /// <summary>
        /// Get the relation with the specified identifier.
        /// </summary>
        /// <returns>The border.</returns>
        /// <param name="id">Identifier.</param>
        public RelationEntity Get(string id)
        {
            if (!relationEntitiesStore.ContainsKey(id))
            {
                return null;
            }

            RelationEntity relationEntity = relationEntitiesStore[id];

            if (relationEntity == null)
            {
                throw new EntityNotFoundException(id, nameof(RelationEntity));
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
        /// Removes the relation with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            try
            {
                relationEntitiesStore.Remove(id);
            }
            catch
            {
                throw new DuplicateEntityException(id, nameof(RelationEntity));
            }
        }
    }
}
