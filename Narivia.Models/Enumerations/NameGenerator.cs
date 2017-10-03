namespace Narivia.Models.Enumerations
{
    /// <summary>
    /// Name generator enumeration.
    /// </summary>
    public enum NameGenerator : byte
    {
        /// <summary>
        /// Name generator based on Markov chains.
        /// </summary>
        MarkovNameGenerator,

        /// <summary>
        /// Name generator that randomly mixes words from different lists
        /// </summary>
        RandomMixerNameGenerator
    }
}
