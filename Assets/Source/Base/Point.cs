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
            return X == other.X && Y == other.Y;
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Point a, Point b)
        {
            return a.X != b.X && a.Y != b.Y;
        }

        public override int GetHashCode()
        {
            int hash = 47;
            hash = hash * 59 + X.GetHashCode();
            hash = hash * 59 + Y.GetHashCode();
            return hash;
        }
    }
}
