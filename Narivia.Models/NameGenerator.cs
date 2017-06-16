namespace Narivia.Models
{
    public enum NameGenerator
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
