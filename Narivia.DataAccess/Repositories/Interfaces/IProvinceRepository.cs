using System.Collections.Generic;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Province repository interface.
    /// </summary>
    public interface IProvinceRepository
    {
        /// <summary>
        /// Adds the specified province.
        /// </summary>
        /// <param name="provinceEntity">Province.</param>
        void Add(ProvinceEntity provinceEntity);

        /// <summary>
        /// Gets the province with the specified identifier.
        /// </summary>
        /// <returns>The province.</returns>
        /// <param name="id">Identifier.</param>
        ProvinceEntity Get(string id);

        /// <summary>
        /// Gets all the provinces.
        /// </summary>
        /// <returns>The provinces.</returns>
        IEnumerable<ProvinceEntity> GetAll();

        /// <summary>
        /// Updates the specified province.
        /// </summary>
        /// <param name="provinceEntity">Province.</param>
        void Update(ProvinceEntity provinceEntity);

        /// <summary>
        /// Remove the province with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(string id);
    }
}
