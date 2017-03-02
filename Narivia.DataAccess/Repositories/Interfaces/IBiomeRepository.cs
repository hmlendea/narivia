using System.Collections.Generic;
using System.Drawing;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface IBiomeRepository
    {
        void Add(Biome biome);

        void Create(string id, string name, string description, Color colour);

        Biome Get(string id);

        List<Biome> GetAll();

        bool Contains(string id);

        void Modify(string id, string name, string description, Color colour);

        void Remove(Biome biome);

        void Remove(string id);

        void Clear();
    }
}
