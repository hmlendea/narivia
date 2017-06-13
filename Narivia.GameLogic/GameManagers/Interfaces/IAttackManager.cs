namespace Narivia.GameLogic.GameManagers.Interfaces
{
    public interface IAttackManager
    {
        /// <summary>
        /// Chooses the region to attack.
        /// </summary>
        /// <returns>The region to attack.</returns>
        /// <param name="factionId">Faction identifier.</param>
        string ChooseRegionToAttack(string factionId);

        /// <summary>
        /// Attacks the region.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="regionId">Region identifier.</param>
        void AttackRegion(string factionId, string regionId);
    }
}
