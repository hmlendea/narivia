using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Narivia.GameLogic.GameManagers.Interfaces;
using Narivia.Graphics;
using Narivia.Graphics.Geometry;
using Narivia.Graphics.Geometry.Mapping;
using Narivia.Graphics.Mapping;
using Narivia.Gui.WorldMap;
using Narivia.Input.Events;
using Narivia.Settings;

namespace Narivia.Gui.GuiElements
{
    /// <summary>
    /// World map GUI element.
    /// </summary>
    public class GuiWorldmap : GuiElement
    {
        /// <summary>
        /// Gets the selected region identifier.
        /// </summary>
        /// <value>The selected region identifier.</value>
        [XmlIgnore]
        public string SelectedRegionId { get; private set; }

        IGameManager game;
        Camera camera;
        Map map;

        Sprite regionHighlight;
        Sprite selectedRegionHighlight;
        Sprite factionBorder;

        Point2D mouseCoords;

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            camera = new Camera { Size = Size };
            map = new Map();

            regionHighlight = new Sprite
            {
                ContentFile = "World/Effects/border",
                SourceRectangle = new Rectangle2D(GameDefines.MAP_TILE_SIZE, GameDefines.MAP_TILE_SIZE * 3,
                                                  GameDefines.MAP_TILE_SIZE, GameDefines.MAP_TILE_SIZE).ToXnaRectangle(),
                Tint = Colour.White.ToXnaColor(),
                Opacity = 1.0f
            };

            selectedRegionHighlight = new Sprite
            {
                ContentFile = "World/Effects/border",
                SourceRectangle = new Rectangle2D(0, GameDefines.MAP_TILE_SIZE * 3,
                                                  GameDefines.MAP_TILE_SIZE, GameDefines.MAP_TILE_SIZE).ToXnaRectangle(),
                Tint = Colour.White.ToXnaColor(),
                Opacity = 1.0f
            };

            factionBorder = new Sprite
            {
                ContentFile = "World/Effects/border",
                SourceRectangle = new Rectangle2D(0, GameDefines.MAP_TILE_SIZE,
                                                  GameDefines.MAP_TILE_SIZE, GameDefines.MAP_TILE_SIZE).ToXnaRectangle(),
                Tint = Colour.Blue.ToXnaColor(),
                Opacity = 1.0f
            };

            camera.LoadContent();
            map.LoadContent(game.GetWorld());

            regionHighlight.LoadContent();
            selectedRegionHighlight.LoadContent();
            factionBorder.LoadContent();

            base.LoadContent();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
            camera.UnloadContent();
            map.UnloadContent();

            regionHighlight.UnloadContent();
            selectedRegionHighlight.UnloadContent();
            factionBorder.UnloadContent();

            base.UnloadContent();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            camera.Size = Size;

            camera.Update(gameTime);
            map.Update(gameTime);

            regionHighlight.Update(gameTime);
            selectedRegionHighlight.Update(gameTime);
            factionBorder.Update(gameTime);

            base.Update(gameTime);

            Point2D mouseGameMapCoords = ScreenToMapCoordinates(mouseCoords);

            int x = mouseGameMapCoords.X;
            int y = mouseGameMapCoords.Y;

            if (x > 0 && x < game.GetWorld().Width &&
                y > 0 && y < game.GetWorld().Height)
            {
                // TODO: Handle the Id retrieval properly
                SelectedRegionId = game.GetWorld().Tiles[x, y].RegionId;

                // TODO: Also handle this properly
                if (game.FactionIdAtLocation(x, y) == GameDefines.GAIA_FACTION)
                {
                    SelectedRegionId = null;
                }
            }
            else
            {
                SelectedRegionId = null;
            }
        }

        /// <summary>
        /// Draw the content on the specified spriteBatch.
        /// </summary>
        /// <returns>The draw.</returns>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch, camera);

            DrawRegionHighlight(spriteBatch);
            DrawFactionBorders(spriteBatch);

