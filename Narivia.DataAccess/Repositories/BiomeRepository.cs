using System.Collections.Generic;
using System.Linq;

using NuciXNA.DataAccess.Exceptions;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Repositories.Interfaces;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Biome repository implementation.
    /// </summary>
    public class BiomeRepository : IRepository<string, BiomeEntity>
    {
        readonly XmlDatabase<BiomeEntity> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiomeRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public BiomeRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<BiomeEntity>(fileName);
        }

        /// <summary>
        /// Adds the specified biome.
        /// </summary>
        /// <param name="biomeEntity">Biome.</param>
        public void Add(BiomeEntity biomeEntity)
        {
            List<BiomeEntity> biomeEntities = xmlDatabase.LoadEntities().ToList();
            biomeEntities.Add(biomeEntity);

            try
            {
                xmlDatabase.SaveEntities(biomeEntities);
            }
            catch
            {
                throw new DuplicateEntityException(biomeEntity.Id, nameof(BiomeEntity));
            }
        }

        /// <summary>
        /// Get the biome with the specified identifier.
        /// </summary>
        /// <returns>The biome.</returns>
        /// <param name="id">Identifier.</param>
        public BiomeEntity Get(string id)
        {
            List<BiomeEntity> biomeEntities = xmlDatabase.LoadEntities().ToList();
            BiomeEntity biomeEntity = biomeEntities.FirstOrDefault(x => x.Id == id);

            if (biomeEntity == null)
            {
                throw new EntityNotFoundException(id, nameof(BiomeEntity));
            }

            return biomeEntity;
        }

        /// <summary>
        /// Gets all the biomes.
        /// </summary>
        /// <returns>The biomes</returns>
        public IEnumerable<BiomeEntity> GetAll()
        {
            List<BiomeEntity> biomeEntities = xmlDatabase.LoadEntities().ToList();

            return biomeEntities;
        }

        /// <summary>
        /// Updates the specified biome.
        /// </summary>
        /// <param name="biomeEntity">Biome.</param>
        public void Update(BiomeEntity biomeEntity)
        {
            List<BiomeEntity> biomeEntities = xmlDatabase.LoadEntities().ToList();
            BiomeEntity biomeEntityToUpdate = biomeEntities.FirstOrDefault(x => x.Id == biomeEntity.Id);

            if (biomeEntityToUpdate == null)
            {
                throw new EntityNotFoundException(biomeEntity.Id, nameof(BorderEntity));
            }

            biomeEntityToUpdate.Name = biomeEntity.Name;
            biomeEntityToUpdate.Description = biomeEntity.Description;
            biomeEntityToUpdate.ColourHexadecimal = biomeEntity.ColourHexadecimal;

            xmlDatabase.SaveEntities(biomeEntities);
        }

        /// <summary>
        /// Removes the biome with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            List<BiomeEntity> biomeEntities = xmlDatabase.LoadEntities().ToList();
            biomeEntities.RemoveAll(x => x.Id == id);

            try
            {
                xmlDatabase.SaveEntities(biomeEntities);
            }
            catch
            {
                throw new DuplicateEntityException(id, nameof(BiomeEntity));
            }
        }
    }
}
