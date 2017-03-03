using System.Collections.Generic;

using Narivia.DataAccess.DataObjects;

namespace Narivia.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Culture repository interface.
    /// </summary>
    public interface ICultureRepository
    {
        /// <summary>
        /// Adds the specified culture.
        /// </summary>
        /// <param name="cultureEntity">Culture.</param>
        void Add(CultureEntity cultureEntity);

        /// <summary>
        /// Gets the culture with the specified identifier.
        /// </summary>
        /// <returns>The culture.</returns>
        /// <param name="id">Identifier.</param>
        CultureEntity Get(string id);

        /// <summary>
        /// Gets all the cultures.
        /// </summary>
        /// <returns>The cultures.</returns>
        IEnumerable<CultureEntity> GetAll();

        /// <summary>
        /// Updates the specified culture.
        /// </summary>
        /// <param name="cultureEntity">Culture.</param>
        void Update(CultureEntity cultureEntity);

        /// <summary>
        /// Remove the culture with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(string id);
    }
}
