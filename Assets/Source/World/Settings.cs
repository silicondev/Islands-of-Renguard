using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandsOfRenguard.World
{
    public class WorldMapperSettings
    {
        public float ShallowWaterLevel { get; }
        public float SandLevel { get; }
        public float DeepWaterLevel { get; }
        public float MountainLevel { get; }

        public WorldMapperSettings(float sWater, float dWater, float sand, float mountain)
        {
            ShallowWaterLevel = sWater;
            DeepWaterLevel = dWater;
            MountainLevel = mountain;
            SandLevel = sand;
        }
    }
}
