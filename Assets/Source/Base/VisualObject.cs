using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandsOfRenguard.Base
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
