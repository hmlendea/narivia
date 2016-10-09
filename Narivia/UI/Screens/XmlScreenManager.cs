using System;
using System.IO;
using System.Xml.Serialization;

namespace Narivia.UI.Screens
{
    public class XmlScreenManager<T>
    {
        public Type Type { get; set; }

        public XmlScreenManager()
        {
            Type = typeof(T);
        }

        public T Load(string path)
        {
            T instance;

            using (TextReader reader = new StreamReader(path))
            {
                XmlSerializer xml = new XmlSerializer(Type);
                instance = (T)xml.Deserialize(reader);
            }

            return instance;
        }

        public void Save(string path, object obj)
        {
            using (TextWriter writer = new StreamWriter(path))
            {
                XmlSerializer xml = new XmlSerializer(Type);
                xml.Serialize(writer, obj);
            }
        }
    }
}

