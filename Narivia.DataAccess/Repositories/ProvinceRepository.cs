using System.Collections.Generic;
using System.Linq;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Exceptions;
using Narivia.DataAccess.Repositories.Interfaces;

namespace Narivia.DataAccess.Repositories
{
    /// <summary>
    /// Province repository implementation.
    /// </summary>
    public class ProvinceRepository : IRepository<string, ProvinceEntity>
    {
        readonly XmlDatabase<ProvinceEntity> xmlDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvinceRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public ProvinceRepository(string fileName)
        {
            xmlDatabase = new XmlDatabase<ProvinceEntity>(fileName);
        }

        /// <summary>
        /// Adds the specified province.
        /// </summary>
        /// <param name="provinceEntity">Province.</param>
        public void Add(ProvinceEntity provinceEntity)
        {
            List<ProvinceEntity> provinceEntities = xmlDatabase.LoadEntities().ToList();
            provinceEntities.Add(provinceEntity);

            try
            {
                xmlDatabase.SaveEntities(provinceEntities);
            }
            catch
            {
                throw new DuplicateEntityException(provinceEntity.Id, nameof(ProvinceEntity));
            }
        }

        /// <summary>
        /// Get the province with the specified identifier.
        /// </summary>
        /// <returns>The province.</returns>
        /// <param name="id">Identifier.</param>
        public ProvinceEntity Get(string id)
        {
            List<ProvinceEntity> provinceEntities = xmlDatabase.LoadEntities().ToList();
            ProvinceEntity provinceEntity = provinceEntities.FirstOrDefault(x => x.Id == id);

            if (provinceEntity == null)
            {
                throw new EntityNotFoundException(id, nameof(BorderEntity));
            }

            return provinceEntity;
        }

        /// <summary>
        /// Gets all the provinces.
        /// </summary>
        /// <returns>The provinces</returns>
        public IEnumerable<ProvinceEntity> GetAll()
        {
            List<ProvinceEntity> provinceEntities = xmlDatabase.LoadEntities().ToList();

            return provinceEntities;
        }

        /// <summary>
        /// Updates the specified province.
        /// </summary>
        /// <param name="provinceEntity">Province.</param>
        public void Update(ProvinceEntity provinceEntity)
        {
            List<ProvinceEntity> provinceEntities = xmlDatabase.LoadEntities().ToList();
            ProvinceEntity provinceEntityToUpdate = provinceEntities.FirstOrDefault(x => x.Id == provinceEntity.Id);

            if (provinceEntityToUpdate == null)
            {
                throw new EntityNotFoundException(provinceEntity.Id, nameof(BorderEntity));
            }

            provinceEntityToUpdate.Name = provinceEntity.Name;
            provinceEntityToUpdate.Description = provinceEntity.Description;
            provinceEntityToUpdate.ColourHexadecimal = provinceEntity.ColourHexadecimal;
            provinceEntityToUpdate.Type = provinceEntity.Type;
            provinceEntityToUpdate.FactionId = provinceEntity.FactionId;
            provinceEntityToUpdate.SovereignFactionId = provinceEntity.SovereignFactionId;

            xmlDatabase.SaveEntities(provinceEntities);
        }

        /// <summary>
        /// Removes the province with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(string id)
        {
            List<ProvinceEntity> provinceEntities = xmlDatabase.LoadEntities().ToList();
            provinceEntities.RemoveAll(x => x.Id == id);

            try
            {
                xmlDatabase.SaveEntities(provinceEntities);
            }
            catch
            {
                throw new DuplicateEntityException(id, nameof(ProvinceEntity));
            }
        }
    }
}
