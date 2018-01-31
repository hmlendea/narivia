using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;

using Narivia.GameLogic.Enumerations;
using Narivia.GameLogic.Events;
using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Common.Extensions;
using Narivia.Models;
using Narivia.Models.Enumerations;
using Narivia.Settings;

namespace Narivia.GameLogic.GameManagers
{
    /// <summary>
    /// Game manager.
    /// </summary>
    public class GameManager : IGameManager
    {
        IHoldingManager holdingManager;
        IWorldManager worldManager;
        IAttackManager attackManager;

        const int HOLDING_CASTLE_INCOME = 5;
        const int HOLDING_CASTLE_RECRUITMENT = 15;
        const int HOLDING_CITY_INCOME = 15;
        const int HOLDING_CITY_RECRUITMENT = 5;
        const int HOLDING_TEMPLE_INCOME = 10;
        const int HOLDING_TEMPLE_RECRUITMENT = 10;

        /// <summary>
        /// Occurs when a player province is attacked.
        /// </summary>
        public event BattleEventHandler PlayerProvinceAttacked;

        /// <summary>
        /// Occurs when a faction was destroyed.
        /// </summary>
        public event FactionEventHandler FactionDestroyed;

        /// <summary>
        /// Occurs when a faction was revived.
        /// </summary>
        public event FactionEventHandler FactionRevived;

        /// <summary>
        /// Occurs a faction won the game.
        /// </summary>
        public event FactionEventHandler FactionWon;

        /// <summary>
        /// Gets the player faction identifier.
        /// </summary>
        /// <value>The player faction identifier.</value>
        public string PlayerFactionId { get; private set; }

        /// <summary>
        /// Gets the turn.
        /// </summary>
        /// <value>The turn.</value>
        public int Turn { get; private set; }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        /// <param name="worldId">World identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        public void NewGame(string worldId, string factionId)
        {
            worldManager = new WorldManager();
            holdingManager = new HoldingManager(worldId, worldManager);
            attackManager = new AttackManager(holdingManager, worldManager);

            // TODO: Create a LoadContent() method and move these to it
            worldManager.LoadWorld(worldId);
            holdingManager.LoadContent();

            foreach (Faction faction in worldManager.GetFactions())
            {
                holdingManager.GenerateHoldings(faction.Id);
            }

            InitializeGame(factionId);
        }

        /// <summary>
        /// Starts a new game as a random faction.
        /// </summary>
        /// <param name="worldId">World identifier.</param>
        public void NewGame(string worldId)
        {
            worldManager = new WorldManager();
            worldManager.LoadWorld(worldId);

            foreach (Faction faction in worldManager.GetFactions())
            {
                holdingManager.GenerateHoldings(faction.Id);
            }

            holdingManager = new HoldingManager(worldId, worldManager);
            attackManager = new AttackManager(holdingManager, worldManager);

            Faction playerFaction = GetFactions().Where(f => f.Id != GameDefines.GAIA_FACTION).GetRandomElement();

            InitializeGame(playerFaction.Id);
        }

        /// <summary>
        /// Advances the game by one turn.
        /// </summary>
        public void NextTurn()
        {
            foreach (Faction faction in worldManager.GetFactions().Where(f => f.Alive))
            {
                // Economy
                faction.Wealth += GetFactionIncome(faction.Id);
                faction.Wealth -= GetFactionOutcome(faction.Id);

                // Recruit
                // TODO: Find a way around the hardcoded "militia" unit identifier
                GetArmy(faction.Id, "militia").Size += GetFactionRecruitment(faction.Id);

                // A.I.
                if (faction.Id == PlayerFactionId)
                {
                    continue;
                }

                AiBuild(faction.Id);
                AiRecruit(faction.Id);

                string provinceId = attackManager.ChooseProvinceToAttack(faction.Id);

                if (GetFactionTroopsAmount(faction.Id) < GetWorld().MinTroopsPerAttack ||
                    string.IsNullOrEmpty(provinceId))
                {
                    continue;
                }

                AttackProvince(faction.Id, provinceId);
            }

            Parallel.ForEach(GetProvinces(), province => province.Locked = false);

            UpdateFactionsAliveStatus();
            CheckForWinner();

            Turn += 1;
        }

