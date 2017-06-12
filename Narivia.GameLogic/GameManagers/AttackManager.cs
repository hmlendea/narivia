using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Narivia.Models;

namespace Narivia.GameLogic.GameManagers
{
    public class AttackManager
    {
        IEnumerable<Border> borders;
        IEnumerable<Holding> holdings;
        IEnumerable<Region> regions;
        IEnumerable<Resource> resources;

        public AttackManager(IEnumerable<Border> borders,
                             IEnumerable<Holding> holdings,
                             IEnumerable<Region> regions,
                             IEnumerable<Resource> resources)
        {
            this.borders = borders;
            this.holdings = holdings;
            this.regions = regions;
            this.resources = resources;
        }

        /// <summary>
        /// WIP blitzkrieg sequencial algorithm for invading a faction.
        /// </summary>
        /// <returns>The region identifier.</returns>
        /// <param name="factionId">Attacking faction identifier.</param>
        /// <param name="targetFactionId">Targeted faction identifier.</param>
        /// <summary>
        public string GetNextRegion_Seq(string factionId, string targetFactionId)
        {
            Random random = new Random();
            List<string> regionsOwnedIds = new List<string>();
            Dictionary<string, int> targets = new Dictionary<string, int>();

            foreach (Region region in regions)
            {
                if (region.FactionId == factionId)
                {
                    regionsOwnedIds.Add(region.Id);
                }
            }

            foreach (Region region in regions)
            {
                bool ok = true;

                if (region.FactionId != targetFactionId)
                {
                    ok = false;
                    continue;
                }

                foreach (string regionOwnedId in regionsOwnedIds)
                {
                    if (regionOwnedId == region.Id)
                    {
                        ok = false;
                    }
                }

                if (!ok)
                {
                    continue;
                }

                foreach (string regionOwnedId in regionsOwnedIds)
                {
                    if (RegionHasBorder(regionOwnedId, region.Id) && targets.ContainsKey(region.Id) == false)
                    {
                        targets.Add(region.Id, 0);
                    }
                }
            }

            foreach (Region region in regions.ToList())
            {
                bool ok = false;

                foreach (string targetRegionId in targets.Keys)
                {
                    if (targetRegionId == region.Id)
                    {
                        ok = true;
                    }
                }

                if (!ok)
                {
                    continue;
                }

                targets[region.Id] += GetSovreignty_Seq(factionId, region.Id);
                targets[region.Id] += GetCastlesImportance(region.Id);
                targets[region.Id] += GetCitiesImportance(region.Id);
                targets[region.Id] += GetTemplesImportance(region.Id);
                targets[region.Id] += GetResourceImportance(region.Id);
                targets[region.Id] += GetBordersImportance(factionId, region.Id);
            }

            if (targets.Count == 0)
            {
                return null;
            }

            int maxScore = 0;

            foreach (int score in targets.Values)
            {
                if (score > maxScore)
                {
                    maxScore = score;
                }
            }

            List<string> topTargets = new List<string>();

            foreach (string regionTargetId in targets.Keys)
            {
                if (targets[regionTargetId] == maxScore)
                {
                    topTargets.Add(regionTargetId);
                }
            }

            int topTargetsCount = 0;

            foreach (string regionTargetId in topTargets)
            {
                topTargetsCount += 1;
            }

            string regionId = topTargets[random.Next(0, topTargetsCount)];

            TransferRegion(regionId, factionId);

            return regionId;
        }

