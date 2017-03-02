using System.Collections.Generic;
using System.Drawing;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface IFactionRepository
    {
        void Add(Faction faction);

        void Create(string id, string name, string description, Color colour);

        Faction Get(string id);

        List<Faction> GetAll();

        bool Contains(string id);

        void Modify(string id, string name, string description, Color colour);

        void Remove(Faction faction);

        void Remove(string id);

        void Clear();
    }
}
