using IslandsOfRenguard.Scripts.Frontend;
using IslandsOfRenguard.Scripts.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager;

namespace IslandsOfRenguard.Scripts.WorldGen
{
    public static class TileID
    {
        public static class ENV
        {
            public static readonly ID VOID = 000;
            public static readonly ID GRASS = 001;
            public static readonly ID STONE = 002;
            public static readonly ID SAND = 003;
            public static readonly ID WATER = 004;
            public static readonly ID CAVE = 005;
            public static readonly ID ROCK = 006;
        }
        public static class ENV_OVERLAY
        {
            public static readonly ID VOID = 100;
            public static readonly ID TREE = 101;
            public static readonly ID CHEST = 102;
            public static class ORE
            {
                public static readonly ID VOID = 200;
                public static readonly ID IRON = 201;
                public static readonly ID SILVER = 202;
                public static readonly ID GOLD = 203;
                public static readonly ID DIAMOND = 204;
            }
        }
    }

    public class Tile : VisualObject
    {
        public ID ID { get; set; }
        public float Height { get; }

        public Tile(ID id, float height, int xLoc, int yLoc) : base(xLoc, yLoc)
        {
            ID = id;
            Height = height;
        }
    }

    public class Overlay : VisualObject
    {
        public ID ID { get; set; }

        public Overlay(ID id, int xLoc, int yLoc) : base(xLoc, yLoc)
        {
            ID = id;
        }
    }
}
