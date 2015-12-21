using World.Model;
using World.Model.PointCollections;
using World.Generator.Algorithms.PerlinNoise;

namespace World.Generator
{
    public class WorldGenerator
    {
        public int octaves = 3;
        public const float eps = 0.01f;

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
        private void GeneratePoint(WorldPoint pt)
        {
            pt.Data.height = perlin2d.Noise(model.GetCoordTransformer(1.0f).ModelCoordToGlobal(pt.NormalCoord).x + eps,
                model.GetCoordTransformer(1.0f).ModelCoordToGlobal(pt.NormalCoord).y + eps, octaves);
        }

        /// <summary>
        /// Generate specifed world points
        /// </summary>
        public void Generate(IWorldPointCollection toGenerate)
        {
            foreach (WorldPoint z in toGenerate)
            {
                GeneratePoint(z);
            }
        }
    }
}
