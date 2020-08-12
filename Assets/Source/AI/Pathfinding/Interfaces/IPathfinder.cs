using IslandsOfRenguard.Base;
using IslandsOfRenguard.World;
using System.Collections;
using System.Collections.Generic;

namespace IslandsOfRenguard.AI.Pathfinding.Interfaces
{
    public interface IPathfinder
    {
        List<Tile> GetPath(Point start, Point end);
    }
}