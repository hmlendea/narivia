using System.Collections.Generic;

using Narivia.DataAccess.DataObjects;

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
        /// <param name="biomeEntity">Biome.</param>
        void Add(BiomeEntity biomeEntity);

        /// <summary>
        /// Get the biome with the specified identifier.
        /// </summary>
        /// <returns>The biome.</returns>
        /// <param name="id">Identifier.</param>
        BiomeEntity Get(string id);

        /// <summary>
        /// Gets all the biomes.
        /// </summary>
        /// <returns>The biomes</returns>
        IEnumerable<BiomeEntity> GetAll();

        /// <summary>
        /// Updates the specified biome.
        /// </summary>
        /// <param name="biomeEntity">Biome.</param>
        void Update(BiomeEntity biomeEntity);

        /// <summary>
        /// Removes the biome with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(string id);
    }
}
