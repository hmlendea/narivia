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
        /// Chooses the province to attack.
        /// </summary>
        /// <returns>The province to attack.</returns>
        /// <param name="factionId">Faction identifier.</param>
        public string ChooseProvinceToAttack(string factionId)
        {
            List<string> provincesOwnedIds = world.GetFactionProvinces(factionId)
                                                .Select(x => x.Id)
                                                .ToList();

            // TODO: Do not target factions with good relations
            Dictionary<string, int> targets = world.GetProvinces()
                                                   .Where(r => r.FactionId != factionId &&
                                                               r.FactionId != GameDefines.GAIA_FACTION &&
                                                               r.Locked == false)
                                                   .Select(x => x.Id)
                                                   .Except(provincesOwnedIds)
                                                   .Where(x => provincesOwnedIds.Any(y => world.ProvinceBordersProvince(x, y)))
                                                   .ToDictionary(x => x, y => 0);

            Parallel.ForEach(world.GetProvinces().Where(r => targets.ContainsKey(r.Id)).ToList(), (province) =>
            {
                if (province.SovereignFactionId == factionId)
                {
                    targets[province.Id] += BLITZKRIEG_SOVEREIGNTY_IMPORTANCE;
                }


                Parallel.ForEach(world.GetProvinceHoldings(province.Id), (holding) =>
                {
                    switch (holding.Type)
                    {
                        case HoldingType.Castle:
                            targets[province.Id] += BLITZKRIEG_HOLDING_CASTLE_IMPORTANCE;
                            break;

                        case HoldingType.City:
                            targets[province.Id] += BLITZKRIEG_HOLDING_CITY_IMPORTANCE;
                            break;

                        case HoldingType.Temple:
                            targets[province.Id] += BLITZKRIEG_HOLDING_TEMPLE_IMPORTANCE;
                            break;
                    }
                });

                Resource provinceResource = world.GetResources().FirstOrDefault(x => x.Id == province.ResourceId);

                if (provinceResource != null)
                {
                    switch (provinceResource.Type)
                    {
                        case ResourceType.Military:
                            targets[province.Id] += BLITZKRIEG_RESOURCE_MILITARY_IMPORTANCE;
                            break;

                        case ResourceType.Economy:
                            targets[province.Id] += BLITZKRIEG_RESOURCE_ECONOMY_IMPORTANCE;
                            break;
                    }
                }

                targets[province.Id] += provincesOwnedIds.Count(x => world.ProvinceBordersProvince(x, province.Id)) * BLITZKRIEG_BORDER_IMPORTANCE;
                targets[province.Id] -= world.GetFactionRelations(factionId)
                                           .FirstOrDefault(r => r.TargetFactionId == province.FactionId)
                                           .Value;

                // TODO: Maybe add a random importance to each province in order to reduce predictibility a little
            });

            if (targets.Count == 0)
            {
                return null;
            }

            int maxScore = targets.Max(x => x.Value);
            List<string> topTargets = targets.Keys.Where(x => targets[x] == maxScore).ToList();
            string provinceId = topTargets[random.Next(0, topTargets.Count())];

            return provinceId;
        }

        /// <summary>
        /// Attacks the province.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="provinceId">Province identifier.</param>
        public BattleResult AttackProvince(string factionId, string provinceId)
        {
            Province targetProvince = world.GetProvinces().FirstOrDefault(r => r.Id == provinceId);

            if (string.IsNullOrWhiteSpace(provinceId) ||
                targetProvince.Locked ||
                !world.FactionBordersProvince(factionId, provinceId))
            {
                throw new InvalidTargetProvinceException(provinceId);
            }

            Faction attackerFaction = world.GetFactions().FirstOrDefault(f => f.Id == factionId);
            Faction defenderFaction = world.GetFactions().FirstOrDefault(f => f.Id == targetProvince.FactionId);

            if (defenderFaction.Id == attackerFaction.Id ||
                defenderFaction.Id == GameDefines.GAIA_FACTION)
            {
                throw new InvalidTargetProvinceException(provinceId);
            }

            while (world.GetFactionTroopsAmount(attackerFaction.Id) > 0 &&
                   world.GetFactionTroopsAmount(defenderFaction.Id) > 0)
            {
                Army attackerArmy = world.GetFactionArmies(attackerFaction.Id)
                                         .Where(a => a.Size > 0)
                                         .GetRandomElement();
                Army defenderArmy = world.GetFactionArmies(defenderFaction.Id)
                                         .Where(a => a.Size > 0)
                                         .GetRandomElement();

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
            // province was sovereign or not

            if (world.GetFactionTroopsAmount(attackerFaction.Id) >
                world.GetFactionTroopsAmount(defenderFaction.Id))
            {
                world.TransferProvince(provinceId, factionId);
                targetProvince.Locked = true;

                return BattleResult.Victory;
            }

            return BattleResult.Defeat;
        }
    }
}
