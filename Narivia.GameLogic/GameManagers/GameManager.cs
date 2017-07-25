using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.GameLogic.Enumerations;
using Narivia.GameLogic.Events;
using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Infrastructure.Extensions;
using Narivia.Infrastructure.Helpers;
using Narivia.Models;
using Narivia.Models.Enumerations;

namespace Narivia.GameLogic.GameManagers
{
    /// <summary>
    /// Game manager.
    /// </summary>
    public class GameManager : IGameManager
    {
        IWorldManager world;
        IAttackManager attack;

        const int HOLDING_CASTLE_INCOME = 5;
        const int HOLDING_CASTLE_RECRUITMENT = 15;
        const int HOLDING_CITY_INCOME = 15;
        const int HOLDING_CITY_RECRUITMENT = 5;
        const int HOLDING_TEMPLE_INCOME = 10;
        const int HOLDING_TEMPLE_RECRUITMENT = 10;

        /// <summary>
        /// Occurs when a player region is attacked.
        /// </summary>
        public event BattleEventHandler PlayerRegionAttacked;

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
        /// Gets or sets the world tiles.
        /// </summary>
        /// <value>The world tiles.</value>
        public string[,] WorldTiles
        {
            get { return world.WorldTiles; }
            set { world.WorldTiles = value; }
        }

        /// <summary>
        /// Gets the width of the world.
        /// </summary>
        /// <value>The width of the world.</value>
        public int WorldWidth => world.WorldWidth;

        /// <summary>
        /// Gets the height of the world.
        /// </summary>
        /// <value>The height of the world.</value>
        public int WorldHeight => world.WorldHeight;

        /// <summary>
        /// Gets the name of the world.
        /// </summary>
        /// <value>The name of the world.</value>
        public string WorldName => world.WorldName;

        /// <summary>
        /// Gets the world identifier.
        /// </summary>
        /// <value>The world identifier.</value>
        public string WorldId => world.WorldId;

        /// <summary>
        /// Gets the base region income.
        /// </summary>
        /// <value>The base region income.</value>
        public int BaseRegionIncome => world.BaseRegionIncome;

        /// <summary>
        /// Gets the base region recruitment.
        /// </summary>
        /// <value>The base region recruitment.</value>
        public int BaseRegionRecruitment => world.BaseRegionRecruitment;

        /// <summary>
        /// Gets the base faction recruitment.
        /// </summary>
        /// <value>The base faction recruitment.</value>
        public int BaseFactionRecruitment => world.BaseFactionRecruitment;

        /// <summary>
        /// Gets the minimum number of troops required to attack.
        /// </summary>
        /// <value>The minimum troops to attack.</value>
        public int MinTroopsPerAttack => world.MinTroopsPerAttack;

        /// <summary>
        /// Gets the starting wealth.
        /// </summary>
        /// <value>The starting wealth.</value>
        public int StartingWealth => world.StartingWealth;

        /// <summary>
        /// Gets the starting troops per unit.
        /// </summary>
        /// <value>The starting troops per unit.</value>
        public int StartingTroopsPerUnit => world.StartingTroops;

        /// <summary>
        /// Gets the price of holdings.
        /// </summary>
        /// <value>The holdings price.</value>
        public int HoldingsPrice => world.HoldingsPrice;

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
            world = new WorldManager();
            world.LoadWorld(worldId);

            attack = new AttackManager(world);

            InitializeGame(factionId);
        }

