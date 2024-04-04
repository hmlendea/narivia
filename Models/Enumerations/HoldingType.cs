using System;
using System.Collections.Generic;
using System.Linq;

namespace Narivia.Models.Enumerations
{
    public class HoldingType : IEquatable<HoldingType>
    {
        static readonly Dictionary<string, HoldingType> values = new()
        {
            { "empty", new HoldingType("string", 0, "N/A") },
            { "castle", new HoldingType("castle", 1, "Castle") },
            { "city", new HoldingType("city", 2, "City") },
            { "temple", new HoldingType("temple", 3, "Temple") }
        };

        public string Id { get; }

        public int NumericalId { get; }

        public string Name { get; }

        HoldingType(string id, int numericalId, string name)
        {
            Id = id;
            NumericalId = numericalId;
            Name = name;
        }

        public static HoldingType Empty = values["empty"];
        public static HoldingType Castle = values["castle"];
        public static HoldingType City = values["city"];
        public static HoldingType Temple = values["temple"];

        public static Array GetValues()
            => values.Values.ToArray();

        public bool Equals(HoldingType other)
        {
            if (other is null)
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
            if (obj is null)
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
            => 873 ^ Id.GetHashCode();

        public override string ToString()
            => Id;

        public static HoldingType FromId(string id)
            => values[id];

        public static HoldingType FromNumericalId(int id)
            => values.Values.First(x => x.NumericalId.Equals(id));

        public static HoldingType FromString(string name)
            => values.First(x => x.Value.Name.Equals(name)).Value;

        public static bool operator ==(HoldingType current, HoldingType other) => current.Equals(other);

        public static bool operator !=(HoldingType current, HoldingType other) => !current.Equals(other);

        public static implicit operator string(HoldingType current) => current.Id;

        public static implicit operator HoldingType(string id) => FromId(id);

        public static implicit operator int(HoldingType current) => current.NumericalId;

        public static implicit operator HoldingType(int id) => FromNumericalId(id);
    }
}
