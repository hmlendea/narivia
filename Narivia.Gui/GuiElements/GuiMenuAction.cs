using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// Menu action GUI element.
    /// </summary>
    public class GuiMenuAction : GuiMenuItem
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The type of the action.</value>
        public string ActionId { get; set; }
        
        /// <summary>
        /// Fired by the Activated event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        protected override void OnActivated(object sender, EventArgs e)
        {
            base.OnActivated(sender, e);

            switch (ActionId)
            {
                case "Exit":
                    // TODO: Close the game somehow...
                    //Program.Game.Exit();
                    break;
            }
        }
    }
}
