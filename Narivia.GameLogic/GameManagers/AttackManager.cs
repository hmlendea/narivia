using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Narivia.DataAccess.Exceptions;
using Narivia.GameLogic.Enumerations;
using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Common.Extensions;
using Narivia.Models;
using Narivia.Models.Enumerations;
using Narivia.Settings;

namespace Narivia.GameLogic.GameManagers
{
    /// <summary>
    /// Attack manager.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AttackManager"/> class.
        /// </summary>
        /// <param name="world">World.</param>
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
            Dictionary<string, int> targets = world.GetRegions()
                                                   .Where(r => r.FactionId != factionId &&
                                                               r.FactionId != GameDefines.GAIA_FACTION &&
                                                               r.Locked == false)
                                                   .Select(x => x.Id)
                                                   .Except(regionsOwnedIds)
                                                   .Where(x => regionsOwnedIds.Any(y => world.RegionBordersRegion(x, y)))
                                                   .ToDictionary(x => x, y => 0);

            Parallel.ForEach(world.GetRegions().Where(r => targets.ContainsKey(r.Id)).ToList(), (region) =>
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

                Resource regionResource = world.GetResources().FirstOrDefault(x => x.Id == region.ResourceId);

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
                targets[region.Id] -= world.GetFactionRelation(factionId, region.FactionId);

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
            Region targetRegion = world.GetRegions().FirstOrDefault(r => r.Id == regionId);

            if (string.IsNullOrWhiteSpace(regionId) ||
                targetRegion.Locked ||
                !world.FactionBordersRegion(factionId, regionId))
            {
                throw new InvalidTargetRegionException(regionId);
            }

            Faction attackerFaction = world.GetFactions().FirstOrDefault(f => f.Id == factionId);
            Faction defenderFaction = world.GetFactions().FirstOrDefault(f => f.Id == targetRegion.FactionId);

            if (defenderFaction.Id == attackerFaction.Id ||
                defenderFaction.Id == GameDefines.GAIA_FACTION)
            {
                throw new InvalidTargetRegionException(regionId);
            }

            while (world.GetFactionTroopsAmount(attackerFaction.Id) > 0 &&
                   world.GetFactionTroopsAmount(defenderFaction.Id) > 0)
            {
                Army attackerArmy = world.GetFactionArmies(attackerFaction.Id)
                                         .Where(a => a.Size > 0)
                                         .RandomElement();
                Army defenderArmy = world.GetFactionArmies(defenderFaction.Id)
                                         .Where(a => a.Size > 0)
                                         .RandomElement();

                Unit attackerUnit = world.GetUnits().FirstOrDefault(u => u.Id == attackerArmy.UnitId);
                Unit defenderUnit = world.GetUnits().FirstOrDefault(u => u.Id == defenderArmy.UnitId);

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

            if (world.GetFactionTroopsAmount(attackerFaction.Id) >
                world.GetFactionTroopsAmount(defenderFaction.Id))
            {
                world.TransferRegion(regionId, factionId);
                targetRegion.Locked = true;

                return BattleResult.Victory;
            }

            return BattleResult.Defeat;
        }
    }
}
