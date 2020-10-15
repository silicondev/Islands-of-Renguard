using dEvine_and_conquer.Base;
using dEvine_and_conquer.Base.Interfaces;
using dEvine_and_conquer.Entity;
using dEvine_and_conquer.Scripts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace dEvine_and_conquer.Entity
{
    public class EntityContainer : IContainer<GenericEntity>
    {
        public List<GenericEntity> GeneratedEntities { get; set; } = new List<GenericEntity>();
        public List<GenericEntity> LoadedEntities { get; set; } = new List<GenericEntity>();
        public GameObject Object { get; private set; }
        public Transform Parent { get; }

        public EntityContainer(Transform parent)
        {
            Parent = parent;
            var obj = new GameObject("Entities");
            Object = GameObject.Instantiate(obj, Parent);
            Object.name = Object.name.Substring(0, Object.name.Length - 7);
            GameObject.Destroy(obj);
        }

        public List<GenericEntity> GetGenerated() => GeneratedEntities;

        public List<GenericEntity> GetLoaded() => LoadedEntities;

        public void Load(GenericEntity item, Transform parent)
        {
            GameObject entityObj = VisualContainer.CreateObject(item.Type.IdName, item.Location, 0.2f, parent);

            item.Instance = entityObj.AddComponent<EntityManager>();
            //item.Instance.Renderer = entityObj.AddComponent<SpriteRenderer>();
            item.Object = entityObj;
            item.Instance.Entity = item;
        }

        public void LoadAll(List<GenericEntity> items)
        {
            UnloadAll();
            foreach (var item in items)
            {
                if (!GeneratedEntities.Contains(item) || LoadedEntities.Contains(item))
                    continue;

                Load(item, Object.transform);
                LoadedEntities.Add(item);
            }
        }

        public void Unload(GenericEntity item)
        {
            GameObject.Destroy(item.Object);
        }

        public void UnloadAll() => UnloadAll(LoadedEntities);

        public void Generate(GenericEntity item)
        {
            GeneratedEntities.Add(item);
        }

        public int LoadedCount() => LoadedEntities.Count();

        public int GeneratedCount() => GeneratedEntities.Count();

        public void UnloadAll(List<GenericEntity> items)
        {
            var list = new List<GenericEntity>(items);
            foreach (var item in list)
            {
                LoadedEntities.Remove(item);
                Unload(item);
            }
        }
    }
}
