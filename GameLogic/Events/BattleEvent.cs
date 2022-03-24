using Narivia.GameLogic.Enumerations;

namespace Narivia.GameLogic.Events
{
    /// <summary>
    /// Battle event handler.
    /// </summary>
    public delegate void BattleEventHandler(object sender, BattleEventArgs e);

    /// <summary>
    /// Battle event arguments.
    /// </summary>
    public class BattleEventArgs
    {
        /// <summary>
        /// The province identifier.
        /// </summary>
        public string ProvinceId { get; private set; }

        /// <summary>
        /// The attacker faction identifier.
        /// </summary>
        public string AttackerFactionId { get; private set; }

        /// <summary>
        /// The battle result.
        /// </summary>
        public BattleResult BattleResult { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleEventArgs"/> class.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        /// <param name="attackerFactionId">Attacker faction identifier.</param>
        /// <param name="battleResult">Battle result.</param>
        public BattleEventArgs(string provinceId, string attackerFactionId, BattleResult battleResult)
        {
            ProvinceId = provinceId;
            AttackerFactionId = attackerFactionId;
            BattleResult = battleResult;
        }
    }
}
