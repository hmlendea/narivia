using NuciDAL.Repositories;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Resource repository implementation.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ResourceRepository"/> class.
    /// </remarks>
    /// <param name="fileName">File name.</param>
    public class ResourceRepository(string fileName) : XmlRepository<ResourceEntity>(fileName)
    {

        /// <summary>
        /// Updates the specified resource.
        /// </summary>
        /// <param name="entity">Resource.</param>
        public override void Update(ResourceEntity entity)
        {
            LoadEntities();

            ResourceEntity resourceEntityToUpdate = Get(entity.Id) ?? throw new EntityNotFoundException(entity.Id, nameof(BorderEntity));
            resourceEntityToUpdate.Name = entity.Name;
            resourceEntityToUpdate.Description = entity.Description;
            resourceEntityToUpdate.Type = entity.Type;
            resourceEntityToUpdate.Output = entity.Output;

            SaveChanges();
        }
    }
}
