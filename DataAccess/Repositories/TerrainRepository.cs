using NuciDAL.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Terrain repository implementation.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="TerrainRepository"/> class.
    /// </remarks>
    /// <param name="fileName">File name.</param>
    public class TerrainRepository(string fileName) : XmlRepository<TerrainEntity>(fileName)
    {

        /// <summary>
        /// Updates the specified terrain.
        /// </summary>
        /// <param name="entity">Terrain.</param>
        public override void Update(TerrainEntity entity)
        {
            LoadEntities();

            TerrainEntity terrainEntityToUpdate = Get(entity.Id) ?? throw new EntityNotFoundException(entity.Id, nameof(BorderEntity));
            terrainEntityToUpdate.Name = entity.Name;
            terrainEntityToUpdate.Description = entity.Description;
            terrainEntityToUpdate.Spritesheet = entity.Spritesheet;
            terrainEntityToUpdate.ColourHexadecimal = entity.ColourHexadecimal;
            terrainEntityToUpdate.ZIndex = entity.ZIndex;

            SaveChanges();
        }
    }
}
