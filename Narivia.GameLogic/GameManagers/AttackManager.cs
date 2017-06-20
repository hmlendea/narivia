using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Narivia.GameLogic.Enumerations;
using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Infrastructure.Exceptions;
using Narivia.Infrastructure.Extensions;
using Narivia.Models;
using Narivia.Models.Enumerations;

namespace Narivia.GameLogic.GameManagers
{
    public class AttackManager : IAttackManager
    {
        Random random;

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

            random = new Random();
        }

        /// <summary>
        /// Chooses the region to attack.
        /// </summary>
        /// <returns>The region to attack.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public string ChooseRegionToAttack(string factionId)
        {
            List<string> regionsOwnedIds = world.GetFactionRegions(factionId)
                                                .Select(x => x.Id)
                                                .ToList();

            // TODO: Do not target factions with good relations
            Dictionary<string, int> targets = world.Regions.Values
                                                   .Where(r => r.FactionId != factionId &&
                                                               r.FactionId != "gaia" &&
                                                               r.Locked == false)
                                                   .Select(x => x.Id)
                                                   .Except(regionsOwnedIds)
                                                   .Where(x => regionsOwnedIds.Any(y => world.RegionBordersRegion(x, y)))
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

                targets[region.Id] += regionsOwnedIds.Count(x => world.RegionBordersRegion(x, region.Id)) * BLITZKRIEG_BORDER_IMPORTANCE;

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
        public BattleResult AttackRegion(string factionId, string regionId)
        {
            if (string.IsNullOrWhiteSpace(regionId) ||
                world.Regions[regionId].Locked ||
                !world.FactionBordersRegion(factionId, regionId))
            {
                throw new InvalidTargetRegionException(regionId);
            }

            Region targetRegion = world.Regions[regionId];

            Faction attackerFaction = world.Factions[factionId];
            Faction defenderFaction = world.Factions[targetRegion.FactionId];

            if (defenderFaction.Id == attackerFaction.Id ||
                defenderFaction.Id == "gaia")
            {
                throw new InvalidTargetRegionException(regionId);
            }

            world.SetRelations(attackerFaction.Id, defenderFaction.Id, 0);

            while (world.GetFactionTroopsCount(attackerFaction.Id) > 0 &&
                   world.GetFactionTroopsCount(defenderFaction.Id) > 0)
            {
                Army attackerArmy = world.GetFactionArmies(attackerFaction.Id)
                                         .Where(a => a.Size > 0)
                                         .RandomElement();
                Army defenderArmy = world.GetFactionArmies(defenderFaction.Id)
                                         .Where(a => a.Size > 0)
                                         .RandomElement();

                Unit attackerUnit = world.Units[attackerArmy.UnitId];
                Unit defenderUnit = world.Units[defenderArmy.UnitId];

                // TODO: Attack and Defence bonuses

                int attackerTroopsLeft =
                    (attackerUnit.Health * attackerArmy.Size - defenderUnit.Power * defenderArmy.Size) /
                    attackerUnit.Health;

                int defenderTroopsLeft =
                    (defenderUnit.Health * defenderArmy.Size - attackerUnit.Power * attackerArmy.Size) /
                    defenderUnit.Health;

                attackerArmy.Size = Math.Max(0, attackerTroopsLeft);
                defenderArmy.Size = Math.Max(0, defenderTroopsLeft);
            }

            // TODO: In the GameDomainService I should change the realations based on wether the
            // region was sovereign or not

            if (world.GetFactionTroopsCount(attackerFaction.Id) >
                world.GetFactionTroopsCount(defenderFaction.Id))
            {
                world.TransferRegion(regionId, factionId);
                world.Regions[regionId].Locked = true;

                return BattleResult.Victory;
            }

            return BattleResult.Defeat;
        }
    }
}
