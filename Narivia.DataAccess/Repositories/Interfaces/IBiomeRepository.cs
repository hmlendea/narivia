using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface IBiomeRepository
    {
        void Add(Biome biome);

        Biome Get(string id);

        IEnumerable<Biome> GetAll();

        void Update(Biome biome);
        
        void Remove(string id);
    }
}
