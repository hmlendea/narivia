namespace Narivia.BusinessLogic.DomainServices.Interfaces
{
    interface IGameDomainService
    {
        void NewGame(string worldId, string factionId);

        void NextTurn();

        void TransferRegion(string regionId, string factionId);

        bool RegionHasBorder(string region1Id, string region2Id);
    }
}
