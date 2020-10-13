using dEvine_and_conquer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.World
{
    public class Block : VisualObject
    {
        public Tile Tile { get; }
        public Overlay Overlay { get; }
        public Point Location { get; set; }
        public bool IsCollidable => Tile.Type.GetBool("IsCollidable") ? true : Overlay.Type.GetBool("IsCollidable");

        public Block(int x, int y, Tile tile, Overlay overlay)// : base(x, y)
        {
            Tile = tile;
            Overlay = overlay;

            Location = new Point(x, y);
        }

        public Block(int x, int y, Prefab tile, float height, Prefab overlay)// : base(x, y)
        {
            Tile = new Tile(tile, height);
            Overlay = new Overlay(overlay);

            Location = new Point(x, y);
        }

        public Point GetLocation()
        {
            return Location;
        }
    }

    public class Tile
    {
        public Prefab Type { get; set; }
        public float Height { get; }

        public Tile(Prefab type, float height)
        {
            Type = type;
            Height = height;
        }
    }

    public class Overlay
    {
        public Prefab Type { get; set; }

        public Overlay(Prefab type)
        {
            Type = type;
        }
    }
    
}
