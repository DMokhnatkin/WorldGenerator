using World.Model;
using World.Model.PointCollections;
using World.Generator.Algorithms.PerlinNoise;
using System.Collections;
using World.Model.Frames;
using UnityEngine;

namespace World.Generator
{
    public class WorldGenerator
    {
        const float BASE_HARSHNESS = 1f; // For better harshness representation
        public int octaves = 3;
        public const float eps = 0.01f;
        public float harshness = 500f;

        Perlin2D perlin2d;
        WorldModel model;

        public WorldGenerator(WorldModel model)
        {
            perlin2d = new Perlin2D(new System.Random().Next());
            this.model = model;
        }

        /// <summary>
        /// Generate single point data
        /// </summary>
        /// <param name="pt"></param>
        private void GeneratePoint(ModelPoint pt)
        {
            pt.Data.height = perlin2d.Noise(pt.NormalCoord.x / (harshness * BASE_HARSHNESS) + eps,
                pt.NormalCoord.y / (harshness * BASE_HARSHNESS) + eps, octaves);
        }

        /// <summary>
        /// Generate specifed world points
        /// </summary>
        public void Generate(BinPlus1SquareFrame frame)
        {
            foreach (ModelCoord z in frame.GetCoords())
            {
                Vector2 coord = model.CoordTransformer.ModelCoordToGlobal(z);
                model[z].Data.height = perlin2d.Noise(0.0001f * coord.x + eps, 0.0001f * coord.y + eps) * 0.8f + 
                    perlin2d.Noise(0.0005f * coord.x + eps, 0.0005f * coord.y + eps, 10) * 0.2f;
                //model[z].Data.height = Mathf.PerlinNoise(0.001f * coord.x + eps, 0.001f * coord.y + eps);
            }
        }
    }
}
