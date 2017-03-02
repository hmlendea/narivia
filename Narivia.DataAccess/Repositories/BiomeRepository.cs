using System.Collections.Generic;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Biome repository implementation.
    /// </summary>
    public class BiomeRepository : IBiomeRepository
    {
        readonly XmlDatabase<Biome> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.BiomeRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public BiomeRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<Biome>(fileName);
        }

        /// <summary>
        /// Adds the specified biome.
        /// </summary>
        /// <param name="biome">Biome.</param>
        public void Add(Biome biome)
        {
            xmlDatabase.Add(biome);
        }

        /// <summary>
        /// Get the biome with the specified identifier.
        /// </summary>
        /// <returns>The biome.</returns>
        /// <param name="id">Identifier.</param>
        public Biome Get(string id)
        {
            return xmlDatabase.Get(id);
        }

        /// <summary>
        /// Gets all the biomes.
        /// </summary>
        /// <returns>The biomes</returns>
        public IEnumerable<Biome> GetAll()
        {
            return xmlDatabase.GetAll();
        }

        /// <summary>
        /// Updates the specified biome.
        /// </summary>
        /// <param name="biome">Biome.</param>
        public void Update(Biome biome)
        {
            xmlDatabase.Update(biome);
        }

        /// <summary>
        /// Removes the biome with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            xmlDatabase.Remove(id);
        }
    }
}
