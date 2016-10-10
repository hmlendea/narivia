using Narivia.UI.Graphics;

namespace Narivia.UI.Menus
{
    public class MenuItem
    {
        public string LinkType { get; set; }

        public string LinkId { get; set; }

        public Image Image { get; set; }

        public MenuItem()
        {
            LinkType = string.Empty;
            LinkId = string.Empty;
            Image = new Image();
        }
    }
}