        /// <summary>
        /// Checks wether the specified provinces share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified provinces share a border, <c>false</c> otherwise.</returns>
        /// <param name="sourceProvinceId">Source province identifier.</param>
        /// <param name="targetProvinceId">Target province identifier.</param>
        public bool ProvinceBordersProvince(string sourceProvinceId, string targetProvinceId)
        => worldManager.ProvinceBordersProvince(sourceProvinceId, targetProvinceId);

        /// <summary>
        /// Checks wether a province has empty holding slots.
        /// </summary>
        /// <returns><c>true</c>, if the province has empty holding slots, <c>false</c> otherwise.</returns>
        /// <param name="provinceId">Province identifier.</param>
        public bool ProvinceHasEmptyHoldingSlots(string provinceId)
        => holdingManager.ProvinceHasEmptyHoldingSlots(provinceId);

        /// <summary>
        /// Checks wether the specified factions share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified factions share a border, <c>false</c> otherwise.</returns>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        public bool FactionBordersFaction(string sourceFactionId, string targetFactionId)
        => worldManager.FactionBordersFaction(sourceFactionId, targetFactionId);

        /// <summary>
        /// Checks wether the specified faction shares a border with the specified province.
        /// </summary>
        /// <returns><c>true</c>, if the faction share a border with that province, <c>false</c> otherwise.</returns>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="provinceId">Province identifier.</param>
        public bool FactionBordersProvince(string factionId, string provinceId)
        => worldManager.FactionBordersProvince(factionId, provinceId);

        /// <summary>
        /// Returns the faction identifier at the given location.
        /// </summary>
        /// <returns>The faction identifier.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public string FactionIdAtLocation(int x, int y)
        => worldManager.FactionIdAtLocation(x, y);

        /// <summary>
        /// Transfers the specified province to the specified faction.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        public void TransferProvince(string provinceId, string factionId)
        => worldManager.TransferProvince(provinceId, factionId);

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
        => worldManager.GetArmies();

        /// <summary>
        /// Gets the biome.
        /// </summary>
        /// <returns>The biome.</returns>
        /// <param name="biomeId">Biome identifier.</param>
        public Biome GetBiome(string biomeId)
        => GetBiomes().FirstOrDefault(b => b.Id == biomeId);

        public IEnumerable<Biome> GetBiomes()
        => worldManager.GetBiomes();

        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <returns>The culture.</returns>
        /// <param name="cultureId">Culture identifier.</param>
        public Culture GetCulture(string cultureId)
        => GetCultures().FirstOrDefault(c => c.Id == cultureId);

        /// <summary>
        /// Gets the cultures.
        /// </summary>
        /// <returns>The cultures.</returns>
        public IEnumerable<Culture> GetCultures()
        => worldManager.GetCultures();

        /// <summary>
        /// Gets the faction.
        /// </summary>
        /// <returns>The faction.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public Faction GetFaction(string factionId)
        => GetFactions().FirstOrDefault(f => f.Id == factionId);

        /// <summary>
        /// Gets the factions.
        /// </summary>
        /// <returns>The factions.</returns>
        public IEnumerable<Faction> GetFactions()
        => worldManager.GetFactions();

        /// <summary>
        /// Gets the culture of a faction.
        /// </summary>
        /// <returns>The culture.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public Culture GetFactionCulture(string factionId)
        => GetCulture(GetFaction(factionId).CultureId);

        /// <summary>
        /// Gets the flag of a factions.
        /// </summary>
        /// <returns>The faction flag.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public Flag GetFactionFlag(string factionId)
        => GetFlag(GetFaction(factionId).FlagId);

        /// <summary>
        /// Gets the faction income.
        /// </summary>
        /// <returns>The faction income.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionIncome(string factionId)
        {
            ConcurrentBag<int> incomes = new ConcurrentBag<int>();

            Parallel.ForEach(GetFactionProvinces(factionId),
                             province => incomes.Add(GetProvinceIncome(province.Id)));

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

            outcome += worldManager.GetArmies()
                            .Where(x => x.FactionId == factionId)
                            .Sum(x => x.Size * GetUnit(x.UnitId).Maintenance);

            return outcome;
        }

