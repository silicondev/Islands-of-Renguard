using dEvine_and_conquer.AI.Pathfinding.AStar;
using dEvine_and_conquer.Base;
using dEvine_and_conquer.Base.Interfaces;
using dEvine_and_conquer.Entity;
using dEvine_and_conquer.Scripts;
using dEvine_and_conquer.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace dEvine_and_conquer.Base
{
    public static class ListExtensions
    {

        /// <summary>
        /// Gets a chunk from the List that matches the id. Returns null if nothing is found.
        /// </summary>
        /// <param name="chunks">The list of chunks to search through.</param>
        /// <param name="id">The id to find.</param>
        /// <returns></returns>
        public static Chunk GetChunk(this List<Chunk> chunks, Point id) => chunks.Find(x => x.ID == id);

        /// <summary>
        /// Gets a chunk from the List that matches the id. Returns null if nothing is found.
        /// </summary>
        /// <param name="chunks">The list of chunks to search through.</param>
        /// <param name="x">X value of the id to find.</param>
        /// <param name="y">Y value of the id to find.</param>
        /// <returns></returns>
        public static Chunk GetChunk(this List<Chunk> chunks, int x, int y) => chunks.GetChunk(new Point(x, y));

        /// <summary>
        /// Gets a chunk from the List that contains the tile which matches the id. Returns null if nothing is found.
        /// </summary>
        /// <param name="chunks">The list of chunks to search through.</param>
        /// <param name="id">The id of the tile to find.</param>
        /// <returns></returns>
        public static Chunk GetChunkWithPosition(this List<Chunk> chunks, Point pos) => chunks.Find(x => x.Contains(pos));

        /// <summary>
        /// Gets a chunk from the List that contains the tile which matches the id. Returns null if nothing is found.
        /// </summary>
        /// <param name="chunks">The list of chunks to search through.</param>
        /// <param name="x">X value of the tiles id to find.</param>
        /// <param name="y">Y value of the tiles id to find.</param>
        /// <returns></returns>
        public static Chunk GetChunkWithPosition(this List<Chunk> chunks, int x, int y) => chunks.GetChunkWithPosition(new Point(x, y));

        public static Block GetBlockFromID(this List<Chunk> chunks, Point id) => chunks.Find(c => c.Contains(id))?.Blocks.ToList().Find(b => b.Location == id);

        public static Block GetBlockFromID(this List<Chunk> chunks, int x, int y) => chunks.GetBlockFromID(new Point(x, y));

        //public static bool AddEntityToWorld(this List<Chunk> chunks, GenericEntity entity)
        //{
        //    foreach (var chunk in chunks)
        //    {
        //        if (chunk.Contains(entity.Location))
        //        {
        //            chunk.Entities.Add(entity);
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public static void UpdateAll<T>(this List<T> list) where T : IUpdateable
        {
            foreach (var item in list)
            {
                item.Update();
            }
        }

        //public static List<GenericEntity> GetAllEntities(this List<Chunk> chunks)
        //{
        //    var list = new List<GenericEntity>();
        //    chunks.ForEach(x => list.AddRange(x.Entities));
        //    return list;
        //}

        public static Chunk GetContains(this List<Chunk> chunks, Point loc) => chunks.Find(x => x.Contains(loc));
        public static bool Contains(this List<Chunk> chunks, float x, float y) => chunks.Any(c => c.Contains(x, y));
        public static bool Contains(this List<Chunk> chunks, Point loc) => chunks.Any(c => c.Contains(loc));

        public static T Min<T>(this List<T> list, Func<T, float> prop) => list.Aggregate((i1, i2) => prop(i1) < prop(i2) ? i1 : i2);
        public static T Max<T>(this List<T> list, Func<T, float> prop) => list.Aggregate((i1, i2) => prop(i1) > prop(i2) ? i1 : i2);

        public static float MinValue<T>(this List<T> list, Func<T, float> prop) => prop(list.Aggregate((i1, i2) => prop(i1) < prop(i2) ? i1 : i2));
        public static float MaxValue<T>(this List<T> list, Func<T, float> prop) => prop(list.Aggregate((i1, i2) => prop(i1) > prop(i2) ? i1 : i2));

        public static T Get<T>(this List<T> tiles, int x, int y) where T : VisualObject => tiles.Get(new Point(x, y));

        public static T Get<T>(this List<T> tiles, Point loc) where T : VisualObject => tiles.Where(x => x.GetLocation().Flatten() == loc).First();
        public static T Get<T>(this T[] tiles, int x, int y) where T : VisualObject => tiles.Get(new Point(x, y));

        public static T Get<T>(this T[] tiles, Point loc) where T : VisualObject => tiles.Where(x => x.GetLocation().Flatten() == loc).First();
    }
}
