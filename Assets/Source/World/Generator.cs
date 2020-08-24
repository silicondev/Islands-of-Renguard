using Assets.Source.Universal;
using dEvine_and_conquer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        private System.Random _rand;

        public Generator(float scale, float seed, int chunkSize, WorldMapper mapper)
        {
            Scale = scale;
            if (seed - (int)seed == 0) seed += 0.1F;
            Seed = seed;
            _rand = new System.Random((int)seed);
            ChunkSize = chunkSize;
            Mapper = mapper;
        }

        public ChunkData GenerateChunk(Point id)
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

                    
                    if (tile.Type == TileID.ENV.GRASS && TreeGen(new Point(perlinX, perlinY), height)) overlay.Type = TileID.ENV_OVERLAY.TREE;

                    overlays[y].Add(overlay);
                    tiles[y].Add(tile);
                }
            }
            ChunkData output = new ChunkData(tiles, overlays);
            return output;
        }

        private bool TreeGen(Point loc, float height)
        {
            int px = (int)Math.Floor(loc.X * 100);
            int py = (int)Math.Floor(loc.Y * 100);

            int hx = new System.Random(px).Next(100) + (int)height + (int)Seed;
            int hy = new System.Random(py).Next(100) + (int)height + (int)Seed;

            string hxstr = hx.ToString();
            string hystr = hy.ToString();

            string hxstrcom = hxstr + hystr + hxstr;
            string hystrcom = hystr + hxstr + hystr;

            int xEven = 0;
            int xOdd = 0;
            int yEven = 0;
            int yOdd = 0;

            int hxlen = hxstrcom.Length;
            int hylen = hystrcom.Length;

            int longestLength = hxlen > hylen ? hxlen : hylen;

            for (int i = 0; i < longestLength; i++)
            {
                if (i < hxlen)
                {
                    if (IsEven(int.Parse(hxstrcom[i].ToString()))) xEven++; else xOdd++;
                }
                if (i < hylen)
                {
                    if (IsEven(int.Parse(hystrcom[i].ToString()))) yEven++; else yOdd++;
                }
            }

            var evenX = xEven >= xOdd;
            var evenY = yEven <= yOdd;

            return evenX && evenY;
        }

        private bool IsEven(int val) => (val % 2) == 0;
    }

    public class ChunkData
    {
        public XYContainer<Tile> Tiles { get; }
        public XYContainer<Overlay> Overlays { get; }

        public ChunkData(XYContainer<Tile> tiles, XYContainer<Overlay> overlays)
        {
            Tiles = tiles;
            Overlays = overlays;
        }
    }
}
