using dEvine_and_conquer.Base;
using dEvine_and_conquer.Base.Interfaces;
using dEvine_and_conquer.Entity;
using dEvine_and_conquer.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditorInternal;
using UnityEngine;

namespace dEvine_and_conquer.World
{
    public class Chunk : IUpdateable
    {
        private GameSystem _system;
        public int xPos { get; private set; }
        public int yPos { get; private set; }
        public string IDStr => ((int)ID.X).ToString() + ";" + ((int)ID.Y).ToString();

        public Point ID
        {
            get
            {
                int chunkS = _generator.ChunkSize;
                return new Point(xPos / chunkS, yPos / chunkS);
            }
        }

        public int Size
        {
            get
            {
                if (_generator != null) return _generator.ChunkSize;
                return 0;
            }
        }

        private Generator _generator;
        public Block[] Blocks { get; private set; }
        public List<GameObject> Objects { get; set; } = new List<GameObject>();
        public GameObject Object { get; set; }
        public bool IsGenerated { get; private set; } = false;

        public Chunk(int x, int y, Generator gen)
        {
            Setup(x, y, gen);
        }

        public Chunk(Point id, Generator gen)
        {
            Setup((int)id.X * gen.ChunkSize, (int)id.Y * gen.ChunkSize, gen);
        }

        public Chunk(int x, int y, Generator gen, Block[] blocks)
        {
            Setup(x, y, gen, blocks);
        }

        public Chunk(Point id, Generator gen, Block[] blocks)
        {
            Setup((int)id.X * gen.ChunkSize, (int)id.Y * gen.ChunkSize, gen, blocks);
        }

        private void Setup(int x, int y, Generator gen, Block[] blocks = null)
        {
            _generator = gen;
            xPos = x;
            yPos = y;
            if (blocks != null)
            {
                IsGenerated = true;
                Blocks = blocks;
            }
        }

        public void Generate()
        {
            Blocks = _generator.GenerateChunk(ID);
            IsGenerated = true;
        }

        public bool Contains(Point loc)
        {
            return Contains(loc.X, loc.Y);
        }

        public bool Contains(float x, float y) =>
            x.Floor() >= xPos && x.Floor() < xPos + _generator.ChunkSize &&
            y.Floor() >= yPos && y.Floor() < yPos + _generator.ChunkSize;

        public void Update()
        {

        }
    }
}
