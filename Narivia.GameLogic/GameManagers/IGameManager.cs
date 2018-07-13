using System.Collections.Generic;

using Narivia.GameLogic.Enumerations;
using Narivia.GameLogic.Events;
using Narivia.Models;
using Narivia.Models.Enumerations;

namespace Narivia.GameLogic.GameManagers
{
    /// <summary>
    /// Game manager interface.
    /// </summary>
    public interface IGameManager
    {
        /// <summary>
        /// Occurs when a player province is attacked.
        /// </summary>
        event BattleEventHandler PlayerProvinceAttacked;

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
        /// Gets the world identifier.
        /// </summary>
        /// <value>The world identifier.</value>
        string WorldId { get; }

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

        void LoadContent();

        void UnloadContent();

        /// <summary>
        /// Advances the game by one turn.
        /// </summary>
        void NextTurn();

        /// <summary>
        /// Checks wether the specified provinces share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified provinces share a border, <c>false</c> otherwise.</returns>
        /// <param name="sourceProvinceId">Source province identifier.</param>
        /// <param name="targetProvinceId">Target province identifier.</param>
        bool ProvinceBordersProvince(string sourceProvinceId, string targetProvinceId);

        /// <summary>
        /// Checks wether a province has empty holding slots.
        /// </summary>
        /// <returns><c>true</c>, if the province has empty holding slots, <c>false</c> otherwise.</returns>
        /// <param name="provinceId">Province identifier.</param>
        bool ProvinceHasEmptyHoldingSlots(string provinceId);

        /// <summary>
        /// Checks wether the specified factions share a border.
        /// </summary>
        /// <returns><c>true</c>, if the specified factions share a border, <c>false</c> otherwise.</returns>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        bool FactionBordersFaction(string sourceFactionId, string targetFactionId);

        /// <summary>
        /// Returns the faction identifier at the given location.
        /// </summary>
        /// <returns>The faction identifier.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        string FactionIdAtLocation(int x, int y);

        /// <summary>
        /// Transfers the specified province to the specified faction.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        /// <param name="factionId">Faction identifier.</param>
        void TransferProvince(string provinceId, string factionId);

        /// <summary>
        /// Gets the army.
        /// </summary>
        /// <returns>The army.</returns>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        Army GetArmy(string factionId, string unitId);

        /// <summary>
        /// Gets the armies.
        /// </summary>
        /// <returns>The armies.</returns>
        IEnumerable<Army> GetArmies();

        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <returns>The culture.</returns>
        /// <param name="cultureId">Culture identifier.</param>
        Culture GetCulture(string cultureId);

        /// <summary>
        /// Gets the cultures.
        /// </summary>
        /// <returns>The cultures.</returns>
        IEnumerable<Culture> GetCultures();

        /// <summary>
        /// Gets the faction.
        /// </summary>
        /// <returns>The faction.</returns>
        /// <param name="factionId">Faction identifier.</param>
        Faction GetFaction(string factionId);

        /// <summary>
        /// Gets the factions.
        /// </summary>
        /// <returns>The factions.</returns>
        IEnumerable<Faction> GetFactions();

        /// <summary>
        /// Gets the culture of a faction.
        /// </summary>
        /// <returns>The culture.</returns>
        /// <param name="factionId">Faction identifier.</param>
        Culture GetFactionCulture(string factionId);

        /// <summary>
        /// Gets the flag of a factions.
        /// </summary>
        /// <returns>The faction flag.</returns>
        /// <param name="factionId">Faction identifier.</param>
        Flag GetFactionFlag(string factionId);

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
        /// Gets the provinces of a faction.
        /// </summary>
        /// <returns>The provinces.</returns>
        /// <param name="factionId">Faction identifier.</param>
        IEnumerable<Province> GetFactionProvinces(string factionId);

        /// <summary>
        /// Gets the holdings of a faction.
        /// </summary>
        /// <returns>The holdings.</returns>
        /// <param name="factionId">Faction identifier.</param>
        IEnumerable<Holding> GetFactionHoldings(string factionId);

        /// <summary>
        /// Gets the faction troops amount.
        /// </summary>
        /// <returns>The faction troops amount.</returns>
        /// <param name="factionId">Faction identifier.</param>
        int GetFactionTroopsAmount(string factionId);

