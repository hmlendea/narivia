namespace Narivia.BusinessLogic.DomainServices.Interfaces
{
    interface IBattleDomainService
    {
        /// <summary>
        /// Attacks the region.
        /// </summary>
        /// <returns>The battle result.</returns>
        /// <param name="attackerFactionId">Attacker faction identifier.</param>
        /// <param name="defenderRegionId">Defender region identifier.</param>
        BattleResult AttackRegion(string attackerFactionId, string defenderRegionId);
    }
}
