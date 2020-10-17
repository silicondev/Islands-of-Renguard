using Assets.Source.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager;

namespace dEvine_and_conquer.Base
{
    public class Prefab : DataContainer
    {
        public ID ID { get; private set; }
        
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

        public Prefab(ID id, string idName, string name, params (string Key, object Value)[] data)
        {
            Setup(id, idName, name);
            foreach (var item in data)
            {
                Data.Add(item.Key, item.Value);
            }
        }

        private static Dictionary<string, Prefab> _objectHold = new Dictionary<string, Prefab>();
        public static Dictionary<string, Prefab> Objects
        {
            get
            {
                if (!_objectHold.Any()) GetAllPrefabs();

                return _objectHold;
            }
        }

        private static void GetAllPrefabs(Type search = null)
        {
            if (search == null)
                search = typeof(ObjectID);

            var props = search.GetFields();
            foreach (var prop in props)
            {
                if (prop.FieldType == typeof(Prefab))
                {
                    var prefab = (Prefab)prop.GetValue(null);
                    _objectHold.Add(prefab.IdName, prefab);
                }
            }

            var classes = search.GetNestedTypes();
            foreach (var cls in classes)
            {
                GetAllPrefabs(cls);
            }
        }
    }

    public static class ObjectID
    {
        public static class ENV
        {
            public static readonly Prefab VOID = new Prefab(000, "env:void", "Empty Tile", ("IsCollidable", false));
            public static class TILE
            {
                public static readonly Prefab GRASS = new Prefab(011, "tile:grass", "Grass", ("IsCollidable", false ));
                public static readonly Prefab STONE = new Prefab(012, "tile:stone", "Stone", ("IsCollidable", true));
                public static readonly Prefab SAND = new Prefab(013, "tile:sand", "Sand", ("IsCollidable", false));
                public static readonly Prefab WATER = new Prefab(014, "tile:water", "Water", ("IsCollidable", true));
                public static readonly Prefab CAVE = new Prefab(015, "tile:cave", "Cave", ("IsCollidable", false));
                public static readonly Prefab ROCK = new Prefab(016, "tile:rock", "Rock", ("IsCollidable", true));
            }
            public static class OVERLAY
            {
                public static readonly Prefab TREE = new Prefab(101, "overlay:tree", "Tree", ("IsCollidable", true), ("Item1Id", "item:wood"), ("Item1Amin", 1), ("Item1Amax", 3));
                public static readonly Prefab CHEST = new Prefab(102, "overlay:chest", "Chest", ("IsCollidable", true));
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
            public static class MATERIAL
            {
                public static readonly Prefab WOOD = new Prefab(1001, "item:wood", "Wooden Log");
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
                        public static readonly Prefab MALE = new Prefab(2000, "entity:human.friendly.male", "Human Male", ("Gender", 0));
                        public static readonly Prefab FEMALE = new Prefab(2001, "entity:human.friendly.female", "Human Female", ("Gender", 1));
                    }
                }
            }
        }
    }
}
