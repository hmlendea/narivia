using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Exceptions;
using Narivia.DataAccess.Repositories.Interfaces;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Border repository implementation.
    /// </summary>
    public class BorderRepository : IRepository<string, BorderEntity>
    {
        readonly XmlDatabase<BorderEntity> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="BorderRepository"/> class.
        /// </summary>
        public BorderRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<BorderEntity>(fileName);
        }

        /// <summary>
        /// Adds the specified border.
        /// </summary>
        /// <param name="borderEntity">Border.</param>
        public void Add(BorderEntity borderEntity)
        {
            List<BorderEntity> borderEntities = xmlDatabase.LoadEntities().ToList();
            borderEntities.Add(borderEntity);

            try
            {
                xmlDatabase.SaveEntities(borderEntities);
            }
            catch
            {
                throw new DuplicateEntityException(borderEntity.Id, nameof(BorderEntity));
            }
        }

        /// <summary>
        /// Gets the border with the specified identifier.
        /// </summary>
        /// <returns>The border.</returns>
        /// <param name="id">Identifier.</param>
        public BorderEntity Get(string id)
        {
            List<BorderEntity> borderEntities = xmlDatabase.LoadEntities().ToList();
            BorderEntity borderEntity = borderEntities.FirstOrDefault(x => x.Id == id);

            if (borderEntity == null)
            {
                throw new EntityNotFoundException(borderEntity.Id, nameof(BorderEntity));
            }

            return borderEntity;
        }

        /// <summary>
        /// Gets all the borders.
        /// </summary>
        /// <returns>The borders.</returns>
        public IEnumerable<BorderEntity> GetAll()
        {
            List<BorderEntity> borderEntities = xmlDatabase.LoadEntities().ToList();

            return borderEntities;
        }

        /// <summary>
        /// Removes the border with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            List<BorderEntity> borderEntities = xmlDatabase.LoadEntities().ToList();
            borderEntities.RemoveAll(x => x.Id == id);

            try
            {
                xmlDatabase.SaveEntities(borderEntities);
            }
            catch
            {
                throw new DuplicateEntityException(id, nameof(BorderEntity));
            }
        }
    }
}
