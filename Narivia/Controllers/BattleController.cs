using System;
using System.Collections.Generic;
using System.Linq;

using Narivia.Models;
using Narivia.Repositories;

namespace Narivia.Controllers
{
    public enum BattleResult
    {
        Victory,
        Defeat
    }

    public class BattleController
    {
        public FactionRepository FactionRepository { get; set; }

        public RegionRepository RegionRepository { get; set; }

        public UnitRepository UnitRepository { get; set; }

        public ArmyRepository ArmyRepository { get; set; }

        public BattleResult AttackRegion(string attackerFactionId, string defenderRegionId)
        {
            Random random = new Random();
            Region defenderRegion = RegionRepository.Get(defenderRegionId);
            Faction attackerFaction = FactionRepository.Get(attackerFactionId);
            Faction defenderFaction = FactionRepository.Get(defenderRegion.FactionId);

            int attackerTroops = ArmyRepository.GetFactionTroops(attackerFaction.Id);
            int defenderTroops = ArmyRepository.GetFactionTroops(defenderFaction.Id);

            while (attackerTroops > 0 && defenderTroops > 0)
            {
                List<Army> attackerArmies = ArmyRepository.GetAllByFaction(attackerFaction.Id);
                List<Army> defenderArmies = ArmyRepository.GetAllByFaction(defenderFaction.Id);

                int attackerUnitNumber = random.Next(attackerArmies.Count);
                int defenderUnitNumber = random.Next(defenderArmies.Count);

                while (attackerArmies.ElementAt(attackerUnitNumber).Size == 0)
                    attackerUnitNumber = random.Next(attackerArmies.Count);

                while (defenderArmies.ElementAt(defenderUnitNumber).Size == 0)
                    defenderUnitNumber = random.Next(defenderArmies.Count);

                string attackerUnitId = attackerArmies.ElementAt(attackerUnitNumber).UnitId;
                string defenderUnitId = defenderArmies.ElementAt(defenderUnitNumber).UnitId;

                Unit attackerUnit = UnitRepository.Get(attackerUnitId);
                Army attackerArmy = ArmyRepository.Get(attackerFactionId, attackerUnitId);

                Unit defenderUnit = UnitRepository.Get(defenderUnitId);
                Army defenderArmy = ArmyRepository.Get(defenderFaction.Id, defenderUnitId);


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

            // TODO: In the GameController I should change the realations based on wether the region was sovereign or not

            if (attackerTroops > defenderTroops)            
                return BattleResult.Victory;
            else
                return BattleResult.Defeat;
        }
    }
}
