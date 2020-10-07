using dEvine_and_conquer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
        private bool _genTree = true;

        public Generator(float scale, float seed, int chunkSize, WorldMapper mapper)
        {
            Scale = scale;
            if (seed - (int)seed == 0) seed += 0.1F;
            Seed = seed;
            _rand = new System.Random((int)seed);
            ChunkSize = chunkSize;
            Mapper = mapper;
        }

        //public ChunkData GenerateChunk(Point id)
        public Block[] GenerateChunk(Point id)
        {
            int xChunkPos = (int)id.X * ChunkSize;
            int yChunkPos = (int)id.Y * ChunkSize;
            float xStart = Seed + xChunkPos;
            float yStart = Seed + yChunkPos;

            //var tiles = new List<List<Tile>>();
            //var overlays = new List<List<Overlay>>();
            Block[] blocks = new Block[ChunkSize * ChunkSize];
            int i = 0;

            for (int y = 0; y < ChunkSize; y++)
            {
                //tiles.Add(new List<Tile>());
                //overlays.Add(new List<Overlay>());
                for (int x = 0; x < ChunkSize; x++)
                {
                    int xPos = x + xChunkPos;
                    int yPos = y + yChunkPos;
                    float perlinX = (x + xStart) * Scale;
                    float perlinY = (y + yStart) * Scale;
                    float height = Mathf.PerlinNoise(perlinX, perlinY) * 255;

                    //Tile tile = new Tile(Mapper.ParseHeight(height), height, xPos, yPos);
                    //Overlay overlay = new Overlay(ObjectID.ENV_OVERLAY.VOID, xPos, yPos);

                    //if (tile.Type == ObjectID.ENV.GRASS && TreeGen(new Point(perlinX, perlinY), height)) overlay.Type = ObjectID.ENV_OVERLAY.TREE;

                    Prefab tile = Mapper.ParseHeight(height);
                    Prefab overlay =
                        tile == ObjectID.ENV.GRASS &&
                        TreeGen(new Point(perlinX, perlinY), height) ?
                            ObjectID.ENV_OVERLAY.TREE :
                            ObjectID.ENV_OVERLAY.VOID;

                    blocks[i] = new Block(xPos, yPos, tile, height, overlay);
                    i++;

                    //overlays[y].Add(overlay);
                    //tiles[y].Add(tile);
                }
            }
            //ChunkData output = new ChunkData(tiles, overlays);
            //return output;
            return blocks;
        }

        private bool TreeGen(Point loc, float height)
        {
            // Generates a seemingly random number from the RNG using the world seed, the tile's height and the location of the pixel in the perlin image.
            int rx = _rand.Next(100) + (int)height + (int)(loc.X * 100);
            int ry = _rand.Next(100) + (int)height + (int)(loc.Y * 100);

            string rxstr = rx.ToString();
            string rystr = ry.ToString();

            // Agregates the strings together to lengthen them out, converts it into a list of digits then adds all those digits together.
            var xNum = (rxstr + rystr + rxstr).Select(x => int.Parse(x.ToString())).Sum();
            var yNum = (rystr + rxstr + rystr).Select(x => int.Parse(x.ToString())).Sum();

            // Getting the even number bools
            var evenX = IsEven(xNum);
            var evenY = IsEven(yNum);

            // Places a tree if both xNum and yNum are even
            var placeTree = evenX && evenY;

            // Only outputs to place a tree every other time a tree should be placed to thin the trees out a bit
            var output = _genTree && placeTree;
            if (placeTree)
            {
                if (_genTree)
                    _genTree = false;
                else
                    _genTree = true;
            }

            return output;
        }

        private bool IsEven(int val) => (val % 2) == 0;
    }

    //public class ChunkData
    //{
    //    public XYContainer<Tile> Tiles { get; }
    //    public XYContainer<Overlay> Overlays { get; }

    //    public ChunkData(XYContainer<Tile> tiles, XYContainer<Overlay> overlays)
    //    {
    //        Tiles = tiles;
    //        Overlays = overlays;
    //    }
    //}
}
