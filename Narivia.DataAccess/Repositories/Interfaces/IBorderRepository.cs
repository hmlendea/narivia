using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface IBorderRepository
    {
        void Add(Border border);

        Border Get(string region1Id, string region2Id);

        IEnumerable<Border> GetAll();

        void Remove(string region1Id, string region2Id);
    }
}
