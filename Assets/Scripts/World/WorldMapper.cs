using IslandsOfRenguard.Scripts.Universal;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace IslandsOfRenguard.Scripts.WorldGen
{
    public class WorldMapper
    {
        private readonly WorldMapperSettings _settings;
        public WorldMapper(WorldMapperSettings settings)
        {
            _settings = settings;
        }

        public ID ParseHeight(float height)
        {
            if (height > _settings.MountainLevel)
                return TileID.ENV.STONE;
            else if (height < _settings.ShallowWaterLevel)
                return TileID.ENV.WATER;
            else
                return TileID.ENV.GRASS;
        }
    }
}