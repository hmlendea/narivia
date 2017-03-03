using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Infrastructure.Exceptions;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Region repository implementation.
    /// </summary>
    public class RegionRepository : IRegionRepository
    {
        readonly XmlDatabase<RegionEntity> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.RegionRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public RegionRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<RegionEntity>(fileName);
        }

        /// <summary>
        /// Adds the specified region.
        /// </summary>
        /// <param name="regionEntity">Region.</param>
        public void Add(RegionEntity regionEntity)
        {
            List<RegionEntity> regionEntities = xmlDatabase.LoadEntities().ToList();
            regionEntities.Add(regionEntity);

            try
            {
                xmlDatabase.SaveEntities(regionEntities);
            }
            catch
            {
                throw new DuplicateEntityException(regionEntity.Id, nameof(RegionEntity).Replace("Entity", ""));
            }
        }

        /// <summary>
        /// Get the region with the specified identifier.
        /// </summary>
        /// <returns>The region.</returns>
        /// <param name="id">Identifier.</param>
        public RegionEntity Get(string id)
        {
            List<RegionEntity> regionEntities = xmlDatabase.LoadEntities().ToList();
            RegionEntity regionEntity = regionEntities.FirstOrDefault(x => x.Id == id);

            if (regionEntity == null)
            {
                throw new EntityNotFoundException(id, nameof(BorderEntity).Replace("Entity", ""));
            }

            return regionEntity;
        }

        /// <summary>
        /// Gets all the regions.
        /// </summary>
        /// <returns>The regions</returns>
        public IEnumerable<RegionEntity> GetAll()
        {
            List<RegionEntity> regionEntities = xmlDatabase.LoadEntities().ToList();

            return regionEntities;
        }

        /// <summary>
        /// Updates the specified region.
        /// </summary>
        /// <param name="regionEntity">Region.</param>
        public void Update(RegionEntity regionEntity)
        {
            List<RegionEntity> regionEntities = xmlDatabase.LoadEntities().ToList();
            RegionEntity regionEntityToUpdate = regionEntities.FirstOrDefault(x => x.Id == regionEntity.Id);

            if (regionEntityToUpdate == null)
            {
                throw new EntityNotFoundException(regionEntity.Id, nameof(BorderEntity).Replace("Entity", ""));
            }

            regionEntityToUpdate.Name = regionEntity.Name;
            regionEntityToUpdate.Description = regionEntity.Description;
            regionEntityToUpdate.Colour = regionEntity.Colour;
            regionEntityToUpdate.Type = regionEntity.Type;
            regionEntityToUpdate.FactionId = regionEntity.FactionId;
            regionEntityToUpdate.SovereignFactionId = regionEntity.SovereignFactionId;

            xmlDatabase.SaveEntities(regionEntities);
        }

        /// <summary>
        /// Removes the region with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            List<RegionEntity> regionEntities = xmlDatabase.LoadEntities().ToList();
            regionEntities.RemoveAll(x => x.Id == id);

            try
            {
                xmlDatabase.SaveEntities(regionEntities);
            }
            catch
            {
                throw new DuplicateEntityException(id, nameof(ArmyEntity).Replace("Entity", ""));
            }
        }
    }
}
