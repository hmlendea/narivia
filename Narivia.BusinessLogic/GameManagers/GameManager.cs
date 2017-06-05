using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.BusinessLogic.GameManagers.Interfaces;
using Narivia.Models;

namespace Narivia.BusinessLogic.GameManagers
{
    public class GameDomainService : IGameDomainService
    {
        WorldManager world;

        public string[,] WorldTiles
        {
            get { return world.WorldTiles; }
            set { world.WorldTiles = value; }
        }

        public int WorldWidth => world.WorldWidth;
        public int WorldHeight => world.WorldHeight;
        public string WorldName => world.WorldName;
        public string WorldId => world.WorldId;

        public string PlayerFactionId { get; private set; }

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
            List<Faction> factionList = world.Factions.Values.ToList();

            foreach (Faction faction in factionList)
            {
                // TODO: Process AI
                // Economy
                // Build
                // Recruit

                string targetRegionId = ChooseAttack(faction.Id);
                AttackRegion(faction.Id, targetRegionId);
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
        /// Transfers the specified region to the specified faction.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        public void TransferRegion(string regionId, string factionId)
        {
            world.TransferRegion(regionId, factionId);
        }

        public string GetFactionName(string factionId)
        {
            return world.Factions.Values.FirstOrDefault(f => f.Id == factionId).Name;
        }

        public int GetFactionSize(string factionId)
        {
            return world.Regions.Values.Count(r => r.FactionId == factionId);
        }

        public int GetFactionWealth(string factionId)
        {
            return world.Factions.Values.FirstOrDefault(f => f.Id == factionId).Wealth;
        }

        public int GetFactionTroops(string factionId)
        {
            return world.Armies.Values
                         .Where(x => x.FactionId == factionId)
                         .Sum(x => x.Size);
        }

        public List<Biome> GetAllBiomes()
        {
            return world.Biomes.Values.ToList();
        }

        void InitializeGame(string factionId)
        {
            PlayerFactionId = factionId;
            Turn = 0;
        }

        void InitializeEntities()
        {
            foreach (Faction faction in world.Factions.Values.ToList())
            {
                // TODO: set starting wealth
                faction.Wealth = 0;

                foreach (Unit unit in world.Units.Values.ToList())
                {
                    Tuple<string, string> armyKey = new Tuple<string, string>(faction.Id, unit.Id);
                    Army army = new Army
                    {
                        FactionId = faction.Id,
                        UnitId = unit.Id,
                        Size = 0
                    };

                    world.Armies.Add(armyKey, army);
                }
            }
        }

        /// <summary>
        /// WIP blitzkrieg sequencial algorithm for invading a faction.
        /// </summary>
        /// <returns>The region identifier.</returns>
        /// <param name="factionId">Attacking faction identifier.</param>
        /// <param name="targetFactionId">Targeted faction identifier.</param>
        /// <summary>
        public string Blitzkrieg_Seq(string factionId, string targetFactionId)
        {
            AttackManager attackManager = new AttackManager(world.Borders.Values,
                                                            world.Holdings.Values,
                                                            world.Regions.Values,
                                                            world.Resources.Values);

            return attackManager.GetNextRegion_Seq(factionId, targetFactionId);
        }

        /// <summary>
        /// WIP blitzkrieg parallelized algorithm for invading a faction.
        /// </summary>
        /// <returns>The region identifier.</returns>
        /// <param name="factionId">Attacking faction identifier.</param>
        /// <param name="targetFactionId">Targeted faction identifier.</param>
        /// <summary>
        public string Blitzkrieg_Parallel(string factionId, string targetFactionId)
        {
            AttackManager attackManager = new AttackManager(world.Borders.Values,
                                                            world.Holdings.Values,
                                                            world.Regions.Values,
                                                            world.Resources.Values);

            return attackManager.GetNextRegion_Parallel(factionId, targetFactionId);
        }



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
                // TODO: Do something
            }

            // TODO: Do something
        }
    }
}
