using dEvine_and_conquer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager;

namespace dEvine_and_conquer.World
{
    public static class TileID
    {
        public static class ENV
        {
            public static readonly Prefab VOID = new Prefab(000, "tile:void", false);
            public static readonly Prefab GRASS = new Prefab(001, "tile:grass", false);
            public static readonly Prefab STONE = new Prefab(002, "tile:stone", true);
            public static readonly Prefab SAND = new Prefab(003, "tile:sand", false);
            public static readonly Prefab WATER = new Prefab(004, "tile:water", true);
            public static readonly Prefab CAVE = new Prefab(005, "tile:cave", false);
            public static readonly Prefab ROCK = new Prefab(006, "tile:rock", true);
        }
        public static class ENV_OVERLAY
        {
            public static readonly Prefab VOID = new Prefab(100, "overlay:void", false);
            public static readonly Prefab TREE = new Prefab(101, "overlay:tree", true);
            public static readonly Prefab CHEST = new Prefab(102, "overlay:chest", true);
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
        public Prefab Type { get; set; }
        public float Height { get; }

        public Tile(Prefab type, float height, int xLoc, int yLoc) : base(xLoc, yLoc)
        {
            Type = type;
            Height = height;
        }
    }

    public class Overlay : VisualObject
    {
        public Prefab Type { get; set; }

        public Overlay(Prefab type, int xLoc, int yLoc) : base(xLoc, yLoc)
        {
            Type = type;
        }
    }
}
