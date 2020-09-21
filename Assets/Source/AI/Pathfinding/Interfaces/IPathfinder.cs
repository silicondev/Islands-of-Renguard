using dEvine_and_conquer.Base;
using dEvine_and_conquer.World;
using System.Collections;
using System.Collections.Generic;

namespace dEvine_and_conquer.AI.Pathfinding.Interfaces
{
    public interface IPathfinder
    {
        List<Tile> GetPath(Point start, Point end);
        void UpdateWorld(XYContainer<Tile> tiles);
    }
}