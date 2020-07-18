using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandsOfRenguard.Scripts.Universal
{
    public struct ID
    {
        private int _id;
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
    }
}
