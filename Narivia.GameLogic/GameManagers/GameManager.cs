using System;
using System.Collections.Generic;
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
        IAttackManager attackManager;
        IDiplomacyManager diplomacyManager;
        IEconomyManager economyManager;
        IHoldingManager holdingManager;
        IMilitaryManager militaryManager;
        IWorldManager worldManager;

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
        /// Gets the world identifier.
        /// </summary>
        /// <value>The world identifier.</value>
        public string WorldId { get; private set; }

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

        public GameManager(string worldId)
        {
            WorldId = worldId;
        }

        public GameManager(string worldId, string playerFactionId)
            : this(worldId)
        {
            PlayerFactionId = playerFactionId;
        }

        public void LoadContent()
        {
            worldManager = new WorldManager(WorldId);
            diplomacyManager = new DiplomacyManager(worldManager);
            holdingManager = new HoldingManager(WorldId, worldManager);
            militaryManager = new MilitaryManager(holdingManager, worldManager);
            economyManager = new EconomyManager(holdingManager, militaryManager, worldManager);
            attackManager = new AttackManager(diplomacyManager, holdingManager, militaryManager, worldManager);

            worldManager.LoadContent();
            diplomacyManager.LoadContent();
            holdingManager.LoadContent();
            militaryManager.LoadContent();
            economyManager.LoadContent();

            foreach (Province province in worldManager.GetProvinces())
            {
                worldManager.InitialiseProvince(province.Id);
            }

            foreach (Faction faction in worldManager.GetFactions())
            {
                worldManager.InitialiseFaction(faction.Id);
                diplomacyManager.InitialiseFactionRelations(faction.Id);
                holdingManager.InitialiseFactionHoldings(faction.Id);
                militaryManager.InitialiseFactionMilitary(faction.Id);
            }

            Turn = 0;
        }

        public void UnloadContent()
        {
            diplomacyManager.UnloadContent();
            economyManager.UnloadContent();
            holdingManager.UnloadContent();
            militaryManager.UnloadContent();
            worldManager.UnloadContent();
        }

        /// <summary>
        /// Advances the game by one turn.
        /// </summary>
        public void NextTurn()
        {
            foreach (Faction faction in worldManager.GetFactions().Where(f => f.Alive))
            {
                // Economy
                faction.Wealth += economyManager.GetFactionIncome(faction.Id);
                faction.Wealth -= economyManager.GetFactionOutcome(faction.Id);

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
        => militaryManager.GetArmy(factionId, unitId);

        /// <summary>
        /// Gets the armies.
        /// </summary>
        /// <returns>The armies.</returns>
        public IEnumerable<Army> GetArmies()
        => militaryManager.GetArmies();

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
            => economyManager.GetFactionIncome(factionId);

        /// <summary>
        /// Gets the faction outcome.
        /// </summary>
        /// <returns>The faction outcome.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionOutcome(string factionId)
            => economyManager.GetFactionOutcome(factionId);

        /// <summary>
        /// Gets the faction recruitment.
        /// </summary>
        /// <returns>The faction recruitment.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionRecruitment(string factionId)
            => militaryManager.GetFactionRecruitment(factionId);

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
        => diplomacyManager.GetFactionRelations(factionId);

        /// <summary>
        /// Gets the faction troops amount.
        /// </summary>
        /// <returns>The faction troops amount.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionTroopsAmount(string factionId)
        => militaryManager.GetFactionTroopsAmount(factionId);

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
            => economyManager.GetProvinceIncome(provinceId);

        /// <summary>
        /// Gets the recruitment of a province.
        /// </summary>
        /// <returns>The province recruitment.</returns>
        /// <param name="provinceId">Province identifier.</param>
        public int GetProvinceRecruitment(string provinceId)
        => militaryManager.GetProvinceRecruitment(provinceId);

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
            => diplomacyManager.GetRelation(sourceFactionId, targetFactionId);

        /// <summary>
        /// Gets the relations between factions.
        /// </summary>
        /// <returns>The relations.</returns>
        public IEnumerable<Relation> GetRelations()
        => diplomacyManager.GetRelations();

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <returns>The resource.</returns>
        /// <param name="resourceId">Resource identifier.</param>
        public Resource GetResource(string resourceId)
            => worldManager.GetResource(resourceId);

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <returns>The resources.</returns>
        public IEnumerable<Resource> GetResources()
        => worldManager.GetResources();

        /// <summary>
        /// Gets the terrain.
        /// </summary>
        /// <returns>The terrain.</returns>
        /// <param name="terrainId">Terrain identifier.</param>
        public Terrain GetTerrain(string terrainId)
        => GetTerrains().FirstOrDefault(b => b.Id == terrainId);

        public IEnumerable<Terrain> GetTerrains()
        => worldManager.GetTerrains();

        /// <summary>
        /// Gets the unit.
        /// </summary>
        /// <returns>The unit.</returns>
        /// <param name="unitId">Unit identifier.</param>
        public Unit GetUnit(string unitId)
        => militaryManager.GetUnit(unitId);

        /// <summary>
        /// Gets the units.
        /// </summary>
        /// <returns>The units.</returns>
        public IEnumerable<Unit> GetUnits()
        => militaryManager.GetUnits();

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
            holdingManager.BuildHolding(provinceId, holdingType);
        }
        /// <summary>
        /// Recruits the specified amount of troops of a unit for a faction.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        /// <param name="amount">Amount.</param>
        public void RecruitUnits(string factionId, string unitId, int amount)
        {
            militaryManager.RecruitUnits(factionId, unitId, amount);
        }

        /// <summary>
        /// The player faction will attack the specified province.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        public BattleResult PlayerAttackProvince(string provinceId)
            => AttackProvince(PlayerFactionId, provinceId);

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

            FactionWon?.Invoke(this, new FactionEventArgs(faction.Id));
        }

        BattleResult AttackProvince(string factionId, string provinceId)
        {
            Province province = GetProvince(provinceId);
            string oldProvinceFactionId = province.FactionId;

            BattleResult result = attackManager.AttackProvince(factionId, provinceId);

            if (GetRelation(factionId, oldProvinceFactionId).Value > 0)
            {
                diplomacyManager.SetRelations(factionId, oldProvinceFactionId, 0);
            }

            if (result == BattleResult.Victory)
            {
                diplomacyManager.ChangeRelations(factionId, oldProvinceFactionId, -10);
            }

            switch (province.Type)
            {
                case ProvinceType.Capital:
                    diplomacyManager.ChangeRelations(factionId, oldProvinceFactionId, -10);
                    break;

                case ProvinceType.Province:
                    diplomacyManager.ChangeRelations(factionId, oldProvinceFactionId, -5);
                    break;
            }

            switch (province.State)
            {
                case ProvinceState.Sovereign:
                    diplomacyManager.ChangeRelations(factionId, oldProvinceFactionId, -5);
                    break;

                case ProvinceState.Occupied:
                    diplomacyManager.ChangeRelations(factionId, oldProvinceFactionId, -2);
                    break;
            }

            if (oldProvinceFactionId == PlayerFactionId)
            {
                BattleEventArgs e = new BattleEventArgs(provinceId, factionId, result);

                PlayerProvinceAttacked?.Invoke(this, e);
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

                militaryManager.RecruitUnits(factionId, unitId, 1);
            }
        }
    }
}
