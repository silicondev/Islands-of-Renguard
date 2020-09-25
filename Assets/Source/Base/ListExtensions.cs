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
        public static Chunk GetChunk(this List<Chunk> chunks, Point id)
        {
            foreach (var chunk in chunks)
            {
                if (chunk.ID == id) return chunk;
            }
            return null;
        }

        /// <summary>
        /// Gets a chunk from the List that matches the id. Returns null if nothing is found.
        /// </summary>
        /// <param name="chunks">The list of chunks to search through.</param>
        /// <param name="x">X value of the id to find.</param>
        /// <param name="y">Y value of the id to find.</param>
        /// <returns></returns>
        public static Chunk GetChunk(this List<Chunk> chunks, int x, int y)
        {
            return chunks.GetChunk(new Point(x, y));
        }

        /// <summary>
        /// Gets a chunk from the List that contains the tile which matches the id. Returns null if nothing is found.
        /// </summary>
        /// <param name="chunks">The list of chunks to search through.</param>
        /// <param name="id">The id of the tile to find.</param>
        /// <returns></returns>
        public static Chunk GetChunkWithPosition(this List<Chunk> chunks, Point pos)
        {
            foreach (var chunk in chunks)
            {
                if (chunk.Contains((int)pos.X, (int)pos.Y)) return chunk;
            }
            return null;
        }

        /// <summary>
        /// Gets a chunk from the List that contains the tile which matches the id. Returns null if nothing is found.
        /// </summary>
        /// <param name="chunks">The list of chunks to search through.</param>
        /// <param name="x">X value of the tiles id to find.</param>
        /// <param name="y">Y value of the tiles id to find.</param>
        /// <returns></returns>
        public static Chunk GetChunkWithPosition(this List<Chunk> chunks, int x, int y)
        {
            return chunks.GetChunkWithPosition(new Point(x, y));
        }

        public static Tile GetTileFromID(this List<Chunk> chunks, Point id)
        {
            var x = (int)id.X;
            var y = (int)id.Y;

            foreach (var chunk in chunks)
            {
                var tiles = chunk.Tiles;
                if (tiles == null || tiles.Get(0, 0) == null)
                    continue;

                var chunkX = (int)chunk.ID.X * 16;
                var chunkY = (int)chunk.ID.Y * 16;

                if (x >= chunkX && x < chunkX + 16 && y >= chunkY && y < chunkY + 16)
                {
                    var tileX = (int)id.X - chunkX;
                    var tileY = (int)id.Y - chunkY;

                    return tiles.Get(tileX, tileY);
                }
            }

            return null;
        }

        public static Tile GetTileFromID(this List<Chunk> chunks, int x, int y)
        {
            return chunks.GetTileFromID(new Point(x, y));
        }

        public static bool AddEntityToWorld(this List<Chunk> chunks, GenericEntity entity)
        {
            foreach (var chunk in chunks)
            {
                if (chunk.Contains(entity.Location))
                {
                    chunk.Entities.Add(entity);
                    return true;
                }
            }
            return false;
        }

        public static void UpdateAll<T>(this List<T> list, GameSystem system) where T : IUpdateable
        {
            foreach (var item in list)
            {
                item.Update(system);
            }
        }

        public static List<GenericEntity> GetAllEntities(this List<Chunk> chunks)
        {
            var list = new List<GenericEntity>();
            chunks.ForEach(x => list.AddRange(x.Entities));
            return list;
        }

        public static T Min<T>(this List<T> list, Func<T, float> prop) => list.Aggregate((i1,i2) => prop(i1) < prop(i2) ? i1 : i2);
        public static T Max<T>(this List<T> list, Func<T, float> prop) => list.Aggregate((i1, i2) => prop(i1) > prop(i2) ? i1 : i2);

        public static float MinValue<T>(this List<T> list, Func<T, float> prop) => prop(list.Aggregate((i1, i2) => prop(i1) < prop(i2) ? i1 : i2));
        public static float MaxValue<T>(this List<T> list, Func<T, float> prop) => prop(list.Aggregate((i1, i2) => prop(i1) > prop(i2) ? i1 : i2));

        public static Tile Get(this List<Tile> tiles, int x, int y) => tiles.Get(new Point(x, y));

        public static Tile Get(this List<Tile> tiles, Point loc) => tiles.Where(x => (int)x.Location.X == (int)loc.X && (int)x.Location.Y == (int)loc.Y).First();
    }
}
