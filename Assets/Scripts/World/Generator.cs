using IslandsOfRenguard.Scripts.Universal;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace IslandsOfRenguard.Scripts.WorldGen
{
    public class Generator
    {
        private GeneratorSettings _settings;
        private WorldMapper _mapper;
        public Generator(GeneratorSettings genSettings, WorldMapperSettings mapSettings)
        {
            _settings = genSettings;
            _mapper = new WorldMapper(mapSettings);
        }

        public World Generate()
        {
            int seed = Random.Range(0, 1000);
            int height = (int)_settings.WorldSize.Y;
            int width = (int)_settings.WorldSize.X;
            ID[,] tileMap = new ID[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float xPerlin = ((seed + x) / (width / 10)) * _settings.Scale;
                    float yPerlin = ((seed + y) / (height / 10)) * _settings.Scale;
                    float map = Mathf.PerlinNoise(xPerlin, yPerlin) * 255;
                    tileMap[x, y] = _mapper.ParseHeight(map);
                }
            }

            return new World(tileMap);
        }
    }

    public class WorldMapper
    {
        private WorldMapperSettings _settings;
        public WorldMapper(WorldMapperSettings settings)
        {
            _settings = settings;
        }

        public ID ParseHeight(float height)
        {
            if (height > _settings.MountainLevel)
                return Tile.ENV.STONE;
            else if (height < _settings.ShallowWaterLevel)
                return Tile.ENV.WATER;
            else
                return Tile.ENV.GRASS;
        }
    }
}