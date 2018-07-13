using System.Collections.Generic;

using Narivia.Models;

namespace Narivia.GameLogic.GameManagers
{
    public interface IMilitaryManager
    {
        void LoadContent();

        void UnloadContent();

        /// <summary>
        /// Gets the army.
        /// </summary>
        /// <returns>The army.</returns>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        Army GetArmy(string factionId, string unitId);

        /// <summary>
        /// Gets the armies.
        /// </summary>
        /// <returns>The armies.</returns>
        IEnumerable<Army> GetArmies();

        /// <summary>
        /// Gets the armies of a faction.
        /// </summary>
        /// <returns>The armies.</returns>
        /// <param name="factionId">Faction identifier.</param>
        IEnumerable<Army> GetFactionArmies(string factionId);

        /// <summary>
        /// Gets the faction troops amount.
        /// </summary>
        /// <returns>The faction troops amount.</returns>
        /// <param name="factionId">Faction identifier.</param>
        int GetFactionTroopsAmount(string factionId);

        /// <summary>
        /// Gets the unit.
        /// </summary>
        /// <returns>The unit.</returns>
        /// <param name="unitId">Unit identifier.</param>
        Unit GetUnit(string unitId);

        /// <summary>
        /// Gets the units.
        /// </summary>
        /// <returns>The units.</returns>
        IEnumerable<Unit> GetUnits();

        /// <summary>
        /// Recruits the specified amount of troops of a unit for a faction.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        /// <param name="amount">Amount.</param>
        void RecruitUnits(string factionId, string unitId, int amount);

        void AddUnits(string factionId, string unitId, int amount);

        /// <summary>
        /// Gets the faction recruitment.
        /// </summary>
        /// <returns>The faction recruitment.</returns>
        /// <param name="factionId">Faction identifier.</param>
        int GetFactionRecruitment(string factionId);

        /// <summary>
        /// Gets the recruitment of a province.
        /// </summary>
        /// <returns>The province recruitment.</returns>
        /// <param name="provinceId">Province identifier.</param>
        int GetProvinceRecruitment(string provinceId);

        void InitialiseFactionMilitary(string factionId);
    }
}
