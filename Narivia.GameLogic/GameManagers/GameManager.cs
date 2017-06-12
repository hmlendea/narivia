using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Models;

namespace Narivia.GameLogic.GameManagers
{
    /// <summary>
    /// Game manager.
    /// </summary>
    public class GameManager : IGameManager
    {
        IWorldManager world;

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

            InitializeGame(factionId);
            InitializeEntities();
        }

        /// <summary>
        /// Advances the game by one turn.
        /// </summary>
        public void NextTurn()
        {
            AttackManager attackManager = new AttackManager(world.Borders.Values,
                                                            world.Holdings.Values,
                                                            world.Regions.Values,
                                                            world.Resources.Values);

            foreach (Faction faction in world.Factions.Values.Where(f => f.Alive))
            {
                if (GetFactionRegionsCount(faction.Id) == 0)
                {
                    faction.Alive = false;
                    continue;
                }

                // Economy
                faction.Wealth += GetFactionIncome(faction.Id);
                faction.Wealth -= GetFactionOutcome(faction.Id);

                // Build

                // Recruit
                // TODO: Find a way around the hardcoded "militia" unit identifier
                world.Armies.Values.FirstOrDefault(u => u.FactionId == faction.Id &&
                                                        u.UnitId == "militia")
                                   .Size += GetFactionRecruitment(faction.Id);

                //string targetRegionId = ChooseAttack(faction.Id);
                //AttackRegion(faction.Id, targetRegionId);
            }

            Turn += 1;
        }

        /// <summary>
        /// Checks wether the specified regions share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified regions share a border, <c>false</c> otherwise.</returns>
        /// <param name="region1Id">First region identifier.</param>
        /// <param name="region2Id">Second region identifier.</param>
        public bool RegionHasBorder(string region1Id, string region2Id)
        {
            return world.RegionHasBorder(region1Id, region2Id);
        }

        /// <summary>
        /// Checks wether the specified regions share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified regions share a border, <c>false</c> otherwise.</returns>
        /// <param name="faction1Id">First faction identifier.</param>
        /// <param name="faction2Id">Second faction identifier.</param>
        public bool FactionHasBorder(string faction1Id, string faction2Id)
        {
            return world.FactionHasBorder(faction1Id, faction2Id);
        }

        /// <summary>
        /// Returns the faction identifier at the given position.
        /// </summary>
        /// <returns>The faction identifier.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public string FactionIdAtPosition(int x, int y)
        {
            return world.FactionIdAtPosition(x, y);
        }

        /// <summary>
        /// Returns the faction colour at the given position.
        /// </summary>
        /// <returns>The faction colour.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Colour FactionColourAtPosition(int x, int y)
        {
            return world.Factions[world.FactionIdAtPosition(x, y)].Colour;
        }

        /// <summary>
        /// Transfers the specified region to the specified faction.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        public void TransferRegion(string regionId, string factionId)
        {
            world.TransferRegion(regionId, factionId);
        }

        /// <summary>
        /// Gets the name of the faction.
        /// </summary>
        /// <returns>The faction name.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public string GetFactionName(string factionId)
        {
            return world.Factions.Values.FirstOrDefault(f => f.Id == factionId).Name;
        }

        /// <summary>
        /// Gets the faction income.
        /// </summary>
        /// <returns>The faction income.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionIncome(string factionId)
        {
            int income = 0;

            income += world.Regions.Values.Count(r => r.FactionId == factionId) * BaseRegionIncome;
            // TODO: Also calculate the holdings income

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
            // TODO: Also calculate the holdings outcome

            return outcome;
        }

        /// <summary>
        /// Gets the faction recruitment.
        /// </summary>
        /// <returns>The faction recruitment.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionRecruitment(string factionId)
        {
            int recruitment = 0;

            recruitment += world.Regions.Values.Count(r => r.FactionId == factionId) * BaseRegionRecruitment;
            recruitment += BaseFactionRecruitment;
            // TODO: Also calculate the holdings recruitment

            return recruitment;
        }

