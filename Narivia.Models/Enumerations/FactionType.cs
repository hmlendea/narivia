using System;
using System.Collections.Generic;
using System.Linq;

namespace Narivia.Models.Enumerations
{
    public class FactionType : IEquatable<FactionType>
    {
        static Dictionary<int, FactionType> values = new Dictionary<int, FactionType>
        {
            { 0, new FactionType(0, nameof(Gaia), false, false) },
            { 1, new FactionType(1, nameof(Player), true, false) },
            { 2, new FactionType(2, nameof(Active), true, true) },
            { 3, new FactionType(3, nameof(Inactive), false, false) }
        };

        public int Id { get; }

        public string Name { get; }

        public bool IsActive { get; }

        public bool HasAi { get; }

        private FactionType(int id, string name, bool isActive, bool hasAi)
        {
            Id = id;
            Name = name;
            IsActive = isActive;
            HasAi = hasAi;
        }

        public static FactionType Gaia => values[0];
        public static FactionType Player => values[1];
        public static FactionType Active => values[2];
        public static FactionType Inactive => values[3];

        public static Array GetValues()
        {
            return values.Values.ToArray();
        }

        public bool Equals(FactionType other)
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

            return Equals((FactionType)obj);
        }

        public override int GetHashCode()
        {
            return 613 ^ Id;
        }

        public override string ToString()
        {
            return Name;
        }

        public static FactionType FromId(int id)
        {
            return values[id];
        }

        public static FactionType FromString(string name)
        {
            return values.First(x => x.Value.Name.Equals(name)).Value;
        }

        public static bool operator ==(FactionType current, FactionType other) => current.Equals(other);

        public static bool operator !=(FactionType current, FactionType other) => !current.Equals(other);

        public static implicit operator int(FactionType current) => current.Id;

        public static implicit operator FactionType(int id) => values[id];
    }
}
