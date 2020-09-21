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
        private int animRefresh = 150;
        public Point GoingTo;
        public bool isMoving = false;
        protected List<Tile> currentPath;
        protected int pathProgress = 0;

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
            if (animTimer >= animRefresh)
            {
                animTimer = 0;
                UpdateAnimation();
            }
            animTimer++;
            if (isMoving)
            {
                var newLoc = currentPath[pathProgress].Location;
                Location.X = Mathf.MoveTowards(Location.X, newLoc.X, Time.deltaTime);
                Location.Y = Mathf.MoveTowards(Location.Y, newLoc.Y, Time.deltaTime);
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
            var currentChunk = system.LoadedChunks.GetChunkWithPosition(Location);
            var destinationChunk = system.LoadedChunks.GetChunkWithPosition(destination);
            var lowestY = currentChunk.ID.Y < destinationChunk.ID.Y ? currentChunk : destinationChunk.ID.Y < currentChunk.ID.Y ? destinationChunk : currentChunk;
            var lowestX = currentChunk.ID.X < destinationChunk.ID.X ? currentChunk : destinationChunk.ID.X < currentChunk.ID.X ? destinationChunk : currentChunk;
            var highestY = lowestY == destinationChunk ? currentChunk : destinationChunk;
            var highestX = lowestX == destinationChunk ? currentChunk : destinationChunk;

            XYContainer<Chunk> chunksToMap = new List<List<Chunk>>();
            XYContainer<Tile> tilesToMap = new List<List<Tile>>();

            if (currentChunk == destinationChunk)
            {
                tilesToMap = currentChunk.Tiles;
            } else
            {
                int ix = 0;
                int iy = 0;
                for (int y = (int)lowestY.ID.Y; y <= highestY.ID.Y; y++)
                {
                    chunksToMap.AddLine();
                    for (int x = (int)lowestX.ID.X; x <= highestX.ID.X; x++)
                    {
                        Chunk newChunk;
                        if (system.GeneratedChunks.GetChunk(new Point(x, y)) == null)
                            newChunk = new Chunk(new Point(x, y), system.Generator, system);
                        else
                            newChunk = system.GeneratedChunks.GetChunk(new Point(x, y));

                        chunksToMap.Add(newChunk);
                        ix++;
                    }
                    iy++;
                }
                var startChunkLoc = chunksToMap.Get(0, 0).ID;

                ix = 0;
                iy = 0;
                for (int y = (int)startChunkLoc.Y * 16; y <= ((int)startChunkLoc.Y + chunksToMap.Count(false)) * 16; y++)
                {
                    tilesToMap.AddLine();
                    for (int x = (int)startChunkLoc.X * 16; x <= ((int)startChunkLoc.X + chunksToMap.Count(true)) * 16; x++)
                    {
                        Chunk chunk = chunksToMap.Get((int)Math.Floor(ix / 16d), (int)Math.Floor(iy / 16d));
                        tilesToMap.Add(chunk.Tiles.Get(x - (int)(chunk.ID.X * 16), y - (int)(chunk.ID.Y * 16)));
                        ix++;
                    }
                    iy++;
                }
            }

            Pathfinder.UpdateWorld(tilesToMap);
            currentPath = Pathfinder.GetPath(Location, destination);
            isMoving = true;
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
