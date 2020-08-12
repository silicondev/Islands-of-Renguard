using IslandsOfRenguard.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace IslandsOfRenguard.Base
{
    public class Variation
    {
        public ID Variant { get; }

        public Variation(ID var)
        {
            Variant = var;
        }
    }
}
