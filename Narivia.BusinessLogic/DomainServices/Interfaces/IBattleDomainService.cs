namespace Narivia.BusinessLogic.DomainServices.Interfaces
{
    interface IBattleDomainService
    {
        BattleResult AttackRegion(string attackerFactionId, string defenderRegionId);
    }
}
