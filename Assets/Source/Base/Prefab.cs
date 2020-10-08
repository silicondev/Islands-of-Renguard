using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.Base
{
    public class Prefab
    {
        public ID ID { get; private set; }
        public Dictionary<string, object> Data { get; private set; } = new Dictionary<string, object>();
        public string IdName { get; private set; }
        public string Name { get; private set; }

        private void Setup(ID id, string idName, string name)
        {
            ID = id;
            IdName = idName;
            Name = name;
        }

        public Prefab(ID id, string idName, string name)
        {
            Setup(id, idName, name);
        }

        public Prefab(ID id, string idName, string name, string dataId, object data)
        {
            Setup(id, idName, name);
            Data.Add(dataId, data);
        }

        public Prefab(ID id, string idName, string name, string dataId1, object data1, string dataId2, object data2)
        {
            Setup(id, idName, name);
            Data.Add(dataId1, data1);
            Data.Add(dataId2, data2);
        }

        public Prefab(ID id, string idName, string name, string dataId1, object data1, string dataId2, object data2, string dataId3, object data3)
        {
            Setup(id, idName, name);
            Data.Add(dataId1, data1);
            Data.Add(dataId2, data2);
            Data.Add(dataId3, data3);
        }

        public string GetString(string id)
        {
            if (Data.ContainsKey(id) && Data[id] is string value) return value;
            return "ERR";
        }

        public bool GetBool(string id)
        {
            if (Data.ContainsKey(id) && Data[id] is bool value) return value;
            return false;
        }

        public float GetFloat(string id)
        {
            if (Data.ContainsKey(id) && Data[id] is float value) return value;
            return -1;
        }

        public int GetInt(string id)
        {
            if (Data.ContainsKey(id) && Data[id] is int value) return value;
            return -1;
        }
    }

    public static class ObjectID
    {
        public static class ENV
        {
            public static readonly Prefab VOID = new Prefab(000, "env:void", "Empty Tile", "IsCollidable", false);
            public static class TILE
            {
                public static readonly Prefab GRASS = new Prefab(011, "tile:grass", "Grass", "IsCollidable", false);
                public static readonly Prefab STONE = new Prefab(012, "tile:stone", "Stone", "IsCollidable", true);
                public static readonly Prefab SAND = new Prefab(013, "tile:sand", "Sand", "IsCollidable", false);
                public static readonly Prefab WATER = new Prefab(014, "tile:water", "Water", "IsCollidable", true);
                public static readonly Prefab CAVE = new Prefab(015, "tile:cave", "Cave", "IsCollidable", false);
                public static readonly Prefab ROCK = new Prefab(016, "tile:rock", "Rock", "IsCollidable", true);
            }
            public static class OVERLAY
            {
                public static readonly Prefab TREE = new Prefab(101, "overlay:tree", "Tree", "IsCollidable", true);
                public static readonly Prefab CHEST = new Prefab(102, "overlay:chest", "Chest", "IsCollidable", true);
                
            }
            public static class VARIANT
            {
                public static class ORE
                {
                    public static readonly Variation VOID = new Variation(200);
                    public static readonly Variation IRON = new Variation(201);
                    public static readonly Variation SILVER = new Variation(202);
                    public static readonly Variation GOLD = new Variation(203);
                    public static readonly Variation DIAMOND = new Variation(204);
                }
            }
        }
        public static class ITEM
        {
            public static class GENERIC
            {
                public static readonly Prefab VOID = new Prefab(1000, "item:void", "Generic Item");
            }
        }
        public static class ENTITY
        {
            public static class NPC
            {
                public static class HUMAN
                {
                    public static class FRIENDLY
                    {
                        public static readonly Prefab MALE = new Prefab(2000, "entity:human.friendly.male", "Human Male", "Gender", 0);
                        public static readonly Prefab FEMALE = new Prefab(2001, "entity:human.friendly.female", "Human Female", "Gender", 1);
                    }
                }
            }
        }
    }
}
