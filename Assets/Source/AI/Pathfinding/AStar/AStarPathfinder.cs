using dEvine_and_conquer.AI.Pathfinding.Interfaces;
using dEvine_and_conquer.Base;
using dEvine_and_conquer.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace dEvine_and_conquer.AI.Pathfinding.AStar
{
    public class AStarPathfinder : IPathfinder
    {
        private AStarTile _closest;
        private List<AStarTile> _openSet = new List<AStarTile>();
        private List<AStarTile> _closedSet = new List<AStarTile>();
        private List<AStarTile> _fullSet
        {
            get
            {
                List<AStarTile> full = new List<AStarTile>();
                full.AddRange(_closedSet);
                full.AddRange(_openSet);
                return full;
            }
        }
        private List<Point> _path = new List<Point>();
        private AStarTile _start;
        private AStarTile _end;
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
            //Timer.Start("Pathfinding");
            var watchFull = Stopwatch.StartNew();

            _path.Clear();

            if (_grid == null) ReloadGrid();

            if (_blocks == null)
                return _path;

            _openSet.Clear();
            _closedSet.Clear();

            _start = _grid.Get(start);
            _end = _grid.Get(end);

            _openSet.Add(_start);

            float loopTotal = 0;
            int iter = 0;
            while (_openSet.Count > 0)
            {
                var watchLoop = Stopwatch.StartNew();
                //Timer.Start("Pathfinding.Loop");
                int low = 0;

                for (int i = 0; i < _openSet.Count; i++)
                {
                    if (_openSet[i].h < _openSet[low].h)
                        low = i;
                }

                _closest = _openSet[low];

                foreach (var tile in _openSet)
                {
                    tile.RefreshLocal(_grid);
                }

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

                foreach (var local in _closest.Local)
                {
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
                    }
                    else
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

                        //_closest = local;
                    }
                }

                if (Timer.Current("Pathfinding") >= 10f)
                {
                    //Timer.StopAndLog("Pathfinding");
                    Debug.Log("Pathfinding stopped due to hanging.");
                    break;
                }

                //loopTotal += Timer.Stop("Pathfinding.Loop");
                loopTotal += watchLoop.StopAndReturn();
                watchLoop.Stop();
                iter++;
            }

            Debug.Log($"Pathfinding loop took {(loopTotal / iter).ToString("F4")} seconds on average to complete");
            Debug.Log($"Pathfinding went through {iter} iterations");

            Debug.Log($"Pathfinding should have taken around {loopTotal.ToString("F4")} seconds to complete and took {watchFull.StopAndReturn()} to complete.");
            //watchFull.StopAndLog("Pathfinding");
            //Timer.StopAndLog("Pathfinding");
            return _path;
        }
    }
}
