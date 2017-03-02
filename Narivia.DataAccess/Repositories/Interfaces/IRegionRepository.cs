using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface IRegionRepository
    {
        void Add(Region region);

        Region Get(string id);

        IEnumerable<Region> GetAll();

        void Update(Region region);

        void Remove(string id);
    }
}
