using System;
using System.Collections.Generic;
using System.IO;

using Narivia.Models;
using Narivia.Settings;

namespace Narivia.GameLogic.GameManagers
{
    public sealed class BuildingManager : IBuildingManager
    {
        readonly Random random;
        readonly IWorldManager worldManager;

        string worldId;
        Dictionary<string, Building> buildings;

        public BuildingManager(
            string worldId,
            IWorldManager worldManager)
        {
            random = new Random();

            this.worldId = worldId;
            this.worldManager = worldManager;
        }

        public void LoadContent()
        {
            string buildingsPath = Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "buildings.xml");

            buildings = new Dictionary<string, Building>();
        }

        public void UnloadContent()
        {
            buildings.Clear();
        }

        public Building GetBuilding(string buildingId)
        {
            return buildings[buildingId];
        }

        public IEnumerable<Building> GetBuildings()
        => buildings.Values;

        public void BuildBuilding(string provinceId, string buildingId)
        {
            Province province = worldManager.GetProvince(provinceId);

            AddBuilding(provinceId, buildingId);
            worldManager.GetFaction(province.FactionId).Wealth -= 1000; // TODO: Actual cost
        }

        public void AddBuilding(string provinceId, string buildingId)
        {
            throw new NotImplementedException();
        }
    }
}
