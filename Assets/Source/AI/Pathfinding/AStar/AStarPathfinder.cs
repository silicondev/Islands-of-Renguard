using dEvine_and_conquer.AI.Pathfinding.Interfaces;
using dEvine_and_conquer.Base;
using dEvine_and_conquer.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace dEvine_and_conquer.AI.Pathfinding.AStar
{
    public class AStarPathfinder : IPathfinder
    {
        private AStarTile _closest;
        private List<AStarTile> _openSet = new List<AStarTile>();
        private List<AStarTile> _closedSet = new List<AStarTile>();
        private List<Point> _path = new List<Point>();
        private AStarTile _start;
        private AStarTile _end;
        private float _maxScopeVal = 100;
        private int _scopeDiv = 2;
        private List<Block> _blocks;
        private List<AStarTile> _gridHold;
        private List<AStarTile> _grid
        {
            get
            {
                if (_gridHold == null) ReloadGrid();
                return _gridHold;
            }
        }

        public AStarPathfinder(List<Block> blocks)
        {
            _blocks = blocks;
        }

        public AStarPathfinder()
        {

        }

        private void ReloadGrid()
        {
            List<AStarTile> tmp = new List<AStarTile>();

            foreach (var block in _blocks)
            {
                tmp.Add(new AStarTile(block));
            }

            _gridHold = tmp;
        }

        public void UpdateWorld(List<Block> blocks)
        {
            _blocks = blocks;
            _gridHold = null;
        }
        public List<Point> GetPath(Point start, Point end)
        {
            _path.Clear();

            if (_grid == null) ReloadGrid();

            if (_blocks == null)
                return _path;

            _openSet.Clear();
            _closedSet.Clear();

            _start = _grid.Where(x => x.Location == start).First();
            _end = _grid.Where(x => x.Location == end).First();

            List<AStarTile> scope = new List<AStarTile>();

            var seh = PlaneFunctions.Heuristic(_start.Location, _end.Location);
            var seScope = Mathf.Clamp(seh / _scopeDiv, 0, _maxScopeVal);

            foreach (var tile in _grid)
            {
                var s = PlaneFunctions.Heuristic(tile.Location, _start.Location);
                var e = PlaneFunctions.Heuristic(tile.Location, _end.Location);
                var h = s + e;
                if (h <= seh + seScope) scope.Add(tile);
            }

            _openSet.Add(_start);

            while (_openSet.Count > 0)
            {
                int low = 0;

                for (int i = 0; i < _openSet.Count; i++)
                {
                    if (_openSet[i].f < _openSet[low].f)
                        low = i;
                }

                _closest = _openSet[low];

                _openSet.Remove(_closest);
                _closedSet.Add(_closest);

                // PATH COMPLETE
                if (_closest == _end)
                {
                    AStarTile tmp = _closest;
                    _path.Add(tmp.Location);
                    while (tmp.Prev != null)
                    {
                        _path.Add(tmp.Prev.Location);
                        tmp = tmp.Prev;
                    }
                    _path.Reverse();
                    break;
                }

                foreach (var tile in scope)
                {
                    tile.RefreshLocal(scope);
                }

                for (int i = 0; i < _closest.Local.Count(); i++)
                {
                    AStarTile local = _closest.Local[i];

                    if (_closedSet.Contains(local))
                        continue;

                    float g = _closest.g + PlaneFunctions.Heuristic(local.Location, _closest.Location);

                    bool newPath = false;
                    if (_openSet.Contains(local))
                    {
                        if (g < local.g)
                        {
                            local.g = g;
                            newPath = true;
                        }
                    } else
                    {
                        local.g = g;
                        newPath = true;
                        _openSet.Add(local);
                    }

                    if (newPath)
                    {
                        local.h = PlaneFunctions.Heuristic(local.Location, _end.Location);
                        local.f = local.g + local.h;
                        local.Prev = _closest;
                    }
                }
            }

            return _path;
        }
    }
}
