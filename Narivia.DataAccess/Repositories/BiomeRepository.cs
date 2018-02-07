using NuciXNA.DataAccess.Exceptions;
using NuciXNA.DataAccess.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Terrain repository implementation.
    /// </summary>
    public class TerrainRepository : XmlRepository<TerrainEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public TerrainRepository(string fileName) : base(fileName)
        {

        }
        
        /// <summary>
        /// Updates the specified terrain.
        /// </summary>
        /// <param name="terrainEntity">Terrain.</param>
        public override void Update(TerrainEntity terrainEntity)
        {
            LoadEntitiesIfNeeded();

            TerrainEntity terrainEntityToUpdate = Get(terrainEntity.Id);

            if (terrainEntityToUpdate == null)
            {
                throw new EntityNotFoundException(terrainEntity.Id, nameof(BorderEntity));
            }

            terrainEntityToUpdate.Name = terrainEntity.Name;
            terrainEntityToUpdate.Description = terrainEntity.Description;
            terrainEntityToUpdate.ColourHexadecimal = terrainEntity.ColourHexadecimal;

            XmlFile.SaveEntities(Entities.Values);
        }
    }
}
