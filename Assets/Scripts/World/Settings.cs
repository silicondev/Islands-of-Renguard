using IslandsOfRenguard.Scripts.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandsOfRenguard.Scripts.WorldGen
{
    public class WorldMapperSettings
    {
        public float ShallowWaterLevel { get; }
        public float DeepWaterLevel { get; }
        public float MountainLevel { get; }

        public WorldMapperSettings(float sWater, float dWater, float mountain)
        {
            ShallowWaterLevel = sWater;
            DeepWaterLevel = dWater;
            MountainLevel = mountain;
        }
    }

    public class GeneratorSettings
    {
        public float Scale { get; } = 1.0F;
        public Point WorldSize { get; }
        public GeneratorSettings(int sizeX, int sizeY, float scale)
        {
            WorldSize = new Point(sizeX, sizeY);
            Scale = scale;
        }
    }
}
