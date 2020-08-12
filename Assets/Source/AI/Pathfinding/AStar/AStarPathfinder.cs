using IslandsOfRenguard.AI.Pathfinding.Interfaces;
using IslandsOfRenguard.Base;
using IslandsOfRenguard.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Source.AI.Pathfinding.AStar
{
    public class AStarPathfinder : IPathfinder
    {
        private AStarTile _closest;
        private List<AStarTile> _openSet = new List<AStarTile>();
        private List<AStarTile> _closedSet = new List<AStarTile>();
        private List<AStarTile> _path = new List<AStarTile>();
        private AStarTile _start;
        private AStarTile _end;
        private int _cols => _tiles.Count();
        private int _rows => _tiles[0].Count();
        private List<List<Tile>> _tiles;
        private List<List<AStarTile>> _gridHold;
        private List<List<AStarTile>> _grid
        {
            get
            {
                if (_gridHold == null)
                {
                    List<List<AStarTile>> tmp = new List<List<AStarTile>>();

                    for (int y = 0; y < _rows; y++)
                    {
                        tmp.Add(new List<AStarTile>());
                        for (int x = 0; x < _cols; x++)
                        {
                            var tile = new AStarTile(_tiles[x][y]);
                            tmp[y].Add(tile);
                        }
                    }

                    _gridHold = tmp;
                }
                return _gridHold;
            }
        }

        public AStarPathfinder(List<List<Tile>> tiles)
        {
            _tiles = tiles;
        }

        public List<Tile> GetPath(Point start, Point end)
        {
            if (_tiles == null || _tiles[0] == null || _tiles[0][0] == null)
                return null;

            _openSet.Clear();
            _closedSet.Clear();

            for (int y = 0; y < _rows; y++)
            {
                for (int x = 0; x < _cols; x++)
                {
                    var tile = _grid[x][y];

                    var pnt = new Point(x, y);
                    if (pnt == start)
                        _start = tile;
                    else if (pnt == end)
                        _end = tile;
                }
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
                    return output;
                }

                for (int y = 0; y < _rows; y++)
                {
                    for (int x = 0; x < _cols; x++)
                    {
                        _grid[x][y].RefreshLocal(_grid);
                    }
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

        private int _difference(int a, int b)
        {
            if (a > b)
                return a - b;
            else if (b > a)
                return b - a;
            else
                return 0;
        }
    }
}