        /// <summary>
        /// Advances the game by one turn.
        /// </summary>
        public void NextTurn()
        {
            foreach (Faction faction in world.Factions.Values.Where(f => f.Alive))
            {
                // Economy
                faction.Wealth += GetFactionIncome(faction.Id);
                faction.Wealth -= GetFactionOutcome(faction.Id);

                // Recruit
                // TODO: Find a way around the hardcoded "militia" unit identifier
                world.Armies.Values.FirstOrDefault(u => u.FactionId == faction.Id &&
                                                        u.UnitId == "militia")
                                   .Size += GetFactionRecruitment(faction.Id);

                // A.I.
                if (faction.Id == PlayerFactionId)
                {
                    continue;
                }

                AiBuild(faction.Id);
                AiRecruit(faction.Id);

                string regionId = attack.ChooseRegionToAttack(faction.Id);

                if (GetFactionTroopsCount(faction.Id) < MinTroopsPerAttack ||
                    string.IsNullOrEmpty(regionId))
                {
                    continue;
                }

                AttackRegion(faction.Id, regionId);
            }

            world.Regions.Values.ToList().ForEach(r => r.Locked = false);
            UpdateFactionsAliveStatus();
            CheckForWinner();

            Turn += 1;
        }

        /// <summary>
        /// Checks wether the specified regions share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified regions share a border, <c>false</c> otherwise.</returns>
        /// <param name="sourceRegionId">Source region identifier.</param>
        /// <param name="targetRegionId">Target region identifier.</param>
        public bool RegionBordersRegion(string sourceRegionId, string targetRegionId)
        => world.RegionBordersRegion(sourceRegionId, targetRegionId);

        /// <summary>
        /// Checks wether a region has empty holding slots.
        /// </summary>
        /// <returns><c>true</c>, if the region has empty holding slots, <c>false</c> otherwise.</returns>
        /// <param name="regionId">Region identifier.</param>
        public bool RegionHasEmptyHoldingSlots(string regionId)
        => world.RegionHasEmptyHoldingSlots(regionId);

        /// <summary>
        /// Checks wether the specified factions share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified factions share a border, <c>false</c> otherwise.</returns>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        public bool FactionBordersFaction(string sourceFactionId, string targetFactionId)
        => world.FactionBordersFaction(sourceFactionId, targetFactionId);

        /// <summary>
        /// Checks wether the specified faction shares a border with the specified region.
        /// </summary>
        /// <returns><c>true</c>, if the faction share a border with that region, <c>false</c> otherwise.</returns>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="regionId">Region identifier.</param>
        public bool FactionBordersRegion(string factionId, string regionId)
        => world.FactionBordersRegion(factionId, regionId);

        /// <summary>
        /// Returns the faction identifier at the given position.
        /// </summary>
        /// <returns>The faction identifier.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public string FactionIdAtPosition(int x, int y)
        => world.FactionIdAtPosition(x, y);

        /// <summary>
        /// Transfers the specified region to the specified faction.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        public void TransferRegion(string regionId, string factionId)
        => world.TransferRegion(regionId, factionId);

        /// <summary>
        /// Gets the faction army size.
        /// </summary>
        /// <returns>The faction army size.</returns>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        public int GetFactionArmySize(string factionId, string unitId)
        => world.Armies.Values.FirstOrDefault(a => a.FactionId == factionId && a.UnitId == unitId).Size;

        /// <summary>
        /// Gets the culture of a faction.
        /// </summary>
        /// <returns>The culture.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public Culture GetFactionCulture(string factionId)
        => world.Cultures[world.Factions[factionId].CultureId];

        /// <summary>
        /// Gets the name of the faction.
        /// </summary>
        /// <returns>The faction name.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public string GetFactionName(string factionId)
        => world.Factions[factionId].Name;

        /// <summary>
        /// Returns the colour of a faction.
        /// </summary>
        /// <returns>The colour.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public Colour GetFactionColour(string factionId)
        => world.Factions[factionId].Colour;

        /// <summary>
        /// Gets the flag of a factions.
        /// </summary>
        /// <returns>The faction flag.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public Flag GetFactionFlag(string factionId)
        => world.GetFactionFlag(factionId);

