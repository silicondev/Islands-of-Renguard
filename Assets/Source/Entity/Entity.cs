using dEvine_and_conquer.AI.Pathfinding.AStar;
using dEvine_and_conquer.AI.Pathfinding.Interfaces;
using dEvine_and_conquer.Base;
using dEvine_and_conquer.Base.Interfaces;
using dEvine_and_conquer.Scripts;
using dEvine_and_conquer.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace dEvine_and_conquer.Entity
{
    public abstract class GenericEntity : IUpdateable
    {
        public Inventory Inventory { get; }
        public List<Sprite> Textures { get; protected set; }
        public Point Location { get; set; }
        public Prefab Type { get; set; }
        public EntityManager Instance { get; set; }
        protected IPathfinder Pathfinder = new AStarPathfinder();
        private int _animRefresh = 150;
        public Point GoingTo;
        public bool isMoving = false;
        protected List<Tile> currentPath;
        protected int pathProgress = 0;
        private float _moveSpeed = 5.0F;

        public GenericEntity(Point loc, Prefab type, int invSlots, List<Sprite> textures = null)
        {
            Inventory = new Inventory(invSlots);
            if (textures == null)
                Textures = new List<Sprite>();
            else
                Textures = textures;
            Location = loc;
            Type = type;
        }

        public GenericEntity(float x, float y, Prefab type, int invSlots, List<Sprite> textures = null)
        {
            Inventory = new Inventory(invSlots);
            if (textures == null)
                Textures = new List<Sprite>();
            else
                Textures = textures;
            Location = new Point(x, y);
            Type = type;
        }

        int animTimer = 0;
        public void Update(GameSystem system)
        {
            UpdateSpecific(system);
            if (animTimer >= _animRefresh)
            {
                animTimer = 0;
                UpdateAnimation();
            }
            animTimer++;
            if (isMoving)
            {
                var newLoc = currentPath[pathProgress].Location;
                Location.X = Mathf.MoveTowards(Location.X, newLoc.X, Time.deltaTime * _moveSpeed);
                Location.Y = Mathf.MoveTowards(Location.Y, newLoc.Y, Time.deltaTime * _moveSpeed);
                if (Location == newLoc) pathProgress++;
            }
            if (currentPath != null && pathProgress == currentPath.Count - 1)
            {
                isMoving = false;
                pathProgress = 0;
            }
        }

        protected abstract void UpdateSpecific(GameSystem system);

        int currentFrame = 0;
        private void UpdateAnimation()
        {
            Instance.Renderer.sprite = Textures[currentFrame];
            currentFrame++;
            if (currentFrame >= Textures.Count) currentFrame = 0;
        }

        public void GoTo(Point destination, GameSystem system)
        {
            if (destination == Location) return;

            //var chunks = system.LoadedChunks2D;
            //XYContainer<Tile> tiles = new List<List<Tile>>();

            //for (int y = 0; y < (chunks.Count(false) * 16) - 1; y++)
            //{
            //    tiles.AddLine();
            //    for (int x = 0; x < (chunks.Count(true) * 16) - 1; x++)
            //    {
            //        var chunkID = new Point((x / 16f).Floor(), (y / 16f).Floor());
            //        var chunk = chunks.Get(chunkID);
            //        tiles.Add(chunk.Tiles.Get(x - ((int)chunkID.X * 16), y - ((int)chunkID.Y * 16)));
            //    }
            //}

            var chunks = system.LoadedChunks;
            List<Tile> tiles = new List<Tile>();

            foreach (var chunk in chunks)
            {
                var cTiles = chunk.Tiles;
                for (int y = 0; y < cTiles.Count(false); y++)
                {
                    for (int x = 0; x < cTiles.Count(true); x++)
                    {
                        tiles.Add(cTiles.Get(x, y));
                    }
                }
            }

            Pathfinder.UpdateWorld(tiles);

            //var currentChunk = system.LoadedChunks.GetChunkWithPosition(Location);
            //var destinationChunk = system.LoadedChunks.GetChunkWithPosition(destination);
            //var lowestY = currentChunk.ID.Y < destinationChunk.ID.Y ? currentChunk : destinationChunk.ID.Y < currentChunk.ID.Y ? destinationChunk : currentChunk;
            //var lowestX = currentChunk.ID.X < destinationChunk.ID.X ? currentChunk : destinationChunk.ID.X < currentChunk.ID.X ? destinationChunk : currentChunk;
            //var highestY = lowestY == destinationChunk ? currentChunk : destinationChunk;
            //var highestX = lowestX == destinationChunk ? currentChunk : destinationChunk;

            //XYContainer<Chunk> chunksToMap = new List<List<Chunk>>();
            //XYContainer<Tile> tilesToMap = new List<List<Tile>>();

            //if (currentChunk == destinationChunk)
            //{
            //    tilesToMap = currentChunk.Tiles;
            //} else
            //{
            //    int ix = 0;
            //    int iy = 0;
            //    for (int y = (int)lowestY.ID.Y; y <= highestY.ID.Y; y++)
            //    {
            //        chunksToMap.AddLine();
            //        for (int x = (int)lowestX.ID.X; x <= highestX.ID.X; x++)
            //        {
            //            Chunk newChunk;
            //            if (system.GeneratedChunks.GetChunk(new Point(x, y)) == null)
            //                newChunk = new Chunk(new Point(x, y), system.Generator, system);
            //            else
            //                newChunk = system.GeneratedChunks.GetChunk(new Point(x, y));

            //            chunksToMap.Add(newChunk);
            //            ix++;
            //        }
            //        iy++;
            //    }
            //    var startChunkLoc = chunksToMap.Get(0, 0).ID;

            //    //ix = 0;
            //    //iy = 0;
            //    //for (int y = (int)startChunkLoc.Y * 16; y <= ((int)startChunkLoc.Y + chunksToMap.Count(false)) * 16; y++)
            //    //{
            //    //    tilesToMap.AddLine();
            //    //    for (int x = (int)startChunkLoc.X * 16; x <= ((int)startChunkLoc.X + chunksToMap.Count(true)) * 16; x++)
            //    //    {
            //    //        Chunk chunk = chunksToMap.Get((int)Math.Floor(ix / 16d), (int)Math.Floor(iy / 16d));
            //    //        tilesToMap.Add(chunk.Tiles.Get(x - (int)(chunk.ID.X * 16), y - (int)(chunk.ID.Y * 16)));
            //    //        ix++;
            //    //    }
            //    //    iy++;
            //    //}

            //    for (int y = 0; y <= chunksToMap.Count(false) * 16; y++)
            //    {
            //        tilesToMap.AddLine();
            //        for (int x = 0; x <= chunksToMap.Count(true) * 16; x++)
            //        {
            //            var chunkLoc = new Point((int)Math.Floor(x / 16d), (int)Math.Floor(y / 16d));
            //            Chunk chunk = chunksToMap.Get(chunkLoc);
            //            tilesToMap.Add(chunk.Tiles.Get(x - ((int)chunkLoc.X * 16), y - ((int)chunkLoc.Y * 16)));
            //        }
            //    }
            //}

            //Pathfinder.UpdateWorld(tilesToMap);
            currentPath = Pathfinder.GetPath(Location, destination);
            isMoving = true;
            Debug.Log(string.Format("{0} Entity is moving from {1},{2} to {3},{4}", Type.Name, Location.X.ToString(), Location.Y.ToString(), destination.X.ToString(), destination.Y.ToString()));
        }
    }

    public static class EntityID
    {
        public static class NPC
        {
            public static readonly Prefab HUMAN = new Prefab(1000, "entity:human", false);
        }
    }
}
