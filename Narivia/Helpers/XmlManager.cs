using System;
using System.IO;
using System.Xml.Serialization;

namespace Narivia.Helpers
{
    /// <summary>
    /// XML Manager.
    /// </summary>
    public class XmlManager<T>
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlManager"/> class.
        /// </summary>
        public XmlManager()
        {
            Type = typeof(T);
        }

        /// <summary>
        /// Load the specified path.
        /// </summary>
        /// <returns>The load.</returns>
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
        /// Save the specified path and obj.
        /// </summary>
        /// <returns>The save.</returns>
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