        /// <summary>
        /// Gets the faction recruitment.
        /// </summary>
        /// <returns>The faction recruitment.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionRecruitment(string factionId)
        {
            ConcurrentBag<int> recruitments = new ConcurrentBag<int>();

            Parallel.ForEach(GetFactionProvinces(factionId),
                             province => recruitments.Add(GetProvinceRecruitment(province.Id)));

            return recruitments.Sum();
        }

        /// <summary>
        /// Gets the holdings of a faction.
        /// </summary>
        /// <returns>The holdings.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Holding> GetFactionHoldings(string factionId)
        => holdingManager.GetFactionHoldings(factionId);

        /// <summary>
        /// Gets the provinces of a faction.
        /// </summary>
        /// <returns>The provinces.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Province> GetFactionProvinces(string factionId)
        => GetProvinces().Where(r => r.FactionId == factionId);

        /// <summary>
        /// Gets the relations of a faction.
        /// </summary>
        /// <returns>The relations of a faction.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Relation> GetFactionRelations(string factionId)
        => worldManager.GetFactionRelations(factionId);

        /// <summary>
        /// Gets the faction troops amount.
        /// </summary>
        /// <returns>The faction troops amount.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionTroopsAmount(string factionId)
        => worldManager.GetFactionTroopsAmount(factionId);

        /// <summary>
        /// Gets the faction capital.
        /// </summary>
        /// <returns>The faction capital province.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public Province GetFactionCapital(string factionId)
        => worldManager.GetFactionCapital(factionId);

        /// <summary>
        /// Gets or sets the X map coordinate of the centre of the faction territoriy.
        /// </summary>
        /// <value>The X coordinate.</value>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionCentreX(string factionId)
        => worldManager.GetFactionCentreX(factionId);

        /// <summary>
        /// Gets or sets the Y map coordinate of the centre of the faction territoriy.
        /// </summary>
        /// <value>The Y coordinate.</value>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionCentreY(string factionId)
        => worldManager.GetFactionCentreY(factionId);

        /// <summary>
        /// Gets the flag.
        /// </summary>
        /// <returns>The flag.</returns>
        /// <param name="flagId">Flag identifier.</param>
        public Flag GetFlag(string flagId)
        => GetFlags().FirstOrDefault(f => f.Id == flagId);

        /// <summary>
        /// Gets the flags.
        /// </summary>
        /// <returns>The flags.</returns>
        public IEnumerable<Flag> GetFlags()
        => worldManager.GetFlags();

        /// <summary>
        /// Gets the holding.
        /// </summary>
        /// <returns>The holding.</returns>
        /// <param name="holdingId">Holding identifier.</param>
        public Holding GetHolding(string holdingId)
        => GetHoldings().FirstOrDefault(h => h.Id == holdingId);

        public IEnumerable<Holding> GetHoldings()
        => holdingManager.GetHoldings();

        /// <summary>
        /// Gets the province.
        /// </summary>
        /// <returns>The province.</returns>
        /// <param name="provinceId">Province identifier.</param>
        public Province GetProvince(string provinceId)
        => GetProvinces().FirstOrDefault(r => r.Id == provinceId);

        /// <summary>
        /// Gets the holdings of a province.
        /// </summary>
        /// <returns>The holdings.</returns>
        /// <param name="provinceId">Province identifier.</param>
        public IEnumerable<Holding> GetProvinceHoldings(string provinceId)
        => holdingManager.GetProvinceHoldings(provinceId);

        /// <summary>
        /// Gets the income of a province.
        /// </summary>
        /// <returns>The province income.</returns>
        /// <param name="provinceId">Province identifier.</param>
        public int GetProvinceIncome(string provinceId)
        {
            Province province = GetProvince(provinceId);
            Resource resource = GetResource(province.ResourceId);

            List<Holding> holdings = holdingManager.GetProvinceHoldings(province.Id).ToList();

            int income = GetWorld().BaseProvinceIncome;

            income += holdings.Count(h => h.Type == HoldingType.Castle) * HOLDING_CASTLE_INCOME;
            income += holdings.Count(h => h.Type == HoldingType.City) * HOLDING_CITY_INCOME;
            income += holdings.Count(h => h.Type == HoldingType.Temple) * HOLDING_TEMPLE_INCOME;

            if (resource.Type == ResourceType.Economy)
            {
                income += (int)(income * 0.1 * resource.Output);
            }

            return income;
        }

