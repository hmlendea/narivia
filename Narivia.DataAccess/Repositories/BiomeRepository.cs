using System.Linq;

using NuciXNA.DataAccess.Exceptions;
using NuciXNA.DataAccess.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Biome repository implementation.
    /// </summary>
    public class BiomeRepository : XmlRepository<BiomeEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiomeRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public BiomeRepository(string fileName) : base(fileName)
        {

        }
        
        /// <summary>
        /// Updates the specified biome.
        /// </summary>
        /// <param name="biomeEntity">Biome.</param>
        public override void Update(BiomeEntity biomeEntity)
        {
            LoadEntitiesIfNeeded();

            BiomeEntity biomeEntityToUpdate = Entities.FirstOrDefault(x => x.Id == biomeEntity.Id);

            if (biomeEntityToUpdate == null)
            {
                throw new EntityNotFoundException(biomeEntity.Id, nameof(BorderEntity));
            }

            biomeEntityToUpdate.Name = biomeEntity.Name;
            biomeEntityToUpdate.Description = biomeEntity.Description;
            biomeEntityToUpdate.ColourHexadecimal = biomeEntity.ColourHexadecimal;

            XmlFile.SaveEntities(Entities);
        }
    }
}
