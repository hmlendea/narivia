namespace Narivia.GameLogic.GameManagers
{
    public interface IEconomyManager
    {
        void LoadContent();

        void UnloadContent();

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
        /// Gets the income of a province.
        /// </summary>
        /// <returns>The province income.</returns>
        /// <param name="provinceId">Province identifier.</param>
        int GetProvinceIncome(string provinceId);
    }
}
