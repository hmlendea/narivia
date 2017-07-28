using System.Collections.Generic;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// World repository interface.
    /// </summary>
    public interface IWorldRepository
    {
        /// <summary>
        /// Adds the specified world.
        /// </summary>
        /// <param name="worldEntity">World.</param>
        void Add(WorldEntity worldEntity);

        /// <summary>
        /// Gets the world with the specified identifier.
        /// </summary>
        /// <returns>The world.</returns>
        /// <param name="id">Identifier.</param>
        WorldEntity Get(string id);

        /// <summary>
        /// Gets all the worlds.
        /// </summary>
        /// <returns>The worlds.</returns>
        IEnumerable<WorldEntity> GetAll();

        /// <summary>
        /// Updates the specified world.
        /// </summary>
        /// <param name="worldEntity">World.</param>
        void Update(WorldEntity worldEntity);

        /// <summary>
        /// Remove the world with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(string id);
    }
}
