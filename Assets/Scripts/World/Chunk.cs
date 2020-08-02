using IslandsOfRenguard.Scripts.Universal;
using IslandsOfRenguard.Scripts.WorldGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditorInternal;
using UnityEngine;

namespace IslandsOfRenguard.Assets.Scripts.World
{
    public class Chunk
    {
        public int xPos { get; private set; }
        public int yPos { get; private set; }
        public string IDStr
        {
            get
            {
                return (xPos / _settings.ChunkSize).ToString() + ";" + (yPos / _settings.ChunkSize).ToString();
            }
        }

        public Point ID
        {
            get
            {
                int chunkS = _settings.ChunkSize;
                return new Point(xPos / chunkS, yPos / chunkS);
            }
        }

        private GeneratorSettings _settings;
        private WorldMapper _mapper;
        public List<List<Tile>> Tiles { get; private set; } = new List<List<Tile>>();
        public List<GameObject> Objects { get; set; } = new List<GameObject>();

        public Chunk(int x, int y, GeneratorSettings gen, WorldMapperSettings worldMap)
        {
            Setup(x, y, gen, worldMap);
        }

        public Chunk(Point id, GeneratorSettings gen, WorldMapperSettings worldMap)
        {
            Setup((int)id.X * gen.ChunkSize, (int)id.Y * gen.ChunkSize, gen, worldMap);
        }

        private void Setup(int x, int y, GeneratorSettings gen, WorldMapperSettings worldMap)
        {
            _settings = gen;
            xPos = x;
            yPos = y;
            _mapper = new WorldMapper(worldMap);
        }

        public void Generate()
        {
            float xStart = _settings.Seed + xPos;
            float yStart = _settings.Seed + yPos;
            int chunkSize = _settings.ChunkSize;
            Tiles = new List<List<Tile>>();
            for (int y = 0; y < chunkSize; y++)
            {
                Tiles.Add(new List<Tile>());
                for (int x = 0; x < chunkSize; x++)
                {
                    float perlinX = x + xStart;
                    float perlinY = y + yStart;
                    float height = Mathf.PerlinNoise(perlinX, perlinY)*255;
                    Tile tile = new Tile(_mapper.ParseHeight(height), height, xPos + x, yPos + y);
                    Tiles[y].Add(tile);
                }
            }
        }
        
        public bool Contains(int x, int y) => 
            x >= xPos &&
            x < xPos + _settings.ChunkSize &&
            y >= yPos &&
            y < yPos + _settings.ChunkSize;
    }
}
