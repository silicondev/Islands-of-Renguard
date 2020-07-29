using IslandsOfRenguard.Scripts.Universal;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace IslandsOfRenguard.Scripts.WorldGen
{
    //public class Generator
    //{
    //    private readonly GeneratorSettings _settings;
    //    private readonly WorldMapper _mapper;
    //    public Generator(GeneratorSettings genSettings, WorldMapperSettings mapSettings)
    //    {
    //        _settings = genSettings;
    //        _mapper = new WorldMapper(mapSettings);
    //    }

    //    public World Generate()
    //    {
    //        int height = (int)_settings.WorldSize.Y;
    //        int width = (int)_settings.WorldSize.X;
    //        Tile[,] tileMap = new Tile[width, height];

    //        for (int y = 0; y < height; y++)
    //        {
    //            for (int x = 0; x < width; x++)
    //            {
    //                float xPerlin = (_settings.Seed + x) * _settings.Scale;
    //                float yPerlin = (_settings.Seed + y) * _settings.Scale;
    //                float map = Mathf.PerlinNoise(xPerlin, yPerlin) * 255;
    //                ID id = _mapper.ParseHeight(map);
    //                tileMap[x, y] = new Tile(id, map);
    //            }
    //        }

    //        return new World(tileMap);
    //    }
    //}

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