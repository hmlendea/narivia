using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Models;

namespace Narivia.GameLogic.GameManagers
{
    public class AttackManager : IAttackManager
    {
        const int BLITZKRIEG_SOVEREIGNTY_IMPORTANCE = 30;
        const int BLITZKRIEG_HOLDING_CASTLE_IMPORTANCE = 30;
        const int BLITZKRIEG_HOLDING_CITY_IMPORTANCE = 20;
        const int BLITZKRIEG_HOLDING_TEMPLE_IMPORTANCE = 10;
        const int BLITZKRIEG_BORDER_IMPORTANCE = 15;
        const int BLITZKRIEG_RESOURCE_ECONOMY_IMPORTANCE = 5;
        const int BLITZKRIEG_RESOURCE_MILITARY_IMPORTANCE = 10;

        readonly IWorldManager world;

        public AttackManager(IWorldManager world)
        {
            this.world = world;
        }

        /// <summary>
        /// Chooses the region to attack.
        /// </summary>
        /// <returns>The region to attack.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public string ChooseRegionToAttack(string factionId)
        {
            Random random = new Random();

            List<string> regionsOwnedIds = world.GetFactionRegions(factionId)
                                                .Select(x => x.Id)
                                                .ToList();

            Dictionary<string, int> targets = world.Regions.Values
                                                   .Where(r => r.FactionId != factionId)
                                                   .Select(x => x.Id)
                                                   .Except(regionsOwnedIds)
                                                   .Where(x => regionsOwnedIds.Any(y => world.RegionHasBorder(x, y)))
                                                   .ToDictionary(x => x, y => 0);



            Parallel.ForEach(world.Regions.Values.Where(r => targets.ContainsKey(r.Id)).ToList(), (region) =>
            {
                if (region.SovereignFactionId == factionId)
                {
                    targets[region.Id] += BLITZKRIEG_SOVEREIGNTY_IMPORTANCE;
                }


                Parallel.ForEach(world.GetRegionHoldings(region.Id), (holding) =>
                {
                    switch (holding.Type)
                    {
                        case HoldingType.Castle:
                            targets[region.Id] += BLITZKRIEG_HOLDING_CASTLE_IMPORTANCE;
                            break;

                        case HoldingType.City:
                            targets[region.Id] += BLITZKRIEG_HOLDING_CITY_IMPORTANCE;
                            break;

                        case HoldingType.Temple:
                            targets[region.Id] += BLITZKRIEG_HOLDING_TEMPLE_IMPORTANCE;
                            break;
                    }
                });

                Resource regionResource = world.Resources.Values.FirstOrDefault(x => x.Id == region.ResourceId);

                if (regionResource != null)
                {
                    switch (regionResource.Type)
                    {
                        case ResourceType.Military:
                            targets[region.Id] += BLITZKRIEG_RESOURCE_MILITARY_IMPORTANCE;
                            break;

                        case ResourceType.Economy:
                            targets[region.Id] += BLITZKRIEG_RESOURCE_ECONOMY_IMPORTANCE;
                            break;
                    }
                }

                targets[region.Id] += regionsOwnedIds.Count(x => world.RegionHasBorder(x, region.Id)) * BLITZKRIEG_BORDER_IMPORTANCE;

                // TODO: I should also take the relations into consideration

                // TODO: Maybe add a random importance to each region in order to reduce predictibility a little
            });

            if (targets.Count == 0)
            {
                return null;
            }

            int maxScore = targets.Max(x => x.Value);
            List<string> topTargets = targets.Keys.Where(x => targets[x] == maxScore).ToList();
            string regionId = topTargets[random.Next(0, topTargets.Count())];

            return regionId;
        }

        /// <summary>
        /// Attacks the region.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="regionId">Region identifier.</param>
        public void AttackRegion(string factionId, string regionId)
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
    }
}
