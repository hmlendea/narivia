using System.Collections.Generic;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Region repository implementation.
    /// </summary>
    public class RegionRepository : IRegionRepository
    {
        readonly XmlDatabase<Region> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.RegionRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public RegionRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<Region>(fileName);
        }

        /// <summary>
        /// Adds the specified region.
        /// </summary>
        /// <param name="region">Region.</param>
        public void Add(Region region)
        {
            xmlDatabase.Add(region);
        }

        /// <summary>
        /// Gets the region with the specified identifier.
        /// </summary>
        /// <returns>The region.</returns>
        /// <param name="id">Identifier.</param>
        public Region Get(string id)
        {
            return xmlDatabase.Get(id);
        }

        /// <summary>
        /// Gets all the regions.
        /// </summary>
        /// <returns>The regions.</returns>
        public IEnumerable<Region> GetAll()
        {
            return xmlDatabase.GetAll();
        }

        /// <summary>
        /// Updates the specified region.
        /// </summary>
        /// <param name="region">Region.</param>
        public void Update(Region region)
        {
            xmlDatabase.Update(region);
        }

        /// <summary>
        /// Remove the region with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            xmlDatabase.Remove(id);
        }
    }
}
