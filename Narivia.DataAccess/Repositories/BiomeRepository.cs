using System.Collections.Generic;
using System.Linq;

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
            List<Biome> biomes = xmlDatabase.LoadEntities().ToList();
            biomes.Add(biome);

            xmlDatabase.SaveEntities(biomes);
        }

        /// <summary>
        /// Get the biome with the specified identifier.
        /// </summary>
        /// <returns>The biome.</returns>
        /// <param name="id">Identifier.</param>
        public Biome Get(string id)
        {
            List<Biome> biomes = xmlDatabase.LoadEntities().ToList();
            Biome biome = biomes.FirstOrDefault(x => x.Id == id);

            return biome;
        }

        /// <summary>
        /// Gets all the biomes.
        /// </summary>
        /// <returns>The biomes</returns>
        public IEnumerable<Biome> GetAll()
        {
            List<Biome> biomes = xmlDatabase.LoadEntities().ToList();

            return biomes;
        }

        /// <summary>
        /// Updates the specified biome.
        /// </summary>
        /// <param name="biome">Biome.</param>
        public void Update(Biome biome)
        {
            List<Biome> biomes = xmlDatabase.LoadEntities().ToList();
            Biome biomeToUpdate = biomes.FirstOrDefault(x => x.Id == biome.Id);

            biomeToUpdate.Name = biome.Name;
            biomeToUpdate.Description = biome.Description;
            biomeToUpdate.Colour = biome.Colour;

            xmlDatabase.SaveEntities(biomes);
        }

        /// <summary>
        /// Removes the biome with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            List<Biome> biomes = xmlDatabase.LoadEntities().ToList();
            biomes.RemoveAll(x => x.Id == id);

            xmlDatabase.SaveEntities(biomes);
        }
    }
}
