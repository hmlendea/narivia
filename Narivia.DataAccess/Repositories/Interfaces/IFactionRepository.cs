using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface IFactionRepository
    {
        void Add(Faction faction);

        Faction Get(string id);

        IEnumerable<Faction> GetAll();

        void Update(Faction faction);

        void Remove(string id);
    }
}
