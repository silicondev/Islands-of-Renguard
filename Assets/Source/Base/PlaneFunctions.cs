using dEvine_and_conquer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.Base
{
    public static class PlaneFunctions
    {
        public static float TriangleArea(Point a, Point b, Point c)
        {
            //return (float)(0.5 * ((a.X * (b.Y - c.Y)) + (b.X * (b.Y - a.Y)) + (c.X * (a.Y - b.Y))));
            var sideA = Heuristic(a, b);
            var sideB = Heuristic(b, c);
            var sideC = Heuristic(c, a);

            var s = (sideA + sideB + sideC) / 2;
            return (float)Math.Sqrt(s * (s - sideA) * (s - sideB) * (s - sideC));
        }
        public static float QuadrilateralArea(Point tl, Point tr, Point bl, Point br)
        {
            var taa = TriangleArea(bl, tl, tr);
            var tab = TriangleArea(bl, br, tr);
            return taa + tab;
        }

        public static float Heuristic(Point a, Point b)
        {
            double x = Math.Pow(Difference(a.X, b.X), 2);
            double y = Math.Pow(Difference(a.Y, b.Y), 2);
            double z = Math.Sqrt(x + y);
            return (float)z;
        }

        public static float Difference(float a, float b) => a > b ? a - b : b > a ? b - a : 0;
    }
}