        /// <summary>
        /// WIP blitzkrieg parallelized algorithm for invading a faction.
        /// </summary>
        /// <returns>The region identifier.</returns>
        /// <param name="factionId">Attacking faction identifier.</param>
        /// <param name="targetFactionId">Targeted faction identifier.</param>
        /// <summary>
        public string GetNextRegion_Parallel(string factionId, string targetFactionId)
        {
            Random random = new Random();
            List<string> regionsOwnedIds = regions.Where(x => x.FactionId == factionId).Select(x => x.Id).ToList();

            int pointsForSovereignty = 30;
            int pointsForHoldingCastle = 30;
            int pointsForHoldingCity = 20;
            int pointsForHoldingTemple = 10;
            int pointsForBorder = 15;
            int pointsForResourceEconomic = 5;
            int pointsForResourceMilitary = 10;

            Dictionary<string, int> targets = regions.Where(x => x.FactionId == targetFactionId)
                                                     .Select(x => x.Id)
                                                     .Except(regionsOwnedIds)
                                                     .Where(x => regionsOwnedIds.Any(y => RegionHasBorder(x, y)))
                                                     .ToDictionary(x => x, y => 0);



            Parallel.ForEach(regions.Where(x => targets.ContainsKey(x.Id)).ToList(), (region) =>
            {
                if (region.SovereignFactionId == factionId)
                {
                    targets[region.Id] += pointsForSovereignty;
                }


                Parallel.ForEach(holdings.Where(x => x.RegionId == region.Id).ToList(), (holding) =>
                {
                    switch (holding.Type)
                    {
                        case HoldingType.Castle:
                            targets[region.Id] += pointsForHoldingCastle;
                            break;

                        case HoldingType.City:
                            targets[region.Id] += pointsForHoldingCity;
                            break;

                        case HoldingType.Temple:
                            targets[region.Id] += pointsForHoldingTemple;
                            break;
                    }
                });

                Resource regionResource = resources.FirstOrDefault(x => x.Id == region.ResourceId);

                if (regionResource != null)
                {
                    switch (regionResource.Type)
                    {
                        case ResourceType.Military:
                            targets[region.Id] += pointsForResourceMilitary;
                            break;

                        case ResourceType.Economy:
                            targets[region.Id] += pointsForResourceEconomic;
                            break;
                    }
                }

                targets[region.Id] += regionsOwnedIds.Count(x => RegionHasBorder(x, region.Id)) * pointsForBorder;
            });

            if (targets.Count == 0)
            {
                return null;
            }

            int maxScore = targets.Max(x => x.Value);
            List<string> topTargets = targets.Keys.Where(x => targets[x] == maxScore).ToList();
            string regionId = topTargets[random.Next(0, topTargets.Count())];

            TransferRegion(regionId, factionId);

            return regionId;
        }

        int GetSovreignty_Seq(string factionId, string regionId)
        {
            Region region = null;

            foreach (Region reg in regions.ToList())
            {
                if (reg.Id == regionId)
                {
                    region = reg;
                }
            }

            if (region.State == RegionState.Occupied &&
                region.FactionId == factionId)
            {
                return 30;
            }

            return 0;
        }

        int GetCastlesImportance(string regionId)
        {
            int score = 0;

            foreach (Holding holding in holdings.ToList())
            {
                if (holding.RegionId != regionId)
                {
                    continue;
                }

                if (holding.Type == HoldingType.Castle)
                {
                    score += 30;
                }
            }

            return score;
        }

        int GetCitiesImportance(string regionId)
        {
            int score = 0;

            foreach (Holding holding in holdings.ToList())
            {
                if (holding.RegionId != regionId)
                {
                    continue;
                }

                if (holding.Type == HoldingType.City)
                {
                    score += 20;
                }
            }

            return score;
        }

        int GetTemplesImportance(string regionId)
        {
            int score = 0;

            foreach (Holding holding in holdings.ToList())
            {
                if (holding.RegionId != regionId)
                {
                    continue;
                }

                if (holding.Type == HoldingType.Temple)
                {
                    score += 10;
                }
            }

            return score;
        }

        int GetResourceImportance(string regionId)
        {
            Resource regionResource = null;
            Region region = null;

            foreach (Region reg in regions.ToList())
            {
                if (reg.Id == regionId)
                {
                    region = reg;
                }

                foreach (Resource resource in resources.ToList())
                {
                    if (region == null)
                    {
                        continue;
                    }

                    if (resource.Id == region.ResourceId)
                    {
                        regionResource = resource;
                    }
                }
            }

            UtilResourceTest();
            if (regionResource != null)
            {
                switch (regionResource.Type)
                {
                    case ResourceType.Military:
                        return 10;

                    case ResourceType.Economy:
                        return 5;
                }
            }

            return 0;
        }

        int GetBordersImportance(string factionId, string regionId)
        {
            int score = 0;

            foreach (Region region in regions)
            {
                if (region.FactionId == factionId &&
                    region.Id == regionId)
                {
                    score += 10;
                }
            }

            return score;
        }

        /// <summary>
        /// Checks wether the specified regions share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified regions share a border, <c>false</c> otherwise.</returns>
        /// <param name="region1Id">First region identifier.</param>
        /// <param name="region2Id">Second region identifier.</param>
        bool RegionHasBorder(string region1Id, string region2Id)
        {
            return borders.Any(x => (x.Region1Id == region1Id && x.Region2Id == region2Id) ||
                                    (x.Region1Id == region2Id && x.Region2Id == region1Id));
        }

        /// <summary>
        /// Transfers the specified region to the specified faction.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        void TransferRegion(string regionId, string factionId)
        {
            Region region = regions.FirstOrDefault(x => x.Id == regionId);
            region.FactionId = factionId;
        }

        void UtilResourceTest()
        {
            // This is used for testing purposes
            Thread.Sleep(10);
        }
    }
}
