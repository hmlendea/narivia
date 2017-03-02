using System.Collections.Generic;

using Narivia.Models;

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
        /// <param name="resource">Resource.</param>
        void Add(Resource resource);

        /// <summary>
        /// Gets the resource with the specified identifier.
        /// </summary>
        /// <returns>The resource.</returns>
        /// <param name="id">Identifier.</param>
        Resource Get(string id);

        /// <summary>
        /// Gets all the resources.
        /// </summary>
        /// <returns>The resources.</returns>
        IEnumerable<Resource> GetAll();

        /// <summary>
        /// Updates the specified resource.
        /// </summary>
        /// <param name="resource">Resource.</param>
        void Update(Resource resource);

        /// <summary>
        /// Remove the resource with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(string id);
    }
}
