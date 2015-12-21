using World.Model;
using World.Model.PointCollections;
using World.Generator.Algorithms.PerlinNoise;
using System.Collections;

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
        public void Generate(IWorldPointCollection toGenerate)
        {
            foreach (ModelPoint z in toGenerate)
            {
                GeneratePoint(z);
            }
        }
    }
}