        /// <summary>
        /// Gets the recruitment of a province.
        /// </summary>
        /// <returns>The province recruitment.</returns>
        /// <param name="provinceId">Province identifier.</param>
        public int GetProvinceRecruitment(string provinceId)
        {
            Province province = GetProvince(provinceId);
            Resource resource = GetResource(province.ResourceId);

            List<Holding> holdings = holdingManager.GetProvinceHoldings(province.Id).ToList();

            int recruitment = GetWorld().BaseProvinceRecruitment;

            recruitment += holdings.Count(h => h.Type == HoldingType.Castle) * HOLDING_CASTLE_RECRUITMENT;
            recruitment += holdings.Count(h => h.Type == HoldingType.City) * HOLDING_CITY_RECRUITMENT;
            recruitment += holdings.Count(h => h.Type == HoldingType.Temple) * HOLDING_TEMPLE_RECRUITMENT;

            if (resource.Type == ResourceType.Military)
            {
                recruitment += (int)(recruitment * 0.1 * resource.Output);
            }

            return recruitment;
        }

        /// <summary>
        /// Gets the provinces.
        /// </summary>
        /// <returns>The provinces.</returns>
        public IEnumerable<Province> GetProvinces()
        => worldManager.GetProvinces();


        /// <summary>
        /// Gets the relation between two factions.
        /// </summary>
        /// <returns>The faction relation.</returns>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        public Relation GetRelation(string sourceFactionId, string targetFactionId)
        => worldManager.GetRelations().FirstOrDefault(r => r.SourceFactionId == sourceFactionId &&
                                                    r.TargetFactionId == targetFactionId);

        /// <summary>
        /// Gets the relations between factions.
        /// </summary>
        /// <returns>The relations.</returns>
        public IEnumerable<Relation> GetRelations()
        => worldManager.GetRelations();

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <returns>The resource.</returns>
        /// <param name="resourceId">Resource identifier.</param>
        public Resource GetResource(string resourceId)
        => GetResources().FirstOrDefault(r => r.Id == resourceId);

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <returns>The resources.</returns>
        public IEnumerable<Resource> GetResources()
        => worldManager.GetResources();

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
        => worldManager.GetUnits();

        /// <summary>
        /// Gets the world.
        /// </summary>
        /// <returns>The world.</returns>
        public World GetWorld()
        => worldManager.GetWorld();

