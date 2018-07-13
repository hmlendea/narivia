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
        static Dictionary<byte, HoldingType> values = new Dictionary<byte, HoldingType>
        {
            { 0, new HoldingType(0, "N/A") },
            { 1, new HoldingType(1, "Castle") },
            { 2, new HoldingType(2, "City") },
            { 3, new HoldingType(3, "Temple") }
        };

        public byte Id { get; }

        public string Name { get; }

        private HoldingType(byte id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
        
        public static HoldingType Empty = values[0];
        public static HoldingType Castle = values[1];
        public static HoldingType City = values[2];
        public static HoldingType Temple = values[3];
        
        public Array GetValues()
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

        public static bool operator ==(HoldingType current, HoldingType other) => current.Equals(other);

        public static bool operator !=(HoldingType current, HoldingType other) => !current.Equals(other);

        public static implicit operator int(HoldingType current) => current.Id;
    }
}
