using dEvine_and_conquer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace dEvine_and_conquer.Entity
{
    public class GenericEntity
    {
        public Inventory Inventory { get; }
        public List<Texture> Textures { get; }
        public Point Location { get; set; }
        public Prefab Type { get; set; }

        public GenericEntity(Point loc, Prefab type, int invSlots, List<Texture> textures)
        {
            Inventory = new Inventory(invSlots);
            Textures = textures;
            Location = loc;
            Type = type;
        }

        public GenericEntity(float x, float y, Prefab type, int invSlots, List<Texture> textures)
        {
            Inventory = new Inventory(invSlots);
            Textures = textures;
            Location = new Point(x, y);
            Type = type;
        }
    }

    public static class EntityID
    {
        public static class NPC
        {
            public static readonly Prefab HUMAN = new Prefab(1000, "entity:human", false);
        }
    }
}
