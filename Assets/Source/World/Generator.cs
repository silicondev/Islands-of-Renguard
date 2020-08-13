using dEvine_and_conquer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;

namespace dEvine_and_conquer.World
{
    public class Generator
    {
        public float Scale { get; } = 0.01F;
        public float Seed { get; }
        public int ChunkSize { get; }
        public WorldMapper Mapper { get; private set; }

        public Generator(float scale, float seed, int chunkSize, WorldMapper mapper)
        {
            Scale = scale;
            if (seed - (int)seed == 0) seed += 0.1F;
            Seed = seed;
            ChunkSize = chunkSize;
            Mapper = mapper;
        }

        public Tuple<List<List<Tile>>,List<List<Overlay>>> GenerateChunk(Point id)
        {
            int xPos = (int)id.X * ChunkSize;
            int yPos = (int)id.Y * ChunkSize;
            float xStart = Seed + xPos;
            float yStart = Seed + yPos;
            var tiles = new List<List<Tile>>();
            var overlays = new List<List<Overlay>>();
            for (int y = 0; y < ChunkSize; y++)
            {
                tiles.Add(new List<Tile>());
                overlays.Add(new List<Overlay>());
                for (int x = 0; x < ChunkSize; x++)
                {
                    float perlinX = (x + xStart) * Scale;
                    float perlinY = (y + yStart) * Scale;
                    float height = Mathf.PerlinNoise(perlinX, perlinY) * 255;
                    Tile tile = new Tile(Mapper.ParseHeight(height), height, xPos + x, yPos + y);
                    Overlay overlay = new Overlay(TileID.ENV_OVERLAY.VOID, xPos + x, yPos + y);

                    if (tile.Type == TileID.ENV.GRASS)
                    {
                        var randomBoolNum = Random.Range(0.0F, 1.0F);
                        var randomBool = randomBoolNum >= 0.95F ? true : false;
                        if (randomBool) overlay.Type = TileID.ENV_OVERLAY.TREE;
                    }

                    overlays[y].Add(overlay);
                    tiles[y].Add(tile);
                }
            }
            Tuple<List<List<Tile>>,List<List<Overlay>>> output = new Tuple<List<List<Tile>>, List<List<Overlay>>>(tiles, overlays);
            return output;
        }
    }
}