            base.Draw(spriteBatch);
        }

        // TODO: Handle this better
        /// <summary>
        /// Associates the game manager.
        /// </summary>
        /// <param name="game">Game.</param>
        public void AssociateGameManager(ref IGameManager game)
        {
            this.game = game;
        }

        /// <summary>
        /// Centres the camera on the specified location.
        /// </summary>
        public void CentreCameraOnLocation(int x, int y)
        {
            camera.CentreOnLocation(new Point2D(x * GameDefines.MAP_TILE_SIZE, y * GameDefines.MAP_TILE_SIZE));
        }

        void DrawRegionHighlight(SpriteBatch spriteBatch)
        {
            if (string.IsNullOrEmpty(SelectedRegionId))
            {
                return;
            }

            int cameraSizeX = camera.Size.Width / GameDefines.MAP_TILE_SIZE + 2;
            int cameraSizeY = camera.Size.Height / GameDefines.MAP_TILE_SIZE + 2;

            for (int j = 0; j < cameraSizeY; j++)
            {
                for (int i = 0; i < cameraSizeX; i++)
                {
                    Point2D screenCoords = new Point2D((i * GameDefines.MAP_TILE_SIZE) - camera.Location.X % GameDefines.MAP_TILE_SIZE,
                                                     (j * GameDefines.MAP_TILE_SIZE) - camera.Location.Y % GameDefines.MAP_TILE_SIZE);
                    Point2D gameCoords = ScreenToMapCoordinates(screenCoords);

                    int x = gameCoords.X;
                    int y = gameCoords.Y;

                    if (x < 0 || x > game.GetWorld().Width ||
                        y < 0 || y > game.GetWorld().Height)
                    {
                        continue;
                    }

                    string regionId = game.GetWorld().Tiles[x, y].RegionId;
                    string factionId = game.FactionIdAtLocation(x, y);
                    Colour factionColour = game.GetFaction(factionId).Colour.ToColour();

                    if (factionId == GameDefines.GAIA_FACTION)
                    {
                        continue;
                    }

                    if (SelectedRegionId == regionId)
                    {
                        selectedRegionHighlight.Location = screenCoords.ToXnaPoint();
                        selectedRegionHighlight.Tint = factionColour.ToXnaColor();
                        selectedRegionHighlight.Draw(spriteBatch);
                    }
                    else
                    {
                        regionHighlight.Location = screenCoords.ToXnaPoint();
                        regionHighlight.Tint = factionColour.ToXnaColor();
                        regionHighlight.Draw(spriteBatch);
                    }
                }
            }
        }

        void DrawFactionBorders(SpriteBatch spriteBatch)
        {
            int cameraSizeX = camera.Size.Width / GameDefines.MAP_TILE_SIZE + 2;
            int cameraSizeY = camera.Size.Height / GameDefines.MAP_TILE_SIZE + 2;

            for (int j = 0; j < cameraSizeY; j++)
            {
                for (int i = 0; i < cameraSizeX; i++)
                {
                    Point2D screenCoords = new Point2D((i * GameDefines.MAP_TILE_SIZE) - camera.Location.X % GameDefines.MAP_TILE_SIZE,
                                                       (j * GameDefines.MAP_TILE_SIZE) - camera.Location.Y % GameDefines.MAP_TILE_SIZE);
                    Point2D gameCoords = ScreenToMapCoordinates(screenCoords);

                    int x = gameCoords.X;
                    int y = gameCoords.Y;

                    if (x < 0 || x > game.GetWorld().Width ||
                        y < 0 || y > game.GetWorld().Height)
                    {
                        continue;
                    }

                    string factionId = game.FactionIdAtLocation(x, y);
                    Colour factionColour = game.GetFaction(factionId).Colour.ToColour();

                    if (factionId == GameDefines.GAIA_FACTION)
                    {
                        continue;
                    }

                    string factionIdN = game.FactionIdAtLocation(x, y - 1);
                    string factionIdW = game.FactionIdAtLocation(x - 1, y);
                    string factionIdS = game.FactionIdAtLocation(x, y + 1);
                    string factionIdE = game.FactionIdAtLocation(x + 1, y);

                    factionBorder.Location = screenCoords.ToXnaPoint();
                    factionBorder.Tint = factionColour.ToXnaColor();

                    if (factionIdN != factionId &&
                        factionIdN != GameDefines.GAIA_FACTION)
                    {
                        factionBorder.SourceRectangle = new Rectangle2D(GameDefines.MAP_TILE_SIZE, 0,
                                                                        GameDefines.MAP_TILE_SIZE, GameDefines.MAP_TILE_SIZE).ToXnaRectangle();
                        factionBorder.Draw(spriteBatch);
                    }
                    if (factionIdW != factionId &&
                        factionIdW != GameDefines.GAIA_FACTION)
                    {
                        factionBorder.SourceRectangle = new Rectangle2D(0, GameDefines.MAP_TILE_SIZE,
                                                                        GameDefines.MAP_TILE_SIZE, GameDefines.MAP_TILE_SIZE).ToXnaRectangle();
                        factionBorder.Draw(spriteBatch);
                    }
                    if (factionIdS != factionId &&
                        factionIdS != GameDefines.GAIA_FACTION)
                    {
                        factionBorder.SourceRectangle = new Rectangle2D(GameDefines.MAP_TILE_SIZE, GameDefines.MAP_TILE_SIZE * 2,
                                                                        GameDefines.MAP_TILE_SIZE, GameDefines.MAP_TILE_SIZE).ToXnaRectangle();
                        factionBorder.Draw(spriteBatch);
                    }
                    if (factionIdE != factionId &&
                        factionIdE != GameDefines.GAIA_FACTION)
                    {
                        factionBorder.SourceRectangle = new Rectangle2D(GameDefines.MAP_TILE_SIZE * 2, GameDefines.MAP_TILE_SIZE,
                                                                        GameDefines.MAP_TILE_SIZE, GameDefines.MAP_TILE_SIZE).ToXnaRectangle();
                        factionBorder.Draw(spriteBatch);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the map cpprdomates based on the specified screen coordinates.
        /// </summary>
        /// <returns>The map coordinates.</returns>
        /// <param name="screenCoords">Screen coordinates.</param>
        Point2D ScreenToMapCoordinates(Point2D screenCoords)
        {
            return new Point2D((camera.Location.X + screenCoords.X) / GameDefines.MAP_TILE_SIZE,
                               (camera.Location.Y + screenCoords.Y) / GameDefines.MAP_TILE_SIZE);
        }

        protected override void OnMouseMoved(object sender, MouseEventArgs e)
        {
            base.OnMouseMoved(sender, e);

            mouseCoords = e.Location.ToPoint2D();
        }
    }
}
