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
            foreach (Faction faction in world.GetFactions().Where(f => f.Alive))
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

                string regionId = attack.ChooseRegionToAttack(faction.Id);

                if (GetFactionTroopsAmount(faction.Id) < MinTroopsPerAttack ||
                    string.IsNullOrEmpty(regionId))
                {
                    continue;
                }

                AttackRegion(faction.Id, regionId);
            }

            Parallel.ForEach(GetRegions(), region => region.Locked = false);

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
        => world.GetArmies();

        /// <summary>
        /// Gets the biome.
        /// </summary>
        /// <returns>The biome.</returns>
        /// <param name="biomeId">Biome identifier.</param>
        public Biome GetBiome(string biomeId)
        => GetBiomes().FirstOrDefault(b => b.Id == biomeId);

        public IEnumerable<Biome> GetBiomes()
        => world.GetBiomes();

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
        => world.GetCultures();

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
        => world.GetFactions();

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

            Parallel.ForEach(GetFactionRegions(factionId),
                             region => incomes.Add(GetRegionIncome(region.Id)));

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

            outcome += world.GetArmies()
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

            Parallel.ForEach(GetFactionRegions(factionId),
                             region => recruitments.Add(GetRegionRecruitment(region.Id)));

            return recruitments.Sum();
        }

        /// <summary>
        /// Gets the holdings of a faction.
        /// </summary>
        /// <returns>The holdings.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Holding> GetFactionHoldings(string factionId)
        => world.GetFactionHoldings(factionId);

        /// <summary>
        /// Gets the regions of a faction.
        /// </summary>
        /// <returns>The regions.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Region> GetFactionRegions(string factionId)
        => GetRegions().Where(r => r.FactionId == factionId);

        /// <summary>
        /// Gets the relations of a faction.
        /// </summary>
        /// <returns>The relations of a faction.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public IEnumerable<Relation> GetFactionRelations(string factionId)
        => world.GetFactionRelations(factionId);

        /// <summary>
        /// Gets the faction troops amount.
        /// </summary>
        /// <returns>The faction troops amount.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionTroopsAmount(string factionId)
        => world.GetFactionTroopsAmount(factionId);

        /// <summary>
        /// Gets the faction capital.
        /// </summary>
        /// <returns>The faction capital region.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public Region GetFactionCapital(string factionId)
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
        => world.GetFlags();

        /// <summary>
        /// Gets the holding.
        /// </summary>
        /// <returns>The holding.</returns>
        /// <param name="holdingId">Holding identifier.</param>
        public Holding GetHolding(string holdingId)
        => GetHoldings().FirstOrDefault(h => h.Id == holdingId);

        public IEnumerable<Holding> GetHoldings()
        => world.GetHoldings();

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <returns>The region.</returns>
        /// <param name="regionId">Region identifier.</param>
        public Region GetRegion(string regionId)
        => GetRegions().FirstOrDefault(r => r.Id == regionId);

        /// <summary>
        /// Gets the holdings of a region.
        /// </summary>
        /// <returns>The holdings.</returns>
        /// <param name="regionId">Region identifier.</param>
        public IEnumerable<Holding> GetRegionHoldings(string regionId)
        => world.GetRegionHoldings(regionId);

        /// <summary>
        /// Gets the income of a region.
        /// </summary>
        /// <returns>The region income.</returns>
        /// <param name="regionId">Region identifier.</param>
        public int GetRegionIncome(string regionId)
        {
            Region region = GetRegion(regionId);
            Resource resource = GetResource(region.ResourceId);

            List<Holding> holdings = world.GetRegionHoldings(region.Id).ToList();

            int income = BaseRegionIncome;

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
        /// Gets the recruitment of a region.
        /// </summary>
        /// <returns>The region recruitment.</returns>
        /// <param name="regionId">Region identifier.</param>
        public int GetRegionRecruitment(string regionId)
        {
            Region region = GetRegion(regionId);
            Resource resource = GetResource(region.ResourceId);

            List<Holding> holdings = world.GetRegionHoldings(region.Id).ToList();

            int recruitment = BaseRegionRecruitment;

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
        /// Gets the regions.
        /// </summary>
        /// <returns>The regions.</returns>
        public IEnumerable<Region> GetRegions()
        => world.GetRegions();


        /// <summary>
        /// Gets the relation between two factions.
        /// </summary>
        /// <returns>The faction relation.</returns>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        public Relation GetRelation(string sourceFactionId, string targetFactionId)
        => world.GetRelations().FirstOrDefault(r => r.SourceFactionId == sourceFactionId &&
                                                    r.TargetFactionId == targetFactionId);

        /// <summary>
        /// Gets the relations between factions.
        /// </summary>
        /// <returns>The relations.</returns>
        public IEnumerable<Relation> GetRelations()
        => world.GetRelations();

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
        => world.GetResources();

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
        => world.GetUnits();

        /// <summary>
        /// Gets the world geographic layers.
        /// </summary>
        /// <returns>The world geographic layers.</returns>
        public IEnumerable<WorldGeoLayer> GetWorldGeoLayers()
        => world.GetWorldGeoLayers();

        /// <summary>
        /// Gets the world tile.
        /// </summary>
        /// <returns>The world tile.</returns>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public WorldTile GetWorldTile(int x, int y)
        => world.WorldTiles[x, y];

        /// <summary>
        /// Sets the world tile.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="value">Value.</param>
        public void SetWorldTile(int x, int y, WorldTile value)
        => world.WorldTiles[x, y] = value;

        /// <summary>
        /// Builds the specified holding type in a region.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="holdingType">Holding type.</param>
        public void BuildHolding(string regionId, HoldingType holdingType)
        {
            Region region = GetRegion(regionId);


            if (RegionHasEmptyHoldingSlots(regionId))
            {
                world.AddHolding(regionId, holdingType);
                GetFaction(region.FactionId).Wealth -= HoldingsPrice;
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
            Parallel.ForEach(GetFactions().Where(f => f.Id != "gaia"),
                             faction =>
                             {
                                 bool wasAlive = faction.Alive;

                                 faction.Alive = GetFactionRegions(faction.Id).Count() > 0;

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

        BattleResult AttackRegion(string factionId, string regionId)
        {
            Region region = GetRegion(regionId);
            string oldRegionFactionId = region.FactionId;

            BattleResult result = attack.AttackRegion(factionId, regionId);

            if (GetRelation(factionId, oldRegionFactionId).Value > 0)
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
            Faction faction = GetFaction(factionId);

            while (faction.Wealth >= HoldingsPrice)
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

            while (faction.Wealth >= HoldingsPrice)
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
            int minPrice = GetUnits().Min(u => u.Price);

            while (GetFaction(factionId).Wealth >= minPrice)
            {
                string unitId = GetUnits().Select(u => u.Id).RandomElement();

                RecruitUnits(factionId, unitId, 1);
            }
        }
    }
}
