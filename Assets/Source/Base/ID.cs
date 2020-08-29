using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.Base
{
    public struct ID : IEquatable<ID>
    {
        private readonly int _id;
        public static implicit operator ID(int id)
        {
            return new ID(id);
        }

        public ID(int id)
        {
            _id = id;
        }

        public static bool operator ==(ID a, ID b)
        {
            return a._id == b._id;
        }

        public static bool operator !=(ID a, ID b)
        {
            return a._id != b._id;
        }

        public override bool Equals(object obj)
        {
            ID other;
            if (obj is ID id)
                other = id;
            else return false;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public bool Equals(ID other)
        {
            return Value == other._id;
        }

        public int Value => _id;
    }
}
