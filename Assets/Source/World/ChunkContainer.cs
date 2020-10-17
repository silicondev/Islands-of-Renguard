using dEvine_and_conquer.Base;
using dEvine_and_conquer.Base.Interfaces;
using dEvine_and_conquer.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace dEvine_and_conquer.World
{
    public class ChunkContainer : IContainer<Chunk>
    {
        public List<Chunk> GeneratedChunks { get; set; } = new List<Chunk>();
        public List<Chunk> LoadedChunks { get; set; } = new List<Chunk>();
        public GameObject Object { get; private set; }
        public Transform Parent { get; }

        public ChunkContainer(Transform parent)
        {
            Parent = parent;
        }

        public void Generate(Chunk item)
        {
            item.Generate();
            GeneratedChunks.Add(item);
        }

        public int GeneratedCount() => GeneratedChunks.Count();

        public List<Chunk> GetGenerated() => new List<Chunk>(GeneratedChunks);

        public List<Chunk> GetLoaded() => new List<Chunk>(LoadedChunks);

        public void Load(Chunk item, Transform parent)
        {
            if (!GeneratedChunks.ContainsID(item.ID) || LoadedChunks.Contains(item))
                return;

            LoadedChunks.Add(item);

            var chunkObj = new GameObject("Chunk:" + item.IDStr);
            item.Object = GameObject.Instantiate(chunkObj, parent);
            item.Object.name = item.Object.name.Substring(0, item.Object.name.Length - 7);

            if (GameSystem.TileSelected && item.Contains(GameSystem.SelectedTile))
            {
                GameObject selector = VisualContainer.CreateObject("ui:selector", GameSystem.SelectedTile, 1.1f, item.Object.transform);
                item.Objects.Add(selector);
            }

            foreach (var block in item.Blocks)
            {
                if (!item.Contains(block.Location))
                {
                    //Debug.Log(string.Format("Attempting to load Block {0},{1} into Chunk {2} has failed.", block.Location.X.ToString(), block.Location.Y.ToString(), item.IDStr));
                    Debug.Log($"Attempting to load Block {block.Location} into Chunk {item.IDStr} has failed.");
                }

                GameObject obj = VisualContainer.CreateObject(block.Tile.Type.IdName, block.Location, 0, item.Object.transform);
                item.Objects.Add(obj);

                if (block.Overlay.Type != ObjectID.ENV.VOID)
                {
                    GameObject overlayObj = VisualContainer.CreateObject(block.Overlay.Type.IdName, block.Location, 0.1f, item.Object.transform);
                    item.Objects.Add(overlayObj);
                }
            }

            GameObject.Destroy(chunkObj);
        }

        public void LoadAll(List<Chunk> items)
        {
            foreach (var item in items)
            {
                Load(item, Parent);
            }
        }

        public int LoadedCount() => LoadedChunks.Count();

        public void Unload(Chunk item)
        {
            foreach (var obj in item.Objects)
            {
                GameObject.Destroy(obj);
            }
            item.Objects.Clear();
            GameObject.Destroy(item.Object);
            item.Object = null;
        }

        public void UnloadAll() => UnloadAll(LoadedChunks);

        public void UnloadAll(List<Chunk> items)
        {
            var list = new List<Chunk>(items);
            foreach (var item in list)
            {
                LoadedChunks.Remove(item);
                Unload(item);
            }
        }
    }
}
