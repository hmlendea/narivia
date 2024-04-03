using Narivia.Models;

namespace Narivia.GameLogic.GameManagers
{
    public interface IBuildingManager
    {
        void LoadContent();

        void UnloadContent();

        Building GetBuilding(string buildingId);

        void BuildBuilding(string provinceId, string buildingId);

        void AddBuilding(string provinceId, string buildingId);
    }
}
