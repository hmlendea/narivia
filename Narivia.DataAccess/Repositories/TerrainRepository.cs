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
        /// <param name="entity">Terrain.</param>
        public override void Update(TerrainEntity entity)
        {
            LoadEntitiesIfNeeded();

            TerrainEntity terrainEntityToUpdate = Get(entity.Id);

            if (terrainEntityToUpdate == null)
            {
                throw new EntityNotFoundException(entity.Id, nameof(BorderEntity));
            }

            terrainEntityToUpdate.Name = entity.Name;
            terrainEntityToUpdate.Description = entity.Description;
            terrainEntityToUpdate.Spritesheet = entity.Spritesheet;
            terrainEntityToUpdate.ColourHexadecimal = entity.ColourHexadecimal;
            terrainEntityToUpdate.ZIndex = entity.ZIndex;

            XmlFile.SaveEntities(Entities.Values);
        }
    }
}
