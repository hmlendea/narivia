using System.Collections.Generic;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Resource repository interface.
    /// </summary>
    public interface IResourceRepository
    {
        /// <summary>
        /// Adds the specified resource.
        /// </summary>
        /// <param name="resourceEntity">Resource.</param>
        void Add(ResourceEntity resourceEntity);

        /// <summary>
        /// Gets the resource with the specified identifier.
        /// </summary>
        /// <returns>The resource.</returns>
        /// <param name="id">Identifier.</param>
        ResourceEntity Get(string id);

        /// <summary>
        /// Gets all the resources.
        /// </summary>
        /// <returns>The resources.</returns>
        IEnumerable<ResourceEntity> GetAll();

        /// <summary>
        /// Updates the specified resource.
        /// </summary>
        /// <param name="resourceEntity">Resource.</param>
        void Update(ResourceEntity resourceEntity);

        /// <summary>
        /// Remove the resource with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(string id);
    }
}
