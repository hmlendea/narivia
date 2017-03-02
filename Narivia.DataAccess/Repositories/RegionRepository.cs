using System.Collections.Generic;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        readonly XmlDatabase<Region> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.RegionRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public RegionRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<Region>(fileName);
        }

        public void Add(Region region)
        {
            xmlDatabase.Add(region);
        }

        public Region Get(string id)
        {
            return xmlDatabase.Get(id);
        }

        public IEnumerable<Region> GetAll()
        {
            return xmlDatabase.GetAll();
        }

        public void Update(Region region)
        {
            xmlDatabase.Update(region);
        }

        public void Remove(string id)
        {
            xmlDatabase.Remove(id);
        }
    }
}
