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

        private Generator _generator;
        //public XYContainer<Tile> Tiles { get; private set; } = new List<List<Tile>>();
        //public XYContainer<Overlay> Overlays { get; private set; } = new List<List<Overlay>>();
        public Block[] Blocks;
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
            //var gen = _generator.GenerateChunk(ID);
            //Tiles = gen.Tiles;
            //Overlays = gen.Overlays;
            Blocks = _generator.GenerateChunk(ID);
        }

        public bool Contains(Point loc)
        {
            return Contains(loc.X.Floor(), loc.Y.Floor());
        }

        public bool Contains(int x, int y) =>
            x >= xPos && x < xPos + _generator.ChunkSize &&
            y >= yPos && y < yPos + _generator.ChunkSize;

        public void Update(GameSystem system)
        {
            List<GenericEntity> toRemove = new List<GenericEntity>();
            foreach (var entity in Entities)
            {
                entity.Update(system);
                if (!Contains(entity.Location))
                {
                    //GameObject.Destroy(entity.Instance.gameObject);
                    _system.GeneratedChunks.AddEntityToWorld(entity);
                    //Entities.Remove(entity); //This just despawns the entity if they leave the generated area. This may need fixing.
                    toRemove.Add(entity);
                }
            }

            foreach (var entity in toRemove)
            {
                Entities.Remove(entity);
            }
        }
    }
}
