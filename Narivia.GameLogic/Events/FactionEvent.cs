namespace Narivia.GameLogic.Events
{
    /// <summary>
    /// Faction event handler.
    /// </summary>
    public delegate void FactionEventHandler(object sender, FactionEventArgs e);

    /// <summary>
    /// Faction event arguments.
    /// </summary>
    public class FactionEventArgs
    {
        /// <summary>
        /// The faction identifier.
        /// </summary>
        public string FactionId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleEventArgs"/> class.
        /// </summary>
        /// <param name="factionId">Faction identifier.</param>
        /// <param name="isAlive">Faction alive status.</param>
        public FactionEventArgs(string factionId)
        {
            FactionId = factionId;
        }
    }
}
