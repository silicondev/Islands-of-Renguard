using IslandsOfRenguard.Scripts.Universal;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace IslandsOfRenguard.Scripts.WorldGen
{
    public class Generator
    {
        private readonly GeneratorSettings _settings;
        private readonly WorldMapper _mapper;
        public Generator(GeneratorSettings genSettings, WorldMapperSettings mapSettings)
        {
            _settings = genSettings;
            _mapper = new WorldMapper(mapSettings);
        }

        public World Generate()
        {
            int seed = Random.Range(0, 1000000000);
            float fseed = Random.Range(0.0F, 10000.0F);
            Debug.Log(string.Format("Seed: {0}", fseed.ToString()));
            int height = (int)_settings.WorldSize.Y;
            int width = (int)_settings.WorldSize.X;
            Debug.Log(string.Format("Perlin Start Location: {0}", (fseed * _settings.Scale).ToString()));
            Tile[,] tileMap = new Tile[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //float xPerlin = ((seed + x) / (width / 10)) * _settings.Scale;
                    //float yPerlin = ((seed + y) / (height / 10)) * _settings.Scale;
                    float xPerlin = (fseed + x) * _settings.Scale;
                    float yPerlin = (fseed + y) * _settings.Scale;
                    float map = Mathf.PerlinNoise(xPerlin, yPerlin) * 255;
                    ID id = _mapper.ParseHeight(map);
                    //Debug.Log(id.ToString());
                    tileMap[x, y] = new Tile(id, map);
                }
            }

            return new World(tileMap);
        }
    }

    public class WorldMapper
    {
        private readonly WorldMapperSettings _settings;
        public WorldMapper(WorldMapperSettings settings)
        {
            _settings = settings;
        }

        public ID ParseHeight(float height)
        {
            //Debug.Log(string.Format("{0} < {1} < {2}", _settings.ShallowWaterLevel.ToString(), height.ToString(), _settings.MountainLevel.ToString()));
            if (height > _settings.MountainLevel)
                return TileID.ENV.STONE;
            else if (height < _settings.ShallowWaterLevel)
                return TileID.ENV.WATER;
            else
                return TileID.ENV.GRASS;
        }
    }
}