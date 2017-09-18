using System;
using System.IO;
using System.Xml.Serialization;

namespace Narivia.DataAccess.IO
{
    /// <summary>
    /// XML Manager.
    /// </summary>
    // TODO: Create an interface
    public class XmlManager<T>
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XmlManager"/> class.
        /// </summary>
        public XmlManager()
        {
            Type = typeof(T);
        }

        /// <summary>
        /// Reads a <see cref="T"/> from an XML file.
        /// </summary>
        /// <param name="path">Path.</param>
        public T Read(string path)
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
        /// Writes the specified <see cref="T"/> into an XML file.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="obj">Object to write.</param>
        // TODO: Shouldn't I use T instead of object for the obj parameter?
        public void Write(string path, object obj)
        {
            using (TextWriter writer = new StreamWriter(path))
            {
                XmlSerializer xml = new XmlSerializer(Type);
                xml.Serialize(writer, obj);
            }
        }
    }
}
