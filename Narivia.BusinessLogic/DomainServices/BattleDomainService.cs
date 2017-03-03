using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.BusinessLogic.DomainServices.Interfaces;
using Narivia.BusinessLogic.Mapping;
using Narivia.DataAccess.Repositories.Interfaces;
using Narivia.Models;

namespace Narivia.BusinessLogic.DomainServices
{
    public enum BattleResult
    {
        Victory,
        Defeat
    }

    public class BattleDomainService : IBattleDomainService
    {
        public IArmyRepository ArmyRepository { get; set; }

        public IFactionRepository FactionRepository { get; set; }

        public IRegionRepository RegionRepository { get; set; }

        public IUnitRepository UnitRepository { get; set; }

        /// <summary>
        /// Attacks the region.
        /// </summary>
        /// <returns>The battle result.</returns>
        /// <param name="attackerFactionId">Attacker faction identifier.</param>
        /// <param name="defenderRegionId">Defender region identifier.</param>
        public BattleResult AttackRegion(string attackerFactionId, string defenderRegionId)
        {
            Random random = new Random();
            Region defenderRegion = RegionRepository.Get(defenderRegionId).ToDomainModel();
            Faction attackerFaction = FactionRepository.Get(attackerFactionId).ToDomainModel();
            Faction defenderFaction = FactionRepository.Get(defenderRegion.FactionId).ToDomainModel();

            int attackerTroops = ArmyRepository.GetAll()
                .Where(x => x.FactionId == attackerFaction.Id)
                .Select(y => y.Size).Sum();

            int defenderTroops = ArmyRepository.GetAll()
                .Where(x => x.FactionId == defenderFaction.Id)
                .Select(y => y.Size).Sum();

            while (attackerTroops > 0 && defenderTroops > 0)
            {
                List<Army> attackerArmies = ArmyRepository.GetAll()
                    .Where(x => x.FactionId == attackerFaction.Id)
                    .ToDomainModels().ToList();
                List<Army> defenderArmies = ArmyRepository.GetAll()
                    .Where(x => x.FactionId == defenderFaction.Id)
                    .ToDomainModels().ToList();

                int attackerUnitNumber = random.Next(attackerArmies.Count);
                int defenderUnitNumber = random.Next(defenderArmies.Count);

                while (attackerArmies.ElementAt(attackerUnitNumber).Size == 0)
                    attackerUnitNumber = random.Next(attackerArmies.Count);

                while (defenderArmies.ElementAt(defenderUnitNumber).Size == 0)
                    defenderUnitNumber = random.Next(defenderArmies.Count);

                string attackerUnitId = attackerArmies.ElementAt(attackerUnitNumber).UnitId;
                string defenderUnitId = defenderArmies.ElementAt(defenderUnitNumber).UnitId;

                Unit attackerUnit = UnitRepository.Get(attackerUnitId).ToDomainModel();
                Army attackerArmy = ArmyRepository.Get(attackerFactionId, attackerUnitId).ToDomainModel();

                Unit defenderUnit = UnitRepository.Get(defenderUnitId).ToDomainModel();
                Army defenderArmy = ArmyRepository.Get(defenderFaction.Id, defenderUnitId).ToDomainModel();


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
                return BattleResult.Victory;
            else
                return BattleResult.Defeat;
        }
    }
}