        /// <summary>
        /// Gets the faction regions count.
        /// </summary>
        /// <returns>The faction regions count.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionRegionsCount(string factionId)
        {
            return world.Regions.Values.Count(r => r.FactionId == factionId);
        }

        /// <summary>
        /// Gets the faction holdings count.
        /// </summary>
        /// <returns>The faction holdings count.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionHoldingsCount(string factionId)
        {
            return world.Holdings.Values.Count(h => h.Type != HoldingType.Empty &&
                                                    world.Regions[h.RegionId].FactionId == factionId);
        }

        /// <summary>
        /// Gets the faction wealth.
        /// </summary>
        /// <returns>The faction wealth.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionWealth(string factionId)
        {
            return world.Factions.Values.FirstOrDefault(f => f.Id == factionId).Wealth;
        }

        /// <summary>
        /// Gets the faction troops count.
        /// </summary>
        /// <returns>The faction troops count.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public int GetFactionTroopsCount(string factionId)
        {
            return world.Armies.Values
                         .Where(x => x.FactionId == factionId)
                         .Sum(x => x.Size);
        }

        /// <summary>
        /// Gets the faction capital.
        /// </summary>
        /// <returns>The faction capital.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public string GetFactionCapital(string factionId)
        {
            return world.GetFactionCapital(factionId);
        }

        /// <summary>
        /// Gets the region holdings.
        /// </summary>
        /// <returns>The region holdings.</returns>
        /// <param name="regionId">Region identifier.</param>
        public IEnumerable<Holding> GetRegionHoldings(string regionId)
        {
            return world.Holdings.Values.Where(h => h.Type != HoldingType.Empty &&
                                                    h.RegionId == regionId);
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
            int costPerTroop = world.Units[unitId].Price;
            int factionWealth = world.Factions[factionId].Wealth;

            if (factionWealth < costPerTroop * amount)
            {
                // TODO: Log warning or something
                amount = factionWealth / costPerTroop;
            }

            AddUnits(factionId, unitId, amount);
        }

        void InitializeGame(string factionId)
        {
            PlayerFactionId = factionId;
            Turn = 0;
        }

        void InitializeEntities()
        {
            foreach (Faction faction in world.Factions.Values)
            {
                faction.Wealth = StartingWealth;
                faction.Alive = true;

                foreach (Unit unit in world.Units.Values.ToList())
                {
                    Tuple<string, string> armyKey = new Tuple<string, string>(faction.Id, unit.Id);
                    Army army = new Army
                    {
                        FactionId = faction.Id,
                        UnitId = unit.Id,
                        Size = StartingTroopsPerUnit
                    };

                    world.Armies.Add(armyKey, army);
                }
            }

            world.Regions.Values.Where(r => string.IsNullOrEmpty(r.SovereignFactionId)).ToList().ForEach(r => r.SovereignFactionId = r.FactionId);

            world.Factions["gaia"].Alive = false;
        }

        /// <summary>
        /// WIP blitzkrieg sequencial algorithm for invading a faction.
        /// </summary>
        /// <returns>The region identifier.</returns>
        /// <param name="factionId">Attacking faction identifier.</param>
        /// <param name="targetFactionId">Targeted faction identifier.</param>
        public string Blitzkrieg_Seq(string factionId, string targetFactionId)
        {
            AttackManager attackManager = new AttackManager(world.Borders.Values,
                                                            world.Holdings.Values,
                                                            world.Regions.Values,
                                                            world.Resources.Values);

            return attackManager.GetNextRegion_Seq(factionId, targetFactionId);
        }

        // TODO: Move this from here
        /// <summary>
        /// WIP blitzkrieg parallelized algorithm for invading a faction.
        /// </summary>
        /// <returns>The region identifier.</returns>
        /// <param name="factionId">Attacking faction identifier.</param>
        /// <param name="targetFactionId">Targeted faction identifier.</param>
        public string Blitzkrieg_Parallel(string factionId, string targetFactionId)
        {
            AttackManager attackManager = new AttackManager(world.Borders.Values,
                                                            world.Holdings.Values,
                                                            world.Regions.Values,
                                                            world.Resources.Values);

            return attackManager.GetNextRegion_Parallel(factionId, targetFactionId);
        }

