using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface IUnitRepository
    {
        void Add(Unit unit);

        void Create(string id, string name, string description, UnitType type,
                    int price, int maintenance, int power, int health);

        Unit Get(string id);

        List<Unit> GetAll();

        bool Contains(string id);

        void Modify(string id, string name, string description, UnitType type,
                    int price, int maintenance, int power, int health);

        void Remove(Unit unit);

        void Remove(string id);

        void Clear();
    }
}
