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
        private ID[,] _tileMap;
        public World(ID[,] tileMap)
        {
            _tileMap = tileMap;
        }
    }
}
