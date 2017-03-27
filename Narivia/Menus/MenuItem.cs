using Narivia.Graphics;

namespace Narivia.Menus
{
    public class MenuItem
    {
        /// <summary>
        /// Gets or sets the type of the link.
        /// </summary>
        /// <value>The type of the link.</value>
        public string LinkType { get; set; }

        /// <summary>
        /// Gets or sets the link identifier.
        /// </summary>
        /// <value>The link identifier.</value>
        public string LinkId { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public Image Image { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Narivia.Menus.MenuItem"/> class.
        /// </summary>
        public MenuItem()
        {
            LinkType = string.Empty;
            LinkId = string.Empty;
            Image = new Image();
        }
    }
}
