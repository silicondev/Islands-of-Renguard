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
    }
}
