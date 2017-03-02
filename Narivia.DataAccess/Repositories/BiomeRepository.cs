using System.Collections.Generic;

using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.DataAccess.Repositories
{
    public class BiomeRepository : IBiomeRepository
    {
        readonly XmlDatabase<Biome> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Repositories.BiomeRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public BiomeRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<Biome>(fileName);
        }
        
        public void Add(Biome biome)
        {
            xmlDatabase.Add(biome);
        }

        public Biome Get(string id)
        {
            return xmlDatabase.Get(id);
        }

        public IEnumerable<Biome> GetAll()
        {
            return xmlDatabase.GetAll();
        }

        public void Update(Biome biome)
        {
            xmlDatabase.Update(biome);
        }

        public void Remove(string id)
        {
            xmlDatabase.Remove(id);
        }
    }
}
