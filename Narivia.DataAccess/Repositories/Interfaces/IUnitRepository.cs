using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface IUnitRepository
    {
        void Add(Unit unit);

        Unit Get(string id);

        IEnumerable<Unit> GetAll();

        void Update(Unit unit);

        void Remove(string id);
    }
}
