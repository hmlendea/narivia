using System.Collections.Generic;
using System.Linq;
using Narivia.Models;
using Narivia.Models.Enumerations;

namespace Narivia.GameLogic.GameManagers
{
    public sealed class EconomyManager : IEconomyManager
    {
        const int HOLDING_CASTLE_INCOME = 5;
        const int HOLDING_CITY_INCOME = 15;
        const int HOLDING_TEMPLE_INCOME = 10;

        readonly IHoldingManager holdingManager;
        readonly IMilitaryManager militaryManager;
        readonly IWorldManager worldManager;

        public EconomyManager(
            IHoldingManager holdingManager,
            IMilitaryManager militaryManager,
            IWorldManager worldManager)
        {
            this.holdingManager = holdingManager;
            this.militaryManager = militaryManager;
            this.worldManager = worldManager;
        }

        public void LoadContent()
        {

        }

        public void UnloadContent()
        {

        }

        /// <summary>
        /// Gets the faction income.
        /// </summary>
        /// <returns>The faction income.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionIncome(string factionId)
        {
            List<int> incomes = new List<int>();

            foreach(Province province in worldManager.GetFactionProvinces(factionId))
            {
                int income = GetProvinceIncome(province.Id);

                incomes.Add(income);
            }

            return incomes.Sum();
        }

        /// <summary>
        /// Gets the faction outcome.
        /// </summary>
        /// <returns>The faction outcome.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionOutcome(string factionId)
        {
            int outcome = 0;

            outcome += militaryManager.GetArmies()
                            .Where(x => x.FactionId == factionId)
                            .Sum(x => x.Size * militaryManager.GetUnit(x.UnitId).Maintenance);

            return outcome;
        }

        /// <summary>
        /// Gets the income of a province.
        /// </summary>
        /// <returns>The province income.</returns>
        /// <param name="provinceId">Province identifier.</param>
        public int GetProvinceIncome(string provinceId)
        {
            Province province = worldManager.GetProvince(provinceId);
            Resource resource = worldManager.GetResource(province.ResourceId);

            List<Holding> holdings = holdingManager.GetProvinceHoldings(province.Id).ToList();

            int income = worldManager.GetWorld().BaseProvinceIncome;

            income += holdings.Count(h => h.Type == HoldingType.Castle) * HOLDING_CASTLE_INCOME;
            income += holdings.Count(h => h.Type == HoldingType.City) * HOLDING_CITY_INCOME;
            income += holdings.Count(h => h.Type == HoldingType.Temple) * HOLDING_TEMPLE_INCOME;

            if (resource.Type == ResourceType.Economy)
            {
                income += (int)(income * 0.1 * resource.Output);
            }

            return income;
        }
    }
}