        /// <summary>
        /// Gets the faction income.
        /// </summary>
        /// <returns>The faction income.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionIncome(string factionId)
        {
            int income = 0;

            foreach (Region region in GetFactionRegions(factionId))
            {
                Resource resource = world.Resources[region.ResourceId];

                int regionIncome = BaseRegionIncome;

                regionIncome += world.GetRegionHoldings(region.Id).Count(h => h.Type == HoldingType.Castle) * HOLDING_CASTLE_INCOME;
                regionIncome += world.GetRegionHoldings(factionId).Count(h => h.Type == HoldingType.City) * HOLDING_CITY_INCOME;
                regionIncome += world.GetRegionHoldings(factionId).Count(h => h.Type == HoldingType.Temple) * HOLDING_TEMPLE_INCOME;

                if (resource.Type == ResourceType.Economy)
                {
                    regionIncome += (int)(regionIncome * 0.1 * resource.Output);
                }

                income += regionIncome;
            }

            return income;
        }

        /// <summary>
        /// Gets the faction outcome.
        /// </summary>
        /// <returns>The faction outcome.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionOutcome(string factionId)
        {
            int outcome = 0;

            outcome += world.Armies.Values.Where(x => x.FactionId == factionId)
                                         .Sum(x => x.Size * world.Units[x.UnitId].Maintenance);

            return outcome;
        }

        /// <summary>
        /// Gets the faction recruitment.
        /// </summary>
        /// <returns>The faction recruitment.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionRecruitment(string factionId)
        {
            int recruitment = BaseFactionRecruitment;

            foreach (Region region in GetFactionRegions(factionId))
            {
                Resource resource = world.Resources[region.ResourceId];

                int regionRecruitment = BaseRegionRecruitment;

                regionRecruitment += world.GetRegionHoldings(region.Id).Count(h => h.Type == HoldingType.Castle) * HOLDING_CASTLE_RECRUITMENT;
                regionRecruitment += world.GetRegionHoldings(factionId).Count(h => h.Type == HoldingType.City) * HOLDING_CITY_RECRUITMENT;
                regionRecruitment += world.GetRegionHoldings(factionId).Count(h => h.Type == HoldingType.Temple) * HOLDING_TEMPLE_RECRUITMENT;

                if (resource.Type == ResourceType.Military)
                {
                    regionRecruitment += (int)(regionRecruitment * 0.1 * resource.Output);
                }

                recruitment += regionRecruitment;
            }

            return recruitment;
        }

        /// <summary>
        /// Gets the regions count of a faction.
        /// </summary>
        /// <returns>The number of regions.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionRegionsCount(string factionId)
        => world.GetFactionRegions(factionId).ToList().Count;

        /// <summary>
        /// Gets the relation between two factions.
        /// </summary>
        /// <returns>The faction relation.</returns>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        public int GetFactionRelation(string sourceFactionId, string targetFactionId)
        => world.GetFactionRelation(sourceFactionId, targetFactionId);

        /// <summary>
        /// Gets the holdings count of a faction.
        /// </summary>
        /// <returns>The number of holdings.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionHoldingsCount(string factionId)
        => world.GetFactionHoldings(factionId).ToList().Count;

        /// <summary>
        /// Gets the faction wealth.
        /// </summary>
        /// <returns>The faction wealth.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionWealth(string factionId)
        => world.Factions[factionId].Wealth;

        /// <summary>
        /// Gets the faction troops count.
        /// </summary>
        /// <returns>The faction troops count.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionTroopsCount(string factionId)
        => world.GetFactionTroopsCount(factionId);

        /// <summary>
        /// Gets the faction capital.
        /// </summary>
        /// <returns>The faction capital.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public string GetFactionCapital(string factionId)
        => world.GetFactionCapital(factionId);

        /// <summary>
        /// Gets or sets the X map coordinate of the centre of the faction territoriy.
        /// </summary>
        /// <value>The X coordinate.</value>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionCentreX(string factionId)
        => world.GetFactionCentreX(factionId);

