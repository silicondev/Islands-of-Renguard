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
    public abstract class GenericEntity : VisualObject, IUpdateable
    {
        public Inventory Inventory { get; }
        public List<Sprite> Textures { get; protected set; }
        public Prefab Type { get; set; }
        public GameObject Object { get; set; }
        public EntityManager Instance { get; set; }
        protected IPathfinder Pathfinder = new AStarPathfinder();
        private int _animRefresh = 150;
        public bool isMoving = false;
        protected List<Point> currentPath;
        protected int pathProgress = 0;
        private float _moveSpeed = 5.0F;
        private Point _movingFrom;
        private Point _movingTo;
        private bool _movingToSuccess = false;

        public GenericEntity(Point loc, Prefab type, int invSlots, List<Sprite> textures = null) : base(loc)
        {
            Inventory = new Inventory(invSlots);
            if (textures == null)
                Textures = new List<Sprite>();
            else
                Textures = textures;
            Type = type;
        }

        public GenericEntity(float x, float y, Prefab type, int invSlots, List<Sprite> textures = null) : base(x, y)
        {
            Inventory = new Inventory(invSlots);
            if (textures == null)
                Textures = new List<Sprite>();
            else
                Textures = textures;
            Type = type;
        }

        int animTimer = 0;
        public void Update()
        {
            UpdateSpecific();
            if (animTimer >= _animRefresh)
            {
                animTimer = 0;
                UpdateAnimation();
            }
            animTimer++;
            if (isMoving)
            {
                var newLoc = currentPath[pathProgress];
                Location.X = Mathf.MoveTowards(Location.X, newLoc.X, Time.deltaTime * _moveSpeed);
                Location.Y = Mathf.MoveTowards(Location.Y, newLoc.Y, Time.deltaTime * _moveSpeed);
                if (Location == newLoc) pathProgress++;

                if (pathProgress == currentPath.Count)
                {
                    isMoving = false;
                    pathProgress = 0;
                    OnDestinationReach?.Invoke(this, new TargetReachArgs(_movingFrom, _movingTo, _movingToSuccess));
                }
            }
        }

        protected abstract void UpdateSpecific();

        int currentFrame = 0;
        private void UpdateAnimation()
        {
            Instance.Renderer.sprite = Textures[currentFrame];
            currentFrame++;
            if (currentFrame >= Textures.Count) currentFrame = 0;
        }

        public async Task GoTo(Point destination)
        {
            if (destination == Location) return;

            _movingFrom = Location.Copy();
            _movingTo = destination.Copy();

            var chunks = GameSystem.Chunks.GetLoaded();

            List<Block> blocks = new List<Block>();
            foreach (var chunk in chunks)
            {
                blocks.AddRange(chunk.Blocks);
            }

            Pathfinder.UpdateWorld(blocks);
            currentPath = await Task.Run(() => Pathfinder.GetPath(Location, destination));
            if (currentPath != null && currentPath.Any())
            {
                isMoving = true;
                _movingToSuccess = currentPath.Last() == destination;
                DevLogger.Log($"{Type.Name} Entity is moving from {Location} to {destination}");
            }
        }

        public event EventHandler OnDestinationReach;
    }
}
