using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Biome repository interface.
    /// </summary>
    public interface IBiomeRepository
    {
        /// <summary>
        /// Adds the specified biome.
        /// </summary>
        /// <param name="biome">Biome.</param>
        void Add(Biome biome);

        /// <summary>
        /// Get the biome with the specified identifier.
        /// </summary>
        /// <returns>The biome.</returns>
        /// <param name="id">Identifier.</param>
        Biome Get(string id);

        /// <summary>
        /// Gets all the biomes.
        /// </summary>
        /// <returns>The biomes</returns>
        IEnumerable<Biome> GetAll();

        /// <summary>
        /// Updates the specified biome.
        /// </summary>
        /// <param name="biome">Biome.</param>
        void Update(Biome biome);

        /// <summary>
        /// Removes the biome with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(string id);
    }
}
