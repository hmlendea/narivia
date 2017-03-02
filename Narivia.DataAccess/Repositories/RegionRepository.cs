using System.Collections.Generic;
using System.Linq;

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
            List<Region> regions = xmlDatabase.LoadEntities().ToList();
            regions.Add(region);

            xmlDatabase.SaveEntities(regions);
        }

        /// <summary>
        /// Get the region with the specified identifier.
        /// </summary>
        /// <returns>The region.</returns>
        /// <param name="id">Identifier.</param>
        public Region Get(string id)
        {
            List<Region> regions = xmlDatabase.LoadEntities().ToList();
            Region region = regions.FirstOrDefault(x => x.Id == id);

            return region;
        }

        /// <summary>
        /// Gets all the regions.
        /// </summary>
        /// <returns>The regions</returns>
        public IEnumerable<Region> GetAll()
        {
            List<Region> regions = xmlDatabase.LoadEntities().ToList();

            return regions;
        }

        /// <summary>
        /// Updates the specified region.
        /// </summary>
        /// <param name="region">Region.</param>
        public void Update(Region region)
        {
            List<Region> regions = xmlDatabase.LoadEntities().ToList();
            Region regionToUpdate = regions.FirstOrDefault(x => x.Id == region.Id);

            regionToUpdate.Name = region.Name;
            regionToUpdate.Description = region.Description;
            regionToUpdate.Colour = region.Colour;
            regionToUpdate.Type = region.Type;
            regionToUpdate.FactionId = region.FactionId;
            regionToUpdate.SovereignFactionId = region.SovereignFactionId;

            xmlDatabase.SaveEntities(regions);
        }

        /// <summary>
        /// Removes the region with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            List<Region> regions = xmlDatabase.LoadEntities().ToList();
            regions.RemoveAll(x => x.Id == id);

            xmlDatabase.SaveEntities(regions);
        }
    }
}
