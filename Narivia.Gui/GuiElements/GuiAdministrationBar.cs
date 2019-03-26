using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NuciXNA.Gui.Controls;
using NuciXNA.Input;
using NuciXNA.Primitives;

namespace Narivia.Gui.Controls
{
    public class GuiAdministrationBar : GuiControl, IGuiControl
    {
        GuiImage bar;

        GuiButton buildButton;
        GuiButton recruitButton;
        GuiButton statsButton;

        public MouseButtonEventHandler BuildButtonClicked;

        public MouseButtonEventHandler RecruitButtonClicked;

        public MouseButtonEventHandler StatsButtonClicked;

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            bar = new GuiImage
            {
                ContentFile = "Interface/AdministrationBar/bar"
            };
            buildButton = new GuiButton
            {
                Id = $"{Id}_{nameof(buildButton)}",
                ContentFile = "Interface/AdministrationBar/button-build",
                Location = new Point2D(16, 10),
                Size = new Size2D(26, 26)
            };
            recruitButton = new GuiButton
            {
                Id = $"{Id}_{nameof(recruitButton)}",
                ContentFile = "Interface/AdministrationBar/button-recruit",
                Location = new Point2D(52, 10),
                Size = new Size2D(26, 26)
            };
            statsButton = new GuiButton
            {
                Id = $"{Id}_{nameof(statsButton)}",
                ContentFile = "Interface/AdministrationBar/button-stats",
                Location = new Point2D(89, 10),
                Size = new Size2D(26, 26)
            };

            RegisterChildren(bar, buildButton, recruitButton, statsButton);
            RegisterEvents();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void DoUnloadContent()
        {
            UnregisterEvents();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void DoUpdate(GameTime gameTime)
        {

        }

        /// <summary>
        /// Draw the content on the specified <see cref="SpriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }
        /// <summary>
        /// Registers the events.
        /// </summary>
        void RegisterEvents()
        {
            buildButton.Clicked += OnBuildButtonClicked;
            recruitButton.Clicked += OnRecruitButtonClicked;
            statsButton.Clicked += OnStatsButtonClicked;
        }

        /// <summary>
        /// Unregisters the events.
        /// </summary>
        void UnregisterEvents()
        {
            buildButton.Clicked -= OnBuildButtonClicked;
            recruitButton.Clicked -= OnRecruitButtonClicked;
            statsButton.Clicked -= OnStatsButtonClicked;
        }

        void OnBuildButtonClicked(object sender, MouseButtonEventArgs e)
        {
            BuildButtonClicked?.Invoke(this, e);
        }

        void OnRecruitButtonClicked(object sender, MouseButtonEventArgs e)
        {
            RecruitButtonClicked?.Invoke(this, e);
        }

        void OnStatsButtonClicked(object sender, MouseButtonEventArgs e)
        {
            StatsButtonClicked?.Invoke(this, e);
        }
    }
}
