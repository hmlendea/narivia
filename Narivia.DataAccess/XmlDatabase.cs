using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Narivia.DataAccess
{
    public class XmlDatabase<T>
    {
        public string FileName { get; private set; }

        public XmlDatabase(string fileName)
        {
            FileName = fileName;
        }

        public IEnumerable<T> LoadEntities()
        {
            if (!File.Exists(FileName))
            {
                return null;
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
