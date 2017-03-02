using System.Collections.Generic;

using Narivia.Models;

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
        /// <param name="culture">Culture.</param>
        void Add(Culture culture);

        /// <summary>
        /// Gets the culture with the specified identifier.
        /// </summary>
        /// <returns>The culture.</returns>
        /// <param name="id">Identifier.</param>
        Culture Get(string id);

        /// <summary>
        /// Gets all the cultures.
        /// </summary>
        /// <returns>The cultures.</returns>
        IEnumerable<Culture> GetAll();

        /// <summary>
        /// Updates the specified culture.
        /// </summary>
        /// <param name="culture">Culture.</param>
        void Update(Culture culture);

        /// <summary>
        /// Remove the culture with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(string id);
    }
}
