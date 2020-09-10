using dEvine_and_conquer.Base;
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
    public class Chunk
    {
        private GameSystem _system;
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
        public List<GenericEntity> Entities { get; } = new List<GenericEntity>();
        public GameObject Object { get; set; }

        public Chunk(int x, int y, Generator gen, GameSystem system = null)
        {
            Setup(x, y, gen, system);
        }

        public Chunk(Point id, Generator gen, GameSystem system = null)
        {
            Setup((int)id.X * gen.ChunkSize, (int)id.Y * gen.ChunkSize, gen, system);
        }

        private void Setup(int x, int y, Generator gen, GameSystem system)
        {
            _generator = gen;
            if (system != null) _system = system;
            xPos = x;
            yPos = y;
        }

        public void Generate()
        {
            var gen = _generator.GenerateChunk(ID);
            Tiles = gen.Tiles;
            Overlays = gen.Overlays;
        }

        public bool Contains(Point loc)
        {
            return Contains((int)loc.X, (int)loc.Y);
        }
        
        public bool Contains(int x, int y) => 
            x >= xPos &&
            x < xPos + _generator.ChunkSize &&
            y >= yPos &&
            y < yPos + _generator.ChunkSize;

        public void Refresh()
        {
            foreach (var entity in Entities)
            {
                if (!Contains(entity.Location))
                {
                    foreach (var chunk in _system.GeneratedChunks)
                    {
                        if (chunk.ID != ID && chunk.Contains(entity.Location))
                        {
                            chunk.Entities.Add(entity);
                            continue;
                        }
                    }
                    Entities.Remove(entity); //This just despawns the entity if they leave the generated area. This may need fixing.
                }
            }
        }
    }
}
