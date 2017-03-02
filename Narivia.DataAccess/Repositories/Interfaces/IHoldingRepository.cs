using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface IHoldingRepository
    {
        void Add(Holding holding);

        void Create(string id, string name, string description);

        Holding Get(string id);

        List<Holding> GetAll();

        bool Contains(string id);

        void Modify(string id, string name, string descrption);

        void Remove(Holding holding);

        void Remove(string id);

        void Clear();
    }
}