        // TODO: Move this from here
        /// <summary>
        /// WIP blitzkrieg sequencial algorithm for invading a faction.
        /// </summary>
        public void Blitzkrieg_AllFactions_Seq()
        {
            while (world.Regions.Values.Count(x => x.FactionId != "gaia" &&
                                             world.Regions.Values.Any(y => y.FactionId == x.Id)) > 1)
            {
                Faction attackerFaction = world.Factions.Values.FirstOrDefault(x => x.Id != "gaia" &&
                                                                              world.Regions.Values.Any(y => y.FactionId == x.Id));
                Faction targetedFaction = world.Factions.Values.FirstOrDefault(x => x.Id != "gaia" &&
                                                                              x.Id != attackerFaction.Id &&
                                                                              world.Regions.Values.Any(y => y.FactionId == x.Id) &&
                                                                              FactionHasBorder(attackerFaction.Id, x.Id));

                if (targetedFaction == null)
                {
                    break;
                }

                //Console.WriteLine(attackerFaction.Id + " attacks " + targetedFaction.Id);

                AttackManager attackManager = new AttackManager(world.Borders.Values,
                                                                world.Holdings.Values,
                                                                world.Regions.Values,
                                                                world.Resources.Values);
                string regionId = string.Empty;

                while (regionId != null)
                {
                    regionId = attackManager.GetNextRegion_Seq(attackerFaction.Id, targetedFaction.Id);
                }
            }
        }

        // TODO: Move this from here
        /// <summary>
        /// WIP blitzkrieg parallelized algorithm for invading a faction.
        /// </summary>
        public void Blitzkrieg_AllFactions_Parallel()
        {
            while (world.Regions.Values.Any(x => x.FactionId != "gaia" &&
                                           world.Regions.Values.Any(y => y.FactionId == x.Id)))
            {
                Faction attackerFaction = world.Factions.Values.FirstOrDefault(x => x.Id != "gaia" &&
                                                                              world.Regions.Values.Any(y => y.FactionId == x.Id));
                Faction targetedFaction = world.Factions.Values.FirstOrDefault(x => x.Id != "gaia" &&
                                                                              x.Id != attackerFaction.Id &&
                                                                              world.Regions.Values.Any(y => y.FactionId == x.Id) &&
                                                                              FactionHasBorder(attackerFaction.Id, x.Id));

                if (targetedFaction == null)
                {
                    break;
                }

                //Console.WriteLine(attackerFaction.Id + " attacks " + targetedFaction.Id);

                AttackManager attackManager = new AttackManager(world.Borders.Values,
                                                                world.Holdings.Values,
                                                                world.Regions.Values,
                                                                world.Resources.Values);
                string regionId = string.Empty;

                while (regionId != null)
                {
                    regionId = attackManager.GetNextRegion_Parallel(attackerFaction.Id, targetedFaction.Id);
                }
            }
        }

        // TODO: Replace this with Blitzkrieg
        /// <summary>
        /// Chooses a region to attack.
        /// </summary>
        /// <returns>The region identifier.</returns>
        /// <param name="factionId">Faction identifier.</param>
        string ChooseAttack(string factionId)
        {
            Random random = new Random();
            List<Region> regionsOwned = world.Regions.Values.Where(x => x.FactionId == factionId).ToList();
            List<string> choices = new List<string>();

            foreach (Region region in regionsOwned)
            {
                List<Border> regionBorders = world.Borders.Values
                                                    .Where(x => x.Region1Id == region.Id ||
                                                                x.Region2Id == region.Id)
                                                    .ToList();

                foreach (Border border in regionBorders)
                {
                    Region region2 = world.Regions[border.Region2Id];

                    if (region2.Id != region.Id && region2.FactionId != "gaia")
                    {
                        choices.Add(region2.Id);
                    }
                }
            }

            while (choices.Count > 0)
            {
                string regionId = choices[random.Next(choices.Count)];
                Region region = world.Regions[regionId];

                if (region.FactionId != "gaia")
                {
                    return regionId;
                }

                choices.Remove(regionId);
            }

            // TODO: Better handling of no region to attack
            return null;
        }

