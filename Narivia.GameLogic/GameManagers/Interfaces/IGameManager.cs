using System.Collections.Generic;

using Narivia.GameLogic.Enumerations;
using Narivia.GameLogic.Events;
using Narivia.Models;
using Narivia.Models.Enumerations;

namespace Narivia.GameLogic.GameManagers.Interfaces
{
    public interface IGameManager
    {
        /// <summary>
        /// Occurs when a player region is attacked.
        /// </summary>
        event BattleEventHandler PlayerRegionAttacked;

        /// <summary>
        /// Occurs when a faction died.
        /// </summary>
        event FactionEventHandler FactionDestroyed;

        /// <summary>
        /// Occurs when a faction was revived.
        /// </summary>
        event FactionEventHandler FactionRevived;

        /// <summary>
        /// Occurs a faction won the game.
        /// </summary>
        event FactionEventHandler FactionWon;

        /// <summary>
        /// Gets or sets the world tiles.
        /// </summary>
        /// <value>The world tiles.</value>
        string[,] WorldTiles { get; set; }

        /// <summary>
        /// Gets the width of the world.
        /// </summary>
        /// <value>The width of the world.</value>
        int WorldWidth { get; }

        /// <summary>
        /// Gets the height of the world.
        /// </summary>
        /// <value>The height of the world.</value>
        int WorldHeight { get; }

        /// <summary>
        /// Gets the name of the world.
        /// </summary>
        /// <value>The name of the world.</value>
        string WorldName { get; }

        /// <summary>
        /// Gets the world identifier.
        /// </summary>
        /// <value>The world identifier.</value>
        string WorldId { get; }

        /// <summary>
        /// Gets the base region income.
        /// </summary>
        /// <value>The base region income.</value>
        int BaseRegionIncome { get; }

        /// <summary>
        /// Gets the base region recruitment.
        /// </summary>
        /// <value>The base region recruitment.</value>
        int BaseRegionRecruitment { get; }

        /// <summary>
        /// Gets the base faction recruitment.
        /// </summary>
        /// <value>The base faction recruitment.</value>
        int BaseFactionRecruitment { get; }

        /// <summary>
        /// Gets the minimum number of troops required to attack.
        /// </summary>
        /// <value>The minimum troops to attack.</value>
        int MinTroopsPerAttack { get; }

        /// <summary>
        /// Gets the starting wealth.
        /// </summary>
        /// <value>The starting wealth.</value>
        int StartingWealth { get; }

        /// <summary>
        /// Gets the starting troops per unit.
        /// </summary>
        /// <value>The starting troops per unit.</value>
        int StartingTroopsPerUnit { get; }

        /// <summary>
        /// Gets the price of holdings.
        /// </summary>
        /// <value>The holdings price.</value>
        int HoldingsPrice { get; }

        /// <summary>
        /// Gets the player faction identifier.
        /// </summary>
        /// <value>The player faction identifier.</value>
        string PlayerFactionId { get; }

        /// <summary>
        /// Gets the turn.
        /// </summary>
        /// <value>The turn.</value>
        int Turn { get; }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        /// <param name="worldId">World identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        void NewGame(string worldId, string factionId);

        /// <summary>
        /// Advances the game by one turn.
        /// </summary>
        void NextTurn();

        /// <summary>
        /// Checks wether the specified regions share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified regions share a border, <c>false</c> otherwise.</returns>
        /// <param name="sourceRegionId">Source region identifier.</param>
        /// <param name="targetRegionId">Target region identifier.</param>
        bool RegionBordersRegion(string sourceRegionId, string targetRegionId);

        /// <summary>
        /// Checks wether a region has empty holding slots.
        /// </summary>
        /// <returns><c>true</c>, if the region has empty holding slots, <c>false</c> otherwise.</returns>
        /// <param name="regionId">Region identifier.</param>
        bool RegionHasEmptyHoldingSlots(string regionId);

        /// <summary>
        /// Checks wether the specified factions share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified factions share a border, <c>false</c> otherwise.</returns>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        bool FactionBordersFaction(string sourceFactionId, string targetFactionId);

        /// <summary>
        /// Returns the faction identifier at the given position.
        /// </summary>
        /// <returns>The faction identifier.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        string FactionIdAtPosition(int x, int y);

        /// <summary>
        /// Transfers the specified region to the specified faction.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        void TransferRegion(string regionId, string factionId);

        /// <summary>
        /// Gets the culture of a faction.
        /// </summary>
        /// <returns>The culture.</returns>
        /// <param name="factionId">Faction identifier.</param>
        Culture GetFactionCulture(string factionId);

        /// <summary>
        /// Gets the name of the faction.
        /// </summary>
        /// <returns>The faction name.</returns>
        /// <param name="factionId">Faction identifier.</param>
        string GetFactionName(string factionId);

        /// <summary>
        /// Returns the colour of a faction.
        /// </summary>
        /// <returns>The colour.</returns>
        /// <param name="factionId">Faction identifier.</param>
        Colour GetFactionColour(string factionId);

