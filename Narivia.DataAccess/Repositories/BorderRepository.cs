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
    public class BorderRepository : IBorderRepository
    {
        readonly XmlDatabase<BorderEntity> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.DataAccess.Repositories.BorderRepository"/> class.
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
                throw new DuplicateEntityException($"{borderEntity.Region1Id}-{borderEntity.Region2Id}", nameof(BorderEntity).Replace("Entity", ""));
            }
        }

        /// <summary>
        /// Gets the border with the specified faction and unit identifiers.
        /// </summary>
        /// <returns>The border.</returns>
        /// <param name="region1Id">First region identifier.</param>
        /// <param name="region2Id">Second region identifier.</param>
        public BorderEntity Get(string region1Id, string region2Id)
        {
            List<BorderEntity> borderEntities = xmlDatabase.LoadEntities().ToList();
            BorderEntity borderEntity = borderEntities.FirstOrDefault(x => x.Region1Id == region1Id &&
                                                                           x.Region2Id == region2Id);

            if (borderEntity == null)
            {
                throw new EntityNotFoundException($"{borderEntity.Region1Id}-{borderEntity.Region2Id}", nameof(BorderEntity).Replace("Entity", ""));
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
        /// Removes the border with the specified faction and unit identifiers.
        /// </summary>
        /// <param name="region1Id">First region identifier.</param>
        /// <param name="region2Id">Second region identifier.</param>
        public void Remove(string region1Id, string region2Id)
        {
            List<BorderEntity> borderEntities = xmlDatabase.LoadEntities().ToList();
            borderEntities.RemoveAll(x => x.Region1Id == region1Id &&
                                         x.Region2Id == region2Id);

            try
            {
                xmlDatabase.SaveEntities(borderEntities);
            }
            catch
            {
                throw new DuplicateEntityException($"{region1Id}-{region2Id}", nameof(BorderEntity).Replace("Entity", ""));
            }
        }
    }
}
