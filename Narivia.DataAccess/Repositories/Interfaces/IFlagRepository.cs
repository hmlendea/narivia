using System.Collections.Generic;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Flag repository interface.
    /// </summary>
    public interface IFlagRepository
    {
        /// <summary>
        /// Adds the specified flag.
        /// </summary>
        /// <param name="flagEntity">Flag.</param>
        void Add(FlagEntity flagEntity);

        /// <summary>
        /// Gets the flag with the specified identifier.
        /// </summary>
        /// <returns>The flag.</returns>
        /// <param name="id">Identifier.</param>
        FlagEntity Get(string id);

        /// <summary>
        /// Gets all the flags.
        /// </summary>
        /// <returns>The flags.</returns>
        IEnumerable<FlagEntity> GetAll();

        /// <summary>
        /// Updates the specified flag.
        /// </summary>
        /// <param name="flagEntity">Flag.</param>
        void Update(FlagEntity flagEntity);

        /// <summary>
        /// Remove the flag with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(string id);
    }
}
