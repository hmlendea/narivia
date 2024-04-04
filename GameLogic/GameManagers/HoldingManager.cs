using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NuciExtensions;
using NuciXNA.DataAccess.Repositories;

using Narivia.DataAccess.DataObjects;
using Narivia.DataAccess.Repositories;
using Narivia.GameLogic.Generators;
using Narivia.GameLogic.Generators.Interfaces;
using Narivia.Models;
using Narivia.Models.Enumerations;
using Narivia.Settings;

namespace Narivia.GameLogic.GameManagers
{
    public sealed class HoldingManager : IHoldingManager
    {
        readonly Random random;
        readonly IWorldManager worldManager;

        string worldId;
        Dictionary<string, Holding> holdings;

        public HoldingManager(
            string worldId,
            IWorldManager worldManager)
        {
            random = new Random();

            this.worldId = worldId;
            this.worldManager = worldManager;
        }

        public void LoadContent()
        {
            holdings = new Dictionary<string, Holding>();
        }

        public void UnloadContent()
        {
            holdings.Clear();
        }

        public bool DoesProvinceHaveEmptyHoldings(string provinceId)
            => holdings.Values.Count(h => h.ProvinceId == provinceId && h.Type == HoldingType.Empty) > 0;

        public IEnumerable<Holding> GetFactionHoldings(string factionId)
            => holdings.Values.Where(h => h.Type != HoldingType.Empty &&
                                      worldManager.GetProvince(h.ProvinceId).FactionId == factionId);

        public Holding GetHolding(string holdingId)
            => holdings[holdingId];

        /// <summary>
        /// Gets the holdings.
        /// </summary>
        /// <returns>The holdings.</returns>
        public IEnumerable<Holding> GetHoldings()
            => holdings.Values;

        /// <summary>
        /// Gets the holdings of a province.
        /// </summary>
        /// <returns>The holdings.</returns>
        /// <param name="provinceId">Province identifier.</param>
        public IEnumerable<Holding> GetProvinceHoldings(string provinceId)
            => holdings.Values.Where(h => h.Type != HoldingType.Empty &&
                                      h.ProvinceId == provinceId);

        /// <summary>
        /// Builds the specified holding type in a province.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        /// <param name="holdingType">Holding type.</param>
        public void BuildHolding(string provinceId, HoldingType holdingType)
        {
            Province province = worldManager.GetProvince(provinceId);

            if (DoesProvinceHaveEmptyHoldings(provinceId))
            {
                AddHolding(provinceId, holdingType);
                worldManager.GetFaction(province.FactionId).Wealth -= worldManager.GetWorld().HoldingsPrice;
            }
        }

        /// <summary>
        /// Adds the specified holding type in a province.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        /// <param name="holdingType">Holding type.</param>
        public void AddHolding(string provinceId, HoldingType holdingType)
        {
            Holding emptySlot = holdings.Values.FirstOrDefault(h => h.ProvinceId == provinceId &&
                                                                    h.Type == HoldingType.Empty);

            if (emptySlot != null)
            {
                emptySlot.Type = holdingType;
            }
        }

        public void InitialiseFactionHoldings(string factionId)
        {
            List<Faction> factions = worldManager.GetFactions().ToList();
            List<Province> provinces = worldManager.GetProvinces().ToList();

            Faction faction = worldManager.GetFaction(factionId);
            Province capitalProvince = worldManager.GetFactionCapital(faction.Id);

            int holdingSlotsLeft = worldManager.HoldingSlotsPerFaction;

            INameGenerator nameGenerator = CreateNameGenerator(faction.CultureId);
            nameGenerator.ExcludedStrings.AddRange(factions.Select(f => f.Name));
            nameGenerator.ExcludedStrings.AddRange(holdings.Values.Select(h => h.Name));
            nameGenerator.ExcludedStrings.AddRange(provinces.Select(r => r.Name));

            List<Province> ownedProvinces = worldManager.GetFactionProvinces(faction.Id).ToList();

            foreach (Province province in ownedProvinces)
            {
                Holding holding = GenerateHolding(nameGenerator, province.Id);

                if (province.Id == capitalProvince.Id)
                {
                    holding.Name = province.Name;
                    holding.Description = $"The government seat castle of {faction.Name}";
                    holding.Type = HoldingType.Castle;
                }

                holdings.AddOrUpdate(holding.Id, holding);
                holdingSlotsLeft -= 1;
            }

            while (holdingSlotsLeft > 0)
            {
                Province province = ownedProvinces.GetRandomElement();
                Holding holding = GenerateHolding(nameGenerator, province.Id);

                holding.Description = string.Empty;
                holding.Type = HoldingType.Empty;

                holdings.AddOrUpdate(holding.Id, holding);
                holdingSlotsLeft -= 1;
            }
        }

        /// <summary>
        /// Creates a name generator.
        /// </summary>
        /// <returns>The name generator.</returns>
        /// <param name="cultureId">Culture identifier.</param>
        INameGenerator CreateNameGenerator(string cultureId)
        {
            INameGenerator nameGenerator;
            Culture culture = worldManager.GetCulture(cultureId);

            List<List<string>> wordLists = culture.PlaceNameSchema.Split(' ').ToList()
                                                  .Select(x => File.ReadAllLines(Path.Combine(ApplicationPaths.WordListsDirectory,
                                                                                              $"{x}.txt")).ToList())
                                                  .ToList();

            if (culture.PlaceNameGenerator == Models.Enumerations.NameGenerator.RandomMixerNameGenerator && wordLists.Count == 2)
            {
                nameGenerator = new RandomMixerNameGenerator(wordLists[0], wordLists[1]);
            }
            else if (culture.PlaceNameGenerator == Models.Enumerations.NameGenerator.RandomMixerNameGenerator && wordLists.Count == 3)
            {
                nameGenerator = new RandomMixerNameGenerator(wordLists[0], wordLists[1], wordLists[2]);
            }
            else // Default: Markov
            {
                nameGenerator = new MarkovNameGenerator(wordLists[0]);
            }

            return nameGenerator;
        }

        Holding GenerateHolding(INameGenerator generator, string provinceId)
        {
            Province province = worldManager.GetProvince(provinceId);
            Array holdingTypes = HoldingType.GetValues();

            HoldingType holdingType = (HoldingType)holdingTypes.GetValue(random.Next(1, holdingTypes.Length));
            string name = generator.GenerateName();

            Holding holding = new Holding
            {
                Id = $"h_{name.Replace(" ", "_").ToLower()}",
                ProvinceId = province.Id,
                Name = name,
                Description = $"The {name} {holdingType.ToString().ToLower()}", // TODO: Better description
                Type = holdingType
            };

            // TODO: Make sure this never happens and then remove this workaround
            while (holdings.Values.Any(h => h.Id == holding.Id))
            {
                return GenerateHolding(generator, province.Id);
            }

            return holding;
        }
    }
}
