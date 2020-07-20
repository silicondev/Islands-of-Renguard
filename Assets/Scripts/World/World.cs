using IslandsOfRenguard.Scripts.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandsOfRenguard.Scripts.WorldGen
{
    public class World
    {
        private Tile[,] _tileMap;
        public World(Tile[,] tileMap)
        {
            _tileMap = tileMap;
        }

        public Tile GetTile(int x, int y)
        {
            return _tileMap[x, y];
        }

        public Point GetSize()
        {
            return new Point(_tileMap.GetLength(0), _tileMap.GetLength(1));
        }
    }
}
