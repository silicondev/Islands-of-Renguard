using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.Base
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
