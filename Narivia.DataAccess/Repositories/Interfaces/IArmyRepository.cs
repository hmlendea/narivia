using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface IArmyRepository
    {
        void Add(Army army);

        Army Get(string factionId, string unitId);

        IEnumerable<Army> GetAll();

        void Remove(string factionId, string unitId);
    }
}
