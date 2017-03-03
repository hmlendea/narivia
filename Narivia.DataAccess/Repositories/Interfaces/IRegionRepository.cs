using System.Collections.Generic;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Region repository interface.
    /// </summary>
    public interface IRegionRepository
    {
        /// <summary>
        /// Adds the specified region.
        /// </summary>
        /// <param name="regionEntity">Region.</param>
        void Add(RegionEntity regionEntity);

        /// <summary>
        /// Gets the region with the specified identifier.
        /// </summary>
        /// <returns>The region.</returns>
        /// <param name="id">Identifier.</param>
        RegionEntity Get(string id);

        /// <summary>
        /// Gets all the regions.
        /// </summary>
        /// <returns>The regions.</returns>
        IEnumerable<RegionEntity> GetAll();

        /// <summary>
        /// Updates the specified region.
        /// </summary>
        /// <param name="regionEntity">Region.</param>
        void Update(RegionEntity regionEntity);

        /// <summary>
        /// Remove the region with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(string id);
    }
}
