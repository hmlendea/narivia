namespace Narivia.GameLogic.Events
{
    /// <summary>
    /// Faction life event handler.
    /// </summary>
    public delegate void FactionLifeEventHandler(object sender, FactionLifeEventArgs e);

    /// <summary>
    /// Faction life event arguments.
    /// </summary>
    public class FactionLifeEventArgs
    {
        /// <summary>
        /// The faction identifier.
        /// </summary>
        public string FactionId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionAttackEventArgs"/> class.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="isAlive">Faction alive status.</param>
        public FactionLifeEventArgs(string factionId)
        {
            FactionId = factionId;
        }
    }
}
