using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface IBorderRepository
    {
        void Add(Border border);

        Border Get(string region1Id, string region2Id);

        bool Contains(string region1Id, string region2Id);

        List<Border> GetAll();

        List<Border> GetAllByRegion(string regionId);

        void Remove(Border border);

        void Remove(string id);

        void Clear();
    }
}
