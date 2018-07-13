using System;
using System.Collections.Generic;
using System.Linq;

namespace Narivia.Models.Enumerations
{
    /// <summary>
    /// Holding Type
    /// </summary>
    public class HoldingType : IEquatable<HoldingType>
    {
        static Dictionary<int, HoldingType> values = new Dictionary<int, HoldingType>
        {
            { 0, new HoldingType(0, "N/A") },
            { 1, new HoldingType(1, "Castle") },
            { 2, new HoldingType(2, "City") },
            { 3, new HoldingType(3, "Temple") }
        };

        public int Id { get; }

        public string Name { get; }

        private HoldingType(int id, string name)
        {
            Id = id;
            Name = name;
        }
        
        public static HoldingType Empty = values[0];
        public static HoldingType Castle = values[1];
        public static HoldingType City = values[2];
        public static HoldingType Temple = values[3];
        
        public static Array GetValues()
        {
            return values.Values.ToArray();
        }

        public bool Equals(HoldingType other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((HoldingType)obj);
        }

        public override int GetHashCode()
        {
            return 873 ^ Id;
        }

        public override string ToString()
        {
            return Name;
        }

        public static HoldingType FromId(int id)
        {
            return values[id];
        }

        public static HoldingType FromString(string name)
        {
            return values.First(x => x.Value.Name.Equals(name)).Value;
        }

        public static bool operator ==(HoldingType current, HoldingType other) => current.Equals(other);

        public static bool operator !=(HoldingType current, HoldingType other) => !current.Equals(other);

        public static implicit operator int(HoldingType current) => current.Id;

        public static implicit operator HoldingType(int id) => FromId(id);
    }
}
