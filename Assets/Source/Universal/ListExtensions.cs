using dEvine_and_conquer.Base;
using dEvine_and_conquer.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.Universal
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
        public static Chunk GetChunkWithTile(this List<Chunk> chunks, Point id)
        {
            foreach (var chunk in chunks)
            {
                if (chunk.Contains((int)id.X, (int)id.Y)) return chunk;
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
        public static Chunk GetChunkWithTile(this List<Chunk> chunks, int x, int y)
        {
            return chunks.GetChunkWithTile(new Point(x, y));
        }

        public static Tile GetTileFromID(this List<Chunk> chunks, Point id)
        {
            var x = (int)id.X;
            var y = (int)id.Y;

            foreach (var chunk in chunks)
            {
                var tiles = chunk.Tiles;
                if (tiles == null || tiles.Value[0] == null || tiles.Value[0][0] == null)
                    continue;

                var chunkX = (int)chunk.ID.X * 16;
                var chunkY = (int)chunk.ID.Y * 16;

                if (x >= chunkX && x < chunkX + 16 && y >= chunkY && y < chunkY + 16)
                {
                    var tileX = (int)id.X - chunkX;
                    var tileY = (int)id.Y - chunkY;

                    return tiles.Value[tileX][tileY];
                }
            }

            return null;
        }

        public static Tile GetTileFromID(this List<Chunk> chunks, int x, int y)
        {
            return chunks.GetTileFromID(new Point(x, y));
        }
    }
}
