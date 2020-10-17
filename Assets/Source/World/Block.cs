using dEvine_and_conquer.Base;
using dEvine_and_conquer.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace dEvine_and_conquer.World
{
    public class Block : VisualObject
    {
        public Tile Tile { get; }
        public Overlay Overlay { get; }
        public Point VisualLocation { get; set; }
        public bool IsCollidable => Tile.Type.GetBool("IsCollidable") ? true : Overlay.Type.GetBool("IsCollidable");

        public Block(int x, int y, Tile tile, Overlay overlay) : base(x, y)
        {
            Tile = tile;
            Overlay = overlay;
        }

        public Block(int x, int y, Prefab tile, float height, Prefab overlay) : base(x, y)
        {
            Tile = new Tile(tile, height);
            Overlay = new Overlay(overlay);
        }
    }

    public class Tile : Fabrication
    {
        public float Height { get; }

        public Tile(Prefab type, float height) : base(type)
        {
            Height = height;
        }
    }

    public class Overlay : Harvestable
    {
        public Overlay(Prefab type) : base(type)
        {

        }
    }

    public abstract class Fabrication
    {
        public Prefab Type { get; set; }

        public Fabrication(Prefab type)
        {
            Type = type;
        }
    }
    
    public abstract class Harvestable : Fabrication
    {
        public Harvestable(Prefab type) : base(type)
        {

        }

        public List<ItemBag> Harvest()
        {
            List<ItemBag> items = new List<ItemBag>();
            int i = 1;
            string idstr = "Item1Id";
            while (Type.DataContainsKey(idstr))
            {
                //string minstr = string.Format("Item{0}Amin", i.ToString());
                //string maxstr = string.Format("Item{0}Amax", i.ToString());
                string minstr = $"Item{i}Amin";
                string maxstr = $"Item{i}Amax";

                var id = Type.GetString(idstr);
                var amountMin = Type.GetInt(minstr);
                var amountMax = Type.GetInt(maxstr);
                var am = Random.Range(amountMin, amountMax);
                ItemBag bag = new ItemBag(Prefab.Objects[id], am);
                items.Add(bag);
                i++;
                //idstr = string.Format("Item{0}Id", i.ToString());
                idstr = $"Item{i}Id";
            }
            return items;
        }
    }
}