        /// <summary>
        /// Gets the faction capital.
        /// </summary>
        /// <returns>The faction capital province.</returns>
        /// <param name="factionId">Faction identifier.</param>
        Province GetFactionCapital(string factionId);

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
        /// Gets the relations of a faction.
        /// </summary>
        /// <returns>The relations of a faction.</returns>
        /// <param name="factionId">Faction identifier.</param>
        IEnumerable<Relation> GetFactionRelations(string factionId);

        /// <summary>
        /// Gets the flag.
        /// </summary>
        /// <returns>The flag.</returns>
        /// <param name="flagId">Flag identifier.</param>
        Flag GetFlag(string flagId);

        /// <summary>
        /// Gets the flags.
        /// </summary>
        /// <returns>The flags.</returns>
        IEnumerable<Flag> GetFlags();

        /// <summary>
        /// Gets the holding.
        /// </summary>
        /// <returns>The holding.</returns>
        /// <param name="holdingId">Holding identifier.</param>
        Holding GetHolding(string holdingId);

        /// <summary>
        /// Gets the holdings.
        /// </summary>
        /// <returns>The holdings.</returns>
        IEnumerable<Holding> GetHoldings();

        /// <summary>
        /// Gets the province.
        /// </summary>
        /// <returns>The province.</returns>
        /// <param name="provinceId">Province identifier.</param>
        Province GetProvince(string provinceId);

        /// <summary>
        /// Gets the province holdings.
        /// </summary>
        /// <returns>The province holdings.</returns>
        /// <param name="provinceId">Province identifier.</param>
        IEnumerable<Holding> GetProvinceHoldings(string provinceId);

        /// <summary>
        /// Gets the income of a province.
        /// </summary>
        /// <returns>The province income.</returns>
        /// <param name="provinceId">Province identifier.</param>
        int GetProvinceIncome(string provinceId);

        /// <summary>
        /// Gets the provinces.
        /// </summary>
        /// <returns>The provinces.</returns>
        IEnumerable<Province> GetProvinces();

        /// <summary>
        /// Gets the relation between two factions.
        /// </summary>
        /// <returns>The faction relation.</returns>
        /// <param name="sourceFactionId">Source faction identifier.</param>
        /// <param name="targetFactionId">Target faction identifier.</param>
        Relation GetRelation(string sourceFactionId, string targetFactionId);

        /// <summary>
        /// Gets the relations between factions.
        /// </summary>
        /// <returns>The relations.</returns>
        IEnumerable<Relation> GetRelations();

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <returns>The resource.</returns>
        /// <param name="resourceId">Resource identifier.</param>
        Resource GetResource(string resourceId);

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <returns>The resources.</returns>
        IEnumerable<Resource> GetResources();

        /// <summary>
        /// Gets the terrain.
        /// </summary>
        /// <returns>The terrain.</returns>
        /// <param name="terrainId">Terrain identifier.</param>
        Terrain GetTerrain(string terrainId);

        /// <summary>
        /// Gets the terrains.
        /// </summary>
        /// <returns>The terrains.</returns>
        IEnumerable<Terrain> GetTerrains();

        /// <summary>
        /// Gets the unit.
        /// </summary>
        /// <returns>The unit.</returns>
        /// <param name="unitId">Unit identifier.</param>
        Unit GetUnit(string unitId);

        /// <summary>
        /// Gets the units.
        /// </summary>
        /// <returns>The units.</returns>
        IEnumerable<Unit> GetUnits();

        /// <summary>
        /// Gets the world.
        /// </summary>
        /// <returns>The world.</returns>
        World GetWorld();

        /// <summary>
        /// Builds the specified holding type in a province.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        /// <param name="holdingType">Holding type.</param>
        void BuildHolding(string provinceId, HoldingType holdingType);

        /// <summary>
        /// Recruits the specified amount of troops of a unit for a faction.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="unitId">Unit identifier.</param>
        /// <param name="amount">Amount.</param>
        void RecruitUnits(string factionId, string unitId, int amount);

        /// <summary>
        /// The player faction will attack the specified province.
        /// </summary>
        /// <param name="provinceId">Province identifier.</param>
        BattleResult PlayerAttackProvince(string provinceId);
    }
}
