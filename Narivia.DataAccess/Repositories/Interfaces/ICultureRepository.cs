using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface ICultureRepository
    {
        void Add(Culture culture);

        Culture Get(string id);

        IEnumerable<Culture> GetAll();

        void Update(Culture culture);

        void Remove(string id);
    }
}
