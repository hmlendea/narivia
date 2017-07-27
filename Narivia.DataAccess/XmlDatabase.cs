using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using Narivia.Logging;
using Narivia.Logging.Enumerations;

namespace Narivia.DataAccess
{
    /// <summary>
    /// XML database.
    /// </summary>
    public class XmlDatabase<T>
    {
        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDatabase"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public XmlDatabase(string fileName)
        {
            FileName = fileName;
        }

        /// <summary>
        /// Loads the entities.
        /// </summary>
        /// <returns>The entities.</returns>
        public IEnumerable<T> LoadEntities()
        {
            if (!File.Exists(FileName))
            {
                LogManager.Instance.Error(LogBuilder.BuildKvpMessage(
                    Operation.RepositoryLoading,
                    OperationStatus.Failure,
                    new Dictionary<LogInfoKey, string>
                    {
                        { LogInfoKey.FileName, FileName },
                        { LogInfoKey.Message, "The repository cannot be accessed" }
                    }));
            }

            XmlSerializer xs = new XmlSerializer(typeof(List<T>));
            List<T> entities;

            using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
            {
                entities = (List<T>)xs.Deserialize(sr);
            }

            return entities;
        }

        /// <summary>
        /// Saves the entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        public void SaveEntities(List<T> entities)
        {
            FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);

            using (StreamWriter sw = new StreamWriter(fs))
            {
                XmlSerializer xs = new XmlSerializer(typeof(List<T>));
                xs.Serialize(sw, entities);
            }
        }
    }
}
