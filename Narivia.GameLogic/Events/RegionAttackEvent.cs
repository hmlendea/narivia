using Narivia.GameLogic.Enumerations;

namespace Narivia.GameLogic.Events
{
    public delegate void RegionAttackEventHandler(object sender, RegionAttackEventArgs e);

    public class RegionAttackEventArgs
    {
        /// <summary>
        /// The region identifier.
        /// </summary>
        public string RegionId { get; private set; }

        /// <summary>
        /// The attacker faction identifier.
        /// </summary>
        public string AttackerFactionId { get; private set; }

        /// <summary>
        /// The battle result.
        /// </summary>
        public BattleResult BattleResult { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionAttackEventArgs"/> class.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="attackerFactionId">Attacker faction identifier.</param>
        /// <param name="battleResult">Battle result.</param>
        public RegionAttackEventArgs(string regionId, string attackerFactionId, BattleResult battleResult)
        {
            RegionId = regionId;
            AttackerFactionId = attackerFactionId;
            BattleResult = battleResult;
        }
    }
}
