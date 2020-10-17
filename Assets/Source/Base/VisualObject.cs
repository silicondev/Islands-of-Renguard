using dEvine_and_conquer.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.Base
{
    public class VisualObject
    {
        public Point Location { get; private set; }

        public VisualObject(float x, float y)
        {
            Location = (x, y);
        }

        public VisualObject(Point loc)
        {
            Location = loc.Copy();
        }
    }
}
