using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface IRegionRepository
    {
        void Add(Region region);

        void Create(string id, string name, string description, System.Drawing.Color colour);

        Region Get(string id);

        List<Region> GetAll();

        List<Region> GetAllByFaction(string factionId);

        bool Contains(string id);
        
        void Modify(string id, string name, string description, System.Drawing.Color colour);

        void Remove(Region region);

        void Remove(string id);

        void Clear();
    }
}
