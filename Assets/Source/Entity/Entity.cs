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
            if (currentPath != null && pathProgress == currentPath.Count)
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

            var chunks = system.LoadedChunks;
            List<Tile> tiles = new List<Tile>();
            List<Overlay> overlays = new List<Overlay>();

            foreach (var chunk in chunks)
            {
                var cTiles = chunk.Tiles;
                var cOverlays = chunk.Overlays;
                for (int y = 0; y < cTiles.Count(false); y++)
                {
                    for (int x = 0; x < cTiles.Count(true); x++)
                    {
                        tiles.Add(cTiles.Get(x, y));
                        overlays.Add(cOverlays.Get(x, y));
                    }
                }
            }

            Pathfinder.UpdateWorld(tiles, overlays);
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
