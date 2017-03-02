using System;
using System.Collections.Generic;

namespace Narivia.DataAccess
{
    public class XmlDatabase<T>
    {
        public string FileName { get; private set; }

        public XmlDatabase(string fileName)
        {
            FileName = fileName;
        }

        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public T Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }
    }
}