        /// <summary>
        /// Gets the faction income.
        /// </summary>
        /// <returns>The faction income.</returns>
        /// <param name="factionId">Faction identifier.</param>
        int GetFactionIncome(string factionId);

        /// <summary>
        /// Gets the faction outcome.
        /// </summary>
        /// <returns>The faction outcome.</returns>
        /// <param name="factionId">Faction identifier.</param>
        int GetFactionOutcome(string factionId);

        /// <summary>
        /// Gets the faction recruitment.
        /// </summary>
        /// <returns>The faction recruitment.</returns>
        /// <param name="factionId">Faction identifier.</param>
        int GetFactionRecruitment(string factionId);

        /// <summary>
        /// Gets the regions count of a faction.
        /// </summary>
        /// <returns>The number of regions.</returns>
        /// <param name="factionId">Faction identifier.</param>
        int GetFactionRegionsCount(string factionId);

        /// <summary>
        /// Gets the holdings count of a faction.
        /// </summary>
        /// <returns>The number of holdings.</returns>
        /// <param name="factionId">Faction identifier.</param>
        int GetFactionHoldingsCount(string factionId);

        /// <summary>
        /// Gets the faction wealth.
        /// </summary>
        /// <returns>The faction wealth.</returns>
        /// <param name="factionId">Faction identifier.</param>
        int GetFactionWealth(string factionId);

        /// <summary>
        /// Gets the faction troops count.
        /// </summary>
        /// <returns>The faction troops count.</returns>
        /// <param name="factionId">Faction identifier.</param>
        int GetFactionTroopsCount(string factionId);

        /// <summary>
        /// Gets the faction capital.
        /// </summary>
        /// <returns>The faction capital.</returns>
        /// <param name="factionId">Faction identifier.</param>
        string GetFactionCapital(string factionId);

        /// <summary>
        /// Gets or sets the X map coordinate of the centre of the faction territoriy.
        /// </summary>
        /// <value>The X coordinate.</value>
        /// <param name="factionId">Faction identifier.</param>
        int GetFactionCentreX(string factionId);

        /// <summary>
        /// Gets or sets the Y map coordinate of the centre of the faction territoriy.
        /// </summary>
        /// <value>The Y coordinate.</value>
        /// <param name="factionId">Faction identifier.</param>
        int GetFactionCentreY(string factionId);

        /// <summary>
        /// Gets the faction army size.
        /// </summary>
        /// <returns>The faction army size.</returns>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        int GetFactionArmySize(string factionId, string unitId);

        /// <summary>
        /// Gets the faction idenfifier of a region.
        /// </summary>
        /// <returns>The faction identifier.</returns>
        /// <param name="regionId">Region identifier.</param>
        string GetRegionFaction(string regionId);

        /// <summary>
        /// Gets the name of a region.
        /// </summary>
        /// <returns>The name.</returns>
        /// <param name="regionId">Region identifier.</param>
        string GetRegionName(string regionId);

        /// <summary>
        /// Gets the resource of a region.
        /// </summary>
        /// <returns>The resource identifier.</returns>
        /// <param name="regionId">Region identifier.</param>
        string GetRegionResource(string regionId);

        /// <summary>
        /// Gets the name of the resource.
        /// </summary>
        /// <returns>The resource name.</returns>
        /// <param name="resourceId">Resource identifier.</param>
        string GetResourceName(string resourceId);

        /// <summary>
        /// Gets the factions.
        /// </summary>
        /// <returns>The factions.</returns>
        IEnumerable<Faction> GetFactions();

        /// <summary>
        /// Gets the regions of a faction.
        /// </summary>
        /// <returns>The regions.</returns>
        /// <param name="factionId">Faction identifier.</param>
        IEnumerable<Region> GetFactionRegions(string factionId);

        /// <summary>
        /// Gets the relations of a faction.
        /// </summary>
        /// <returns>The relations of a faction.</returns>
        /// <param name="factionId">Faction identifier.</param>
        IEnumerable<Relation> GetFactionRelations(string factionId);

        /// <summary>
        /// Gets the holdings of a faction.
        /// </summary>
        /// <returns>The holdings.</returns>
        /// <param name="factionId">Faction identifier.</param>
        IEnumerable<Holding> GetFactionHoldings(string factionId);

        /// <summary>
        /// Gets the region holdings.
        /// </summary>
        /// <returns>The region holdings.</returns>
        /// <param name="regionId">Region identifier.</param>
        IEnumerable<Holding> GetRegionHoldings(string regionId);

        /// <summary>
        /// Gets the units.
        /// </summary>
        /// <returns>The units.</returns>
        IEnumerable<Unit> GetUnits();

        /// <summary>
        /// Builds the specified holding type in a region.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="holdingType">Holding type.</param>
        void BuildHolding(string regionId, HoldingType holdingType);

        /// <summary>
        /// Recruits the specified amount of troops of a unit for a faction.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        /// <param name="amount">Amount.</param>
        void RecruitUnits(string factionId, string unitId, int amount);

        /// <summary>
        /// The player faction will attack the specified region.
        /// </summary>
        /// <param name="regionId">Region identifier.</param>
        BattleResult PlayerAttackRegion(string regionId);
    }
}
