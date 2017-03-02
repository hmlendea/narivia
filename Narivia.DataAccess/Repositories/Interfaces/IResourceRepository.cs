using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        void Add(Resource resource);

        void Create(string id, string name, string description, ResourceType type, int output);

        Resource Get(string id);

        List<Resource> GetAll();

        bool Contains(string id);

        void Modify(string id, string name, string description, ResourceType type, int output);

        void Remove(Resource resource);

        void Remove(string id);

        void Clear();
    }
}
