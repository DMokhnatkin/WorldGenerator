using Map.MapModels.CompressedTable;
using Map.Generator.Algorithms.PerlinNoise;
using System;
using Map.MapModels.Common;
using System.Collections.Generic;
using System.Collections;

namespace Map.MapModels.Seismic
{
    public class SeismicMap
    {
        Perlin2D noise = new Perlin2D(new System.Random().Next());

        int octaves;
        float mountainFrequency;

        public SeismicMap(int octaves, float mountainFrequency)
        {
            this.octaves = octaves;
            this.mountainFrequency = mountainFrequency;
        }

        public float GetStrength(float x, float y)
        {
            return noise.Noise(x, y, octaves);
        }
    }
}