        /// <summary>
        /// Builds the specified holding type in a province.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        /// <param name="holdingType">Holding type.</param>
        public void BuildHolding(string provinceId, HoldingType holdingType)
        {
            Province province = GetProvince(provinceId);


            if (ProvinceHasEmptyHoldingSlots(provinceId))
            {
                holdingManager.AddHolding(provinceId, holdingType);
                GetFaction(province.FactionId).Wealth -= GetWorld().HoldingsPrice;
            }
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
        /// Recruits the specified amount of troops of a unit for a faction.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        /// <param name="amount">Amount.</param>
        public void RecruitUnits(string factionId, string unitId, int amount)
        {
            Faction faction = GetFaction(factionId);
            Unit unit = GetUnit(unitId);

            if (faction.Wealth < unit.Price * amount)
            {
                amount = faction.Wealth / unit.Price;
            }

            AddUnits(faction.Id, unit.Id, amount);
            faction.Wealth -= unit.Price * amount;
        }

        /// <summary>
        /// The player faction will attack the specified province.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        public BattleResult PlayerAttackProvince(string provinceId)
        {
            BattleResult result = AttackProvince(PlayerFactionId, provinceId);

            return result;
        }

        void InitializeGame(string factionId)
        {
            PlayerFactionId = factionId;
            Turn = 0;
        }

        void UpdateFactionsAliveStatus()
        {
            Parallel.ForEach(GetFactions().Where(f => f.Id != GameDefines.GAIA_FACTION), faction =>
            {
                bool wasAlive = faction.Alive;

                faction.Alive = GetFactionProvinces(faction.Id).Count() > 0;

                if (wasAlive && !faction.Alive && FactionDestroyed != null)
                {
                    FactionDestroyed(this, new FactionEventArgs(faction.Id));
                }
                else if (!wasAlive && faction.Alive && FactionRevived != null)
                {
                    FactionRevived(this, new FactionEventArgs(faction.Id));
                }
            });
        }

        void CheckForWinner()
        {
            if (GetFactions().Count(f => f.Alive) > 1)
            {
                return;
            }

            Faction faction = GetFactions().FirstOrDefault(f => f.Alive);

            if (FactionWon != null)
            {
                FactionWon(this, new FactionEventArgs(faction.Id));
            }
        }

        BattleResult AttackProvince(string factionId, string provinceId)
        {
            Province province = GetProvince(provinceId);
            string oldProvinceFactionId = province.FactionId;

            BattleResult result = attackManager.AttackProvince(factionId, provinceId);

            if (GetRelation(factionId, oldProvinceFactionId).Value > 0)
            {
                worldManager.SetRelations(factionId, oldProvinceFactionId, 0);
            }

            if (result == BattleResult.Victory)
            {
                worldManager.ChangeRelations(factionId, oldProvinceFactionId, -10);
            }

            switch (province.Type)
            {
                case ProvinceType.Capital:
                    worldManager.ChangeRelations(factionId, oldProvinceFactionId, -10);
                    break;

                case ProvinceType.Province:
                    worldManager.ChangeRelations(factionId, oldProvinceFactionId, -5);
                    break;
            }

            switch (province.State)
            {
                case ProvinceState.Sovereign:
                    worldManager.ChangeRelations(factionId, oldProvinceFactionId, -5);
                    break;

                case ProvinceState.Occupied:
                    worldManager.ChangeRelations(factionId, oldProvinceFactionId, -2);
                    break;
            }

            if (oldProvinceFactionId == PlayerFactionId)
            {
                BattleEventArgs e = new BattleEventArgs(provinceId, factionId, result);

                if (PlayerProvinceAttacked != null)
                {
                    PlayerProvinceAttacked(this, e);
                }
            }

            return result;
        }

        void AiBuild(string factionId)
        {
            Faction faction = GetFaction(factionId);

            // TODO: Reduce all this duplicated code

            while (faction.Wealth >= GetWorld().HoldingsPrice)
            {
                List<Province> validSovereignProvinces = GetFactionProvinces(factionId).Where(r => r.State == ProvinceState.Sovereign &&
                                                                                             ProvinceHasEmptyHoldingSlots(r.Id)).ToList();

                if (validSovereignProvinces.Count < 1)
                {
                    break;
                }

                HoldingType type = Enum.GetValues(typeof(HoldingType)).Cast<HoldingType>().Where(x => x != HoldingType.Empty).GetRandomElement();

                BuildHolding(validSovereignProvinces.GetRandomElement().Id, type);
            }

            while (faction.Wealth >= GetWorld().HoldingsPrice)
            {
                List<Province> validOccupiedProvinces = GetFactionProvinces(factionId).Where(r => r.State == ProvinceState.Occupied &&
                                                                                             ProvinceHasEmptyHoldingSlots(r.Id)).ToList();

                if (validOccupiedProvinces.Count < 1)
                {
                    break;
                }

                HoldingType type = Enum.GetValues(typeof(HoldingType)).Cast<HoldingType>().Where(x => x != HoldingType.Empty).GetRandomElement();

                BuildHolding(validOccupiedProvinces.GetRandomElement().Id, type);
            }
        }

        void AiRecruit(string factionId)
        {
            int minPrice = GetUnits().Min(u => u.Price);

            while (GetFaction(factionId).Wealth >= minPrice)
            {
                string unitId = GetUnits().Select(u => u.Id).GetRandomElement();

                RecruitUnits(factionId, unitId, 1);
            }
        }
    }
}
