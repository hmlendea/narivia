using System.Collections.Generic;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface.
    /// </summary>
    public interface IRepository<TKey, TElement>
    {
        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        void Add(TElement entity);

        /// <summary>
        /// Gets the entity  with the specified identifier.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="id">Identifier.</param>
        TElement Get(TKey id);

        /// <summary>
        /// Gets all the entities.
        /// </summary>
        /// <returns>The entities.</returns>
        IEnumerable<TElement> GetAll();

        /// <summary>
        /// Removes the entity with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(TKey id);
    }
}
