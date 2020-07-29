using IslandsOfRenguard.Assets.Scripts.World;
using IslandsOfRenguard.Scripts.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IslandsOfRenguard.Scripts.World
{
    public static class ChunkListExtensions
    {
        public static Chunk GetChunk(this List<Chunk> chunks, Point id)
        {
            foreach (var chunk in chunks)
            {
                if (chunk.ID == id) return chunk;
            }
            return null;
        }

        public static Chunk GetChunk(this List<Chunk> chunks, int x, int y)
        {
            return chunks.GetChunk(new Point(x, y));
        }
    }
}
