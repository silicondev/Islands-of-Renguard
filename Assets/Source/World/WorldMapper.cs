using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace dEvine_and_conquer.World
{
    public class WorldMapper
    {
        private readonly WorldMapperSettings _settings;
        public WorldMapper(WorldMapperSettings settings)
        {
            _settings = settings;
        }

        public Prefab ParseHeight(float height)
        {
            if (height > _settings.MountainLevel)
                return TileID.ENV.STONE;
            else if (height > _settings.ShallowWaterLevel && height <= _settings.SandLevel)
                return TileID.ENV.SAND;
            else if (height < _settings.ShallowWaterLevel)
                return TileID.ENV.WATER;
            else
                return TileID.ENV.GRASS;
        }
    }
}