using System.Collections.Generic;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        readonly XmlDatabase<Resource> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.ResourceRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public ResourceRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<Resource>(fileName);
        }

        public void Add(Resource resource)
        {
            xmlDatabase.Add(resource);
        }

        public Resource Get(string id)
        {
            return xmlDatabase.Get(id);
        }

        public IEnumerable<Resource> GetAll()
        {
            return xmlDatabase.GetAll();
        }

        public void Update(Resource resource)
        {
            xmlDatabase.Update(resource);
        }

        public void Remove(string id)
        {
            xmlDatabase.Remove(id);
        }
    }
}
