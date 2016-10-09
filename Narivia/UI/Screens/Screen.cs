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
        string xmlPath;
        Type type;

        public string XmlPath
        {
            get { return xmlPath; }
            set { xmlPath = value; }
        }

        [XmlIgnore]
        public Type Type
        {
            get { return type; }
            set { type = value; }
        }

        public Screen()
        {
            type = this.GetType();
            xmlPath = @"Screens/" + type.ToString().Replace("Craftico.Screens.", "") + ".xml";
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

