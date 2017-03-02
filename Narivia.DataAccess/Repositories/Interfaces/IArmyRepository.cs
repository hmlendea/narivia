using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface IArmyRepository
    {
        void Add(Army army);

        Army Get(string factionId, string unitId);

        List<Army> GetAll();

        List<Army> GetAllByFaction(string factionId);

        int GetFactionTroops(string factionId);

        bool Contains(string factionId, string unitId);

        bool Contains(string id);

        void Remove(Army army);

        void Remove(string id);

        void Clear();
    }
}
