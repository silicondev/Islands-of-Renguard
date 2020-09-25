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
        private List<AStarTile> _path = new List<AStarTile>();
        private AStarTile _start;
        private AStarTile _end;
        private float _maxScopeVal = 100;
        private int _scopeDiv = 2;
        private List<Tile> _tiles;
        private List<AStarTile> _gridHold;
        private List<AStarTile> _grid
        {
            get
            {
                if (_gridHold == null) ReloadGrid();
                return _gridHold;
            }
        }
        public AStarPathfinder(List<Tile> tiles)
        {
            _tiles = tiles;
        }

        public AStarPathfinder()
        {

        }

        private void ReloadGrid()
        {
            List<AStarTile> tmp = new List<AStarTile>();

            foreach (var tile in _tiles)
            {
                tmp.Add(new AStarTile(tile));
            }

            _gridHold = tmp;
        }

        public void UpdateWorld(List<Tile> tiles)
        {
            _tiles = tiles;
            _gridHold = null;
        }

        public List<Tile> GetPath(Point start, Point end)
        {
            if (_grid == null) ReloadGrid();

            if (_tiles == null)
                return null;

            _openSet.Clear();
            _closedSet.Clear();

            _start = _grid.Where(x => x.Location == start).First();
            _end = _grid.Where(x => x.Location == end).First();

            List<AStarTile> scope = new List<AStarTile>();

            var seh = _heuristic(_start, _end);
            var seScope = Mathf.Clamp(seh / _scopeDiv, 0, _maxScopeVal);

            foreach (var tile in _grid)
            {
                var s = _heuristic(tile, _start);
                var e = _heuristic(tile, _end);
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

                if (_closest == _end)
                {
                    _path.Clear();
                    AStarTile tmp = _closest;
                    _path.Add(tmp);
                    while (tmp.Prev != null)
                    {
                        _path.Add(tmp.Prev);
                        tmp = tmp.Prev;
                    }
                    List<Tile> output = new List<Tile>();
                    foreach (var i in _path)
                    {
                        output.Add(i.Tile);
                    }
                    output.Reverse();
                    return output;
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

                    float g = _closest.g + _heuristic(local, _closest);

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
                        local.h = _heuristic(local, _end);
                        local.f = local.g + local.h;
                        local.Prev = _closest;
                    }
                }
            }

            return null;
        }

        private float _heuristic(AStarTile a, AStarTile b)
        {
            int ax = (int)a.Tile.Location.X;
            int ay = (int)a.Tile.Location.Y;
            int bx = (int)b.Tile.Location.X;
            int by = (int)b.Tile.Location.Y;

            double x = Math.Pow(_difference(ax, bx), 2);
            double y = Math.Pow(_difference(ay, by), 2);
            double z = Math.Sqrt(x + y);
            return (float)z;
        }

        private int _difference(int a, int b) => a > b ? a - b : b > a ? b - a : 0;
    }
}
