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
        public GenericEntity(int invSlots, List<Texture> textures)
        {
            Inventory = new Inventory(invSlots);
            Textures = textures;
        }
    }
}
