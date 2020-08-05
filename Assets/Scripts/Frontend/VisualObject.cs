using IslandsOfRenguard.Scripts.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandsOfRenguard.Scripts.Frontend
{
    public class VisualObject
    {
        public Point Location { get; protected set; }

        public VisualObject(int x, int y)
        {
            Location = new Point(x, y);
        }
    }
}
