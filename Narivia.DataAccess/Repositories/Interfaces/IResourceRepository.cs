using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        void Add(Resource resource);

        Resource Get(string id);

        IEnumerable<Resource> GetAll();

        void Update(Resource resource);

        void Remove(string id);
    }
}
