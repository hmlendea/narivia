using Narivia.GameLogic.Enumerations;

namespace Narivia.GameLogic.GameManagers
{
    public interface IAttackManager
    {
        void LoadContent();

        void UnloadContent();

        /// <summary>
        /// Finds a province to attack.
        /// </summary>
        /// <returns>The province to attack.</returns>
        /// <param name="factionId">Faction identifier.</param>
        string FindProviceToAttack(string factionId);

        /// <summary>
        /// Attacks the province.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="provinceId">Province identifier.</param>
        BattleResult AttackProvince(string factionId, string provinceId);
    }
}
