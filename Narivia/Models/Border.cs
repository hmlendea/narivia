using System.ComponentModel.DataAnnotations;

namespace Narivia.Models
{
    public class Border : EntityBase
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public override string Id
        {
            get { return Region1Id + ":" + Region2Id; }
        }

        /// <summary>
        /// Gets the first region identifier.
        /// </summary>
        /// <value>The first region identifier.</value>
        [StringLength(40, MinimumLength = 3)]
        public string Region1Id { get; set; }

        /// <summary>
        /// Gets the second region identifier.
        /// </summary>
        /// <value>The second region identifier.</value>
        [StringLength(40, MinimumLength = 3)]
        public string Region2Id { get; set; }
    }
}