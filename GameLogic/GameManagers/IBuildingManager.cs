using System.Collections.Generic;
using Narivia.Models;

namespace Narivia.GameLogic.GameManagers
{
    public interface IBuildingManager
    {
        void LoadContent();

        void UnloadContent();

        Building GetBuilding(string buildingId);

        IEnumerable<Building> GetBuildings();

        IEnumerable<Building> GetHoldingBuildings(string holdingId);

        void BuildBuilding(string holdingId, string buildingTypeId);

        void AddBuilding(string holdingId, string buildingTypeId, string cultureId);
    }
}
