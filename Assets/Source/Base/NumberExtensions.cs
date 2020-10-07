using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.Base
{
    public static class NumberExtensions
    {
        public static int Floor(this float val)
        {
            if (val == (int)val) return (int)val;
            return (int)Math.Floor(val);
        }
    }
}
