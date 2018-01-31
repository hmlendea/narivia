using System.Collections.Generic;

using Narivia.Models;
using Narivia.Models.Enumerations;

namespace Narivia.GameLogic.GameManagers.Interfaces
{
    public interface IHoldingManager
    {
        void LoadContent();

        void UnloadContent();

        void GenerateHoldings(string factionId);

        /// <summary>
        /// Checks wether a province has empty holding slots.
        /// </summary>
        /// <returns><c>true</c>, if the province has empty holding slots, <c>false</c> otherwise.</returns>
        /// <param name="provinceId">Province identifier.</param>
        bool ProvinceHasEmptyHoldingSlots(string provinceId);

        /// <summary>
        /// Gets the holdings of a faction.
        /// </summary>
        /// <returns>The holdings.</returns>
        /// <param name="factionId">Faction identifier.</param>
        IEnumerable<Holding> GetFactionHoldings(string factionId);

        /// <summary>
        /// Gets the holdings.
        /// </summary>
        /// <returns>The holdings.</returns>
        IEnumerable<Holding> GetHoldings();

        /// <summary>
        /// Gets the province holdings.
        /// </summary>
        /// <returns>The province holdings.</returns>
        /// <param name="provinceId">Province identifier.</param>
        IEnumerable<Holding> GetProvinceHoldings(string provinceId);

        /// <summary>
        /// Adds the specified holding type in a province.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        /// <param name="holdingType">Holding type.</param>
        void AddHolding(string provinceId, HoldingType holdingType);
    }
}
