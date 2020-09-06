using dEvine_and_conquer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditorInternal;
using UnityEngine;

namespace dEvine_and_conquer.World
{
    public class Chunk
    {
        public int xPos { get; private set; }
        public int yPos { get; private set; }
        public string IDStr
        {
            get
            {
                return (xPos / _generator.ChunkSize).ToString() + ";" + (yPos / _generator.ChunkSize).ToString();
            }
        }

        public Point ID
        {
            get
            {
                int chunkS = _generator.ChunkSize;
                return new Point(xPos / chunkS, yPos / chunkS);
            }
        }

        private Generator _generator;
        public XYContainer<Tile> Tiles { get; private set; } = new List<List<Tile>>();
        public XYContainer<Overlay> Overlays { get; private set; } = new List<List<Overlay>>();
        public List<GameObject> Objects { get; set; } = new List<GameObject>();
        public GameObject Object { get; set; }

        public Chunk(int x, int y, Generator gen)
        {
            Setup(x, y, gen);
        }

        public Chunk(Point id, Generator gen)
        {
            Setup((int)id.X * gen.ChunkSize, (int)id.Y * gen.ChunkSize, gen);
        }

        private void Setup(int x, int y, Generator gen)
        {
            _generator = gen;
            xPos = x;
            yPos = y;
        }

        public void Generate()
        {
            var gen = _generator.GenerateChunk(ID);
            Tiles = gen.Tiles;
            Overlays = gen.Overlays;
        }
        
        public bool Contains(int x, int y) => 
            x >= xPos &&
            x < xPos + _generator.ChunkSize &&
            y >= yPos &&
            y < yPos + _generator.ChunkSize;
    }
}