        /// <summary>
        /// Gets or sets the Y map coordinate of the centre of the faction territoriy.
        /// </summary>
        /// <value>The Y coordinate.</value>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionCentreY(string factionId)
        => world.GetFactionCentreY(factionId);

        /// <summary>
        /// Gets the faction idenfifier of a region.
        /// </summary>
        /// <returns>The faction identifier.</returns>
        /// <param name="regionId">Region identifier.</param>
        public string GetRegionFaction(string regionId)
        => world.Regions[regionId].FactionId;

        /// <summary>
        /// Gets the name of a region.
        /// </summary>
        /// <returns>The name.</returns>
        /// <param name="regionId">Region identifier.</param>
        public string GetRegionName(string regionId)
        => world.Regions[regionId].Name;

        /// <summary>
        /// Gets the resource of a region.
        /// </summary>
        /// <returns>The resource identifier.</returns>
        /// <param name="regionId">Region identifier.</param>
        public string GetRegionResource(string regionId)
        => world.Resources[world.Regions[regionId].ResourceId].Id;

        /// <summary>
        /// Gets the name of the resource.
        /// </summary>
        /// <returns>The resource name.</returns>
        /// <param name="resourceId">Resource identifier.</param>
        public string GetResourceName(string resourceId)
        => world.Resources[resourceId].Name;

        /// <summary>
        /// Gets the factions.
        /// </summary>
        /// <returns>The factions.</returns>
        public IEnumerable<Faction> GetFactions()
        => world.Factions.Values;

        /// <summary>
        /// Gets the regions of a faction.
        /// </summary>
        /// <returns>The regions.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Region> GetFactionRegions(string factionId)
        => world.GetFactionRegions(factionId);

        /// <summary>
        /// Gets the relations of a faction.
        /// </summary>
        /// <returns>The relations of a faction.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Relation> GetFactionRelations(string factionId)
        => world.GetFactionRelations(factionId);

        /// <summary>
        /// Gets the holdings of a faction.
        /// </summary>
        /// <returns>The holdings.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Holding> GetFactionHoldings(string factionId)
        => world.GetFactionHoldings(factionId);

        /// <summary>
        /// Gets the holdings of a region.
        /// </summary>
        /// <returns>The holdings.</returns>
        /// <param name="regionId">Region identifier.</param>
        public IEnumerable<Holding> GetRegionHoldings(string regionId)
        => world.GetRegionHoldings(regionId);

        /// <summary>
        /// Gets the units.
        /// </summary>
        /// <returns>The units.</returns>
        public IEnumerable<Unit> GetUnits()
        => world.Units.Values;

