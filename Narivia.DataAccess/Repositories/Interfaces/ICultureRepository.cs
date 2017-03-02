using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface ICultureRepository
    {
        void Add(Culture culture);

        void Create(string id, string name, string description);

        Culture Get(string id);

        List<Culture> GetAll();

        bool Contains(string id);

        void Modify(string id, string name, string description);

        void Remove(Culture culture);

        void Remove(string id);

        void Clear();
    }
}
