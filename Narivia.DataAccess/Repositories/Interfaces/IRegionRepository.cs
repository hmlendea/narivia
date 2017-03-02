using System.Collections.Generic;

using Narivia.Models;

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
        /// <param name="region">Region.</param>
        void Add(Region region);

        /// <summary>
        /// Gets the region with the specified identifier.
        /// </summary>
        /// <returns>The region.</returns>
        /// <param name="id">Identifier.</param>
        Region Get(string id);

        /// <summary>
        /// Gets all the regions.
        /// </summary>
        /// <returns>The regions.</returns>
        IEnumerable<Region> GetAll();

        /// <summary>
        /// Updates the specified region.
        /// </summary>
        /// <param name="region">Region.</param>
        void Update(Region region);

        /// <summary>
        /// Remove the region with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(string id);
    }
}
