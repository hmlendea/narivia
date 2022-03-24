using System.Collections.Generic;
using System.IO;
using System.Linq;

using NuciExtensions;
using NuciXNA.DataAccess.Repositories;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Repositories;
using Narivia.GameLogic.Mapping;
using Narivia.Models;
using Narivia.Models.Enumerations;
using Narivia.Settings;

namespace Narivia.GameLogic.GameManagers
{
    public sealed class MilitaryManager : IMilitaryManager
    {
        const int HOLDING_CASTLE_RECRUITMENT = 15;
        const int HOLDING_CITY_RECRUITMENT = 5;
        const int HOLDING_TEMPLE_RECRUITMENT = 10;

        readonly IHoldingManager holdingManager;
        readonly IWorldManager worldManager;

        Dictionary<string, Army> armies;
        Dictionary<string, Unit> units;

        World world;

        public MilitaryManager(
            IHoldingManager holdingManager,
            IWorldManager worldManager)
        {
            this.holdingManager = holdingManager;
            this.worldManager = worldManager;
        }

        public void LoadContent()
        {
            world = worldManager.GetWorld();

            string unitsPath = Path.Combine(ApplicationPaths.WorldsDirectory, world.Id, "units.xml");

            IRepository<string, UnitEntity> unitRepository = new UnitRepository(unitsPath);

            IEnumerable<Unit> unitList = unitRepository.GetAll().ToDomainModels();

            armies = new Dictionary<string, Army>();
            units = new Dictionary<string, Unit>(unitList.ToDictionary(unit => unit.Id, unit => unit));
        }

        public void UnloadContent()
        {
            armies.Clear();
        }

        /// <summary>
        /// Gets the army.
        /// </summary>
        /// <returns>The army.</returns>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        public Army GetArmy(string factionId, string unitId)
        => GetArmies().FirstOrDefault(a => a.FactionId == factionId &&
                                           a.UnitId == unitId);


        /// <summary>
        /// Gets the armies.
        /// </summary>
        /// <returns>The armies.</returns>
        public IEnumerable<Army> GetArmies()
        => armies.Values;

        /// <summary>
        /// Gets the armies of a faction.
        /// </summary>
        /// <returns>The armies.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Army> GetFactionArmies(string factionId)
        => armies.Values.Where(a => a.FactionId == factionId);

        /// <summary>
        /// Gets the faction troops amount.
        /// </summary>
        /// <returns>The faction troops amount.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionTroopsAmount(string factionId)
        => armies.Values.Where(a => a.FactionId == factionId)
                        .Sum(a => a.Size);

        /// <summary>
        /// Gets the unit.
        /// </summary>
        /// <returns>The unit.</returns>
        /// <param name="unitId">Unit identifier.</param>
        public Unit GetUnit(string unitId)
        => GetUnits().FirstOrDefault(u => u.Id == unitId);

        /// <summary>
        /// Gets the units.
        /// </summary>
        /// <returns>The units.</returns>
        public IEnumerable<Unit> GetUnits()
        => units.Values;

        /// <summary>
        /// Recruits the specified amount of troops of a unit for a faction.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        /// <param name="amount">Amount.</param>
        public void RecruitUnits(string factionId, string unitId, int amount)
        {
            Faction faction = worldManager.GetFaction(factionId);
            Unit unit = GetUnit(unitId);

            if (faction.Wealth < unit.Price * amount)
            {
                amount = faction.Wealth / unit.Price;
            }

            AddUnits(faction.Id, unit.Id, amount);
            faction.Wealth -= unit.Price * amount;
        }

        /// <summary>
        /// Adds the specified amount of troops of a unit for a faction.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        /// <param name="amount">Amount.</param>
        public void AddUnits(string factionId, string unitId, int amount)
        {
            GetArmy(factionId, unitId).Size += amount;
        }

        /// <summary>
        /// Gets the faction recruitment.
        /// </summary>
        /// <returns>The faction recruitment.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionRecruitment(string factionId)
        {
            List<int> recruitments = new List<int>();

            foreach (Province province in worldManager.GetFactionProvinces(factionId))
            {
                int income = GetProvinceRecruitment(province.Id);

                recruitments.Add(income);
            }

            return recruitments.Sum();
        }

        /// <summary>
        /// Gets the recruitment of a province.
        /// </summary>
        /// <returns>The province recruitment.</returns>
        /// <param name="provinceId">Province identifier.</param>
        public int GetProvinceRecruitment(string provinceId)
        {
            Province province = worldManager.GetProvince(provinceId);
            Resource resource = worldManager.GetResource(province.ResourceId);

            List<Holding> holdings = holdingManager.GetProvinceHoldings(province.Id).ToList();

            int recruitment = world.BaseProvinceRecruitment;

            recruitment += holdings.Count(h => h.Type == HoldingType.Castle) * HOLDING_CASTLE_RECRUITMENT;
            recruitment += holdings.Count(h => h.Type == HoldingType.City) * HOLDING_CITY_RECRUITMENT;
            recruitment += holdings.Count(h => h.Type == HoldingType.Temple) * HOLDING_TEMPLE_RECRUITMENT;

            if (resource.Type == ResourceType.Military)
            {
                recruitment += (int)(recruitment * 0.1 * resource.Output);
            }

            return recruitment;
        }

        public void InitialiseFactionMilitary(string factionId)
        {
            foreach (Unit unit in units.Values)
            {
                Army army = new Army
                {
                    Id = $"{factionId}:{unit.Id}",
                    FactionId = factionId,
                    UnitId = unit.Id,
                    Size = world.StartingTroops
                };

                armies.AddOrUpdate(army.Id, army);
            }
        }
    }
}
