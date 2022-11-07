using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSS.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public override bool Equals(object obj)
        {
            var comparteTo = obj as Entity;
            if (ReferenceEquals(this, comparteTo)) return true;
            if (ReferenceEquals(null, comparteTo.Id)) return false;
            return Id.Equals(comparteTo.Id);
        }
        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }
        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            return a.Equals(b);
        }
        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }
    }

}