        void AttackRegion(string factionId, string regionId)
        {
            Random random = new Random();

            Region targetRegion = world.Regions[regionId];
            Faction attackerFaction = world.Factions[factionId];
            Faction defenderFaction = world.Factions[targetRegion.FactionId];

            List<Army> attackerArmies = world.Armies.Values.Where(army => army.FactionId == attackerFaction.Id).ToList();
            List<Army> defenderArmies = world.Armies.Values.Where(army => army.FactionId == defenderFaction.Id).ToList();

            int attackerTroops = attackerArmies.Select(y => y.Size).Sum();
            int defenderTroops = defenderArmies.Select(y => y.Size).Sum();

            while (attackerTroops > 0 && defenderTroops > 0)
            {
                int attackerUnitNumber = random.Next(attackerArmies.Count);
                int defenderUnitNumber = random.Next(defenderArmies.Count);

                while (attackerArmies.ElementAt(attackerUnitNumber).Size == 0)
                {
                    attackerUnitNumber = random.Next(attackerArmies.Count);
                }

                while (defenderArmies.ElementAt(defenderUnitNumber).Size == 0)
                {
                    defenderUnitNumber = random.Next(defenderArmies.Count);
                }

                string attackerUnitId = attackerArmies.ElementAt(attackerUnitNumber).UnitId;
                string defenderUnitId = defenderArmies.ElementAt(defenderUnitNumber).UnitId;

                Unit attackerUnit = world.Units.Values.FirstOrDefault(unit => unit.Id == attackerUnitId);
                Army attackerArmy = attackerArmies.FirstOrDefault(army => army.FactionId == attackerFaction.Id);

                Unit defenderUnit = world.Units.Values.FirstOrDefault(unit => unit.Id == defenderUnitId);
                Army defenderArmy = defenderArmies.FirstOrDefault(army => army.FactionId == defenderFaction.Id);


                // TODO: Attack and Defence bonuses

                attackerArmy.Size =
                    (attackerUnit.Health * attackerArmy.Size - defenderUnit.Power * defenderArmy.Size) /
                    attackerUnit.Health;

                defenderArmy.Size =
                    (defenderUnit.Health * defenderArmy.Size - attackerUnit.Power * attackerArmy.Size) /
                    defenderUnit.Health;

                attackerArmy.Size = Math.Max(0, attackerArmy.Size);
                defenderArmy.Size = Math.Max(0, defenderArmy.Size);
            }

            // TODO: In the GameDomainService I should change the realations based on wether the
            // region was sovereign or not

            if (attackerTroops > defenderTroops)
            {
                Console.WriteLine($"{factionId} won in {regionId}");
                world.TransferRegion(regionId, factionId);
            }

            // TODO: Do something when the attack failed
            Console.WriteLine($"{factionId} lost in {regionId}");
        }

        /// <summary>
        /// Changes the relations between two factions.
        /// </summary>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        /// <param name="delta">Relations value delta.</param>
        void ChangeRelations(string sourceFactionId, string targetFactionId, int delta)
        {
            Relation sourceRelation = world.Relations.Values.FirstOrDefault(r => r.SourceFactionId == sourceFactionId &&
                                                                                 r.TargetFactionId == targetFactionId);
            Relation targetRelation = world.Relations.Values.FirstOrDefault(r => r.SourceFactionId == targetFactionId &&
                                                                                 r.TargetFactionId == sourceFactionId);

            int oldRelations = sourceRelation.Value;
            sourceRelation.Value = Math.Max(-100, Math.Min(sourceRelation.Value + delta, 100));
            targetRelation.Value = sourceRelation.Value;
        }
    }
}
