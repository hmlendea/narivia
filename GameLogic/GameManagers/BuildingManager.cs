using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Narivia.DataAccess.DataObjects;
using Narivia.GameLogic.Mapping;
using Narivia.Models;
using Narivia.Settings;
using NuciDAL.Repositories;

namespace Narivia.GameLogic.GameManagers
{
    public sealed class BuildingManager : IBuildingManager
    {
        readonly Random random;
        readonly IHoldingManager holdingManager;
        readonly IWorldManager worldManager;

        string worldId;
        Dictionary<string, Building> buildings;
        Dictionary<string, BuildingType> buildingTypes;

        public BuildingManager(
            string worldId,
            IHoldingManager holdingManager,
            IWorldManager worldManager)
        {
            random = new Random();

            this.worldId = worldId;
            this.holdingManager = holdingManager;
            this.worldManager = worldManager;
        }

        public void LoadContent()
        {
            string buildingsPath = Path.Combine(ApplicationPaths.WorldsDirectory, worldId, "building_types.xml");

            IRepository<BuildingTypeEntity> repository = new XmlRepository<BuildingTypeEntity>(buildingsPath);

            buildings = new Dictionary<string, Building>();
            buildingTypes = repository.GetAll().ToDictionary(x => x.Id, x => x.ToDomainModel());
        }

        public void UnloadContent()
        {
            buildings.Clear();
        }

        public Building GetBuilding(string buildingId)
        => buildings[buildingId];

        public IEnumerable<Building> GetBuildings()
        => buildings.Values;

        public IEnumerable<Building> GetHoldingBuildings(string holdingId)
        => buildings.Values.Where(building => building.HoldingId == holdingId);

        public void BuildBuilding(string holdingId, string buildingTypeId)
        {
            BuildingType buildingType = buildingTypes[buildingTypeId];
            Holding holding = holdingManager.GetHolding(holdingId);
            Province province = worldManager.GetProvince(holding.ProvinceId);
            Faction faction = worldManager.GetFaction(province.FactionId);

            AddBuilding(holding.Id, buildingType.Id, faction.CultureId);
            faction.Wealth -= buildingType.Price;
        }

        public void AddBuilding(string holdingId, string buildingTypeId, string cultureId)
        {
            // TODO: Don't pass cultureId as a parameter

            Building building = new()
            {
                Id = $"{holdingId}_{buildingTypeId}",
                Name = buildingTypes[buildingTypeId].Name,
                Description = buildingTypes[buildingTypeId].Description,
                TypeId = buildingTypeId,
                HoldingId = holdingId,
                CultureId = cultureId
            };

            buildings.Add(building.Id, building);
        }
    }
}
