using dEvine_and_conquer.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.Base
{
    public class VisualObject
    {
        public Point Location { get; set; }

        //Point GetLocation();

        public VisualObject(float x, float y)
        {
            Location = new Point(x, y);
        }

        public VisualObject(Point loc)
        {
            Location = loc;
        }
    }
}
