using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dEvine_and_conquer.Base
{
    public class Point : IEquatable<Point>
    {
        public float X = 0;
        public float Y = 0;

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            Point other;
            if (obj is Point point)
                other = point;
            else return false;
            return Equals(other);
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Point a, Point b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            int hash = 47;
            hash = hash * 59 + X.GetHashCode();
            hash = hash * 59 + Y.GetHashCode();
            return hash;
        }

        public void Move(float x, float y)
        {
            X += x;
            Y += y;
        }

        public Point Flatten() => new Point(X.Floor(), Y.Floor());

        public bool IsContainedInCross(Point tl, Point tr, Point bl, Point br)
        {
            var topArea = PlaneFunctions.TriangleArea(this, tl, tr);
            var leftArea = PlaneFunctions.TriangleArea(this, tl, bl);
            var rightArea = PlaneFunctions.TriangleArea(this, tr, br);
            var botArea = PlaneFunctions.TriangleArea(this, bl, br);
            var fullArea = topArea + leftArea + rightArea + botArea;
            var realArea = PlaneFunctions.QuadrilateralArea(tl, tr, bl, br);
            return fullArea == realArea;
        }

        public Point Copy() => new Point(X, Y);
    }
}
