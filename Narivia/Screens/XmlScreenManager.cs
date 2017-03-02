using System;
using System.IO;
using System.Xml.Serialization;

namespace Narivia.Screens
{
    /// <summary>
    /// XML screen manager.
    /// </summary>
    public class XmlScreenManager<T>
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Narivia.Screens.XmlScreenManager`1"/> class.
        /// </summary>
        public XmlScreenManager()
        {
            Type = typeof(T);
        }

        /// <summary>
        /// Load the specified screen.
        /// </summary>
        /// <param name="path">Path.</param>
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

        /// <summary>
        /// Save the specified screen.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="obj">Object.</param>
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