        /// <summary>
        /// Builds the specified holding type in a region.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="holdingType">Holding type.</param>
        public void BuildHolding(string regionId, HoldingType holdingType)
        {
            Region region = world.Regions[regionId];


            if (RegionHasEmptyHoldingSlots(regionId))
            {
                world.AddHolding(regionId, holdingType);
                world.Factions[region.FactionId].Wealth -= HoldingsPrice;
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
            world.Armies.Values.FirstOrDefault(a => a.FactionId == factionId &&
                                                    a.UnitId == unitId).Size += amount;
        }

        /// <summary>
        /// Recruits the specified amount of troops of a unit for a faction.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        /// <param name="amount">Amount.</param>
        public void RecruitUnits(string factionId, string unitId, int amount)
        {
            if (world.Factions[factionId].Wealth < world.Units[unitId].Price * amount)
            {
                amount = world.Factions[factionId].Wealth / world.Units[unitId].Price;
            }

            AddUnits(factionId, unitId, amount);
            world.Factions[factionId].Wealth -= world.Units[unitId].Price * amount;
        }

        /// <summary>
        /// The player faction will attack the specified region.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        public BattleResult PlayerAttackRegion(string regionId)
        {
            BattleResult result = AttackRegion(PlayerFactionId, regionId);

            return result;
        }

        void InitializeGame(string factionId)
        {
            PlayerFactionId = factionId;
            Turn = 0;
        }

        void UpdateFactionsAliveStatus()
        {
            foreach (Faction faction in world.Factions.Values.Where(f => f.Id != "gaia"))
            {
                bool wasAlive = faction.Alive;

                faction.Alive = GetFactionRegionsCount(faction.Id) > 0;

                if (wasAlive && !faction.Alive && FactionDestroyed != null)
                {
                    FactionDestroyed(this, new FactionEventArgs(faction.Id));
                }
                else if (!wasAlive && faction.Alive && FactionRevived != null)
                {
                    FactionRevived(this, new FactionEventArgs(faction.Id));
                }
            }
        }

        void CheckForWinner()
        {
            if (world.Factions.Values.Count(f => f.Alive) > 1)
            {
                return;
            }

            Faction faction = world.Factions.Values.FirstOrDefault(f => f.Alive);

            if (FactionWon != null)
            {
                FactionWon(this, new FactionEventArgs(faction.Id));
            }
        }

        BattleResult AttackRegion(string factionId, string regionId)
        {
            Region region = world.Regions[regionId];
            string oldRegionFactionId = region.FactionId;

            BattleResult result = attack.AttackRegion(factionId, regionId);

            if (GetFactionRelation(factionId, oldRegionFactionId) > 0)
            {
                world.SetRelations(factionId, oldRegionFactionId, 0);
            }

            if (result == BattleResult.Victory)
            {
                world.ChangeRelations(factionId, oldRegionFactionId, -10);
            }

            switch (region.Type)
            {
                case RegionType.Capital:
                    world.ChangeRelations(factionId, oldRegionFactionId, -10);
                    break;

                case RegionType.Province:
                    world.ChangeRelations(factionId, oldRegionFactionId, -5);
                    break;
            }

            switch (region.State)
            {
                case RegionState.Sovereign:
                    world.ChangeRelations(factionId, oldRegionFactionId, -5);
                    break;

                case RegionState.Occupied:
                    world.ChangeRelations(factionId, oldRegionFactionId, -2);
                    break;
            }

            if (oldRegionFactionId == PlayerFactionId)
            {
                BattleEventArgs e = new BattleEventArgs(regionId, factionId, result);

                if (PlayerRegionAttacked != null)
                {
                    PlayerRegionAttacked(this, e);
                }
            }

            return result;
        }

        void AiBuild(string factionId)
        {
            while (GetFactionWealth(factionId) >= HoldingsPrice)
            {
                List<Region> validSovereignRegions = GetFactionRegions(factionId).Where(r => r.State == RegionState.Sovereign &&
                                                                                             RegionHasEmptyHoldingSlots(r.Id)).ToList();

                if (validSovereignRegions.Count < 1)
                {
                    break;
                }

                HoldingType type = Enum.GetValues(typeof(HoldingType)).Cast<HoldingType>().Where(x => x != HoldingType.Empty).RandomElement();

                BuildHolding(validSovereignRegions.RandomElement().Id, type);
            }

            while (GetFactionWealth(factionId) >= HoldingsPrice)
            {
                List<Region> validOccupiedRegions = GetFactionRegions(factionId).Where(r => r.State == RegionState.Occupied &&
                                                                                             RegionHasEmptyHoldingSlots(r.Id)).ToList();

                if (validOccupiedRegions.Count < 1)
                {
                    break;
                }

                HoldingType type = Enum.GetValues(typeof(HoldingType)).Cast<HoldingType>().Where(x => x != HoldingType.Empty).RandomElement();

                BuildHolding(validOccupiedRegions.RandomElement().Id, type);
            }
        }

        void AiRecruit(string factionId)
        {
            int minPrice = world.Units.Values.Min(u => u.Price);

            while (GetFactionWealth(factionId) >= minPrice)
            {
                string unitId = world.Units.Keys.RandomElement();

                RecruitUnits(factionId, unitId, 1);
            }
        }
    }
}
