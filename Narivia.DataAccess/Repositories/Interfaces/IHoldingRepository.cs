using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface IHoldingRepository
    {
        void Add(Holding holding);

        Holding Get(string id);

        IEnumerable<Holding> GetAll();

        void Update(Holding holding);

        void Remove(string id);
    }
}
