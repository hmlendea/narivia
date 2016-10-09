using System;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Narivia.UI.Screens
{
    public class Screen
    {
        protected ContentManager content;

        public string XmlPath { get; set; }

        [XmlIgnore]
        public Type Type { get; set; }

        public Screen()
        {
            Type = GetType();
            XmlPath = @"UI/Screens/" + Type.ToString().Replace("Narivia.UI.Screens.", "") + ".xml";
        }

        public virtual void LoadContent()
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}

