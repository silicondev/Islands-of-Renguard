using IslandsOfRenguard.Scripts.Universal;
using IslandsOfRenguard.Scripts.WorldGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace IslandsOfRenguard.Scripts.WorldGen
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

        public List<List<Tile>> GenerateChunk(Point id)
        {
            int xPos = (int)id.X * ChunkSize;
            int yPos = (int)id.Y * ChunkSize;
            float xStart = Seed + xPos;
            float yStart = Seed + yPos;
            var tiles = new List<List<Tile>>();
            for (int y = 0; y < ChunkSize; y++)
            {
                tiles.Add(new List<Tile>());
                for (int x = 0; x < ChunkSize; x++)
                {
                    float perlinX = (x + xStart) * Scale;
                    float perlinY = (y + yStart) * Scale;
                    float height = Mathf.PerlinNoise(perlinX, perlinY) * 255;
                    Tile tile = new Tile(Mapper.ParseHeight(height), height, xPos + x, yPos + y);
                    tiles[y].Add(tile);
                }
            }
            return tiles;
        }
    }
}
