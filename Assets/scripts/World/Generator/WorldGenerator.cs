using World.Model;
using World.Generator.Algorithms.PerlinNoise;
using System.Collections;
using UnityEngine;
using World.Instance;
using System.Collections.Generic;
using World.DataStructures.ChunksGrid;
using World.DataStructures;

namespace World.Generator
{
    [RequireComponent(typeof(WorldInstance))]
    public class WorldGenerator : MonoBehaviour
    {
        const float BASE_HARSHNESS = 1f; // For better harshness representation
        public const float eps = 0.01f;

        Perlin2D perlin2d_1;
        Perlin2D perlin2d_2;
        Perlin2D perlin2d_3;

        Vector2 seed1;
        Vector2 seed2;
        Vector2 seed3;
        Vector2 seed4;

        WorldInstance worldInstance;

        /// <summary>
        /// Current generated detalization for chunks
        /// </summary>
        Dictionary<Chunk, int> generatedChunks = new Dictionary<Chunk, int>();

        public WorldGeneratorSettings settings = new WorldGeneratorSettings();

        void Awake()
        {
            perlin2d_1 = new Perlin2D(new System.Random().Next());
            perlin2d_2 = new Perlin2D(new System.Random().Next());
            perlin2d_3 = new Perlin2D(new System.Random().Next());
            seed1 = new Vector2(UnityEngine.Random.value * 1000, UnityEngine.Random.value * 1000);
            seed2 = new Vector2(UnityEngine.Random.value * 1000, UnityEngine.Random.value * 1000);
            seed3 = new Vector2(UnityEngine.Random.value * 1000, UnityEngine.Random.value * 1000);
            seed4 = new Vector2(UnityEngine.Random.value * 1000, UnityEngine.Random.value * 1000);
            worldInstance = GetComponent<WorldInstance>();
        }

        /// <summary>
        /// Generate single point data
        /// </summary>
        /// <param name="pt"></param>
        private void GeneratePoint(IntCoord baseCoord)
        {
            Vector2 pos = worldInstance.Model.CoordTransformer.ModelCoordToGlobal(baseCoord);
            worldInstance.Model.heighmap[baseCoord] = Mathf.PerlinNoise(seed1.x + 0.0001f * pos.x + eps, seed1.y + 0.0001f * pos.y + eps) *
                Mathf.PerlinNoise(seed1.x + 0.001f * pos.x + eps, seed1.y + 0.001f * pos.y + eps) * 0.7f;
            //    Mathf.PerlinNoise(seed2.x + 0.008f * pos.x + eps, seed2.y + 0.008f * pos.y + eps) * 0.25f +
            //    Mathf.PerlinNoise(seed3.x + 0.01f * pos.x + eps, seed3.y + 0.01f * pos.y + eps) * 0.125f +
            //    Mathf.PerlinNoise(seed4.x + 0.02f * pos.x + eps, seed4.y + 0.02f * pos.y + eps) * 0.125f;
            //worldInstance.Model[coord].Data.Height = perlin2d.Noise(0.0001f * coord.x + eps, 0.0001f * coord.y + eps) * 0.8f +
            //        perlin2d.Noise(0.0005f * coord.x + eps, 0.0005f * coord.y + eps, 10) * 0.2f;
        }

        private void GenerateChunk(Chunk chunk, int detalization)
        {
            worldInstance.Model.heighmap.Initialize(chunk);
            foreach (IntCoord z in worldInstance.Model.detalizationAccessor.GetBaseCoordsInLayer(chunk, detalization))
                GeneratePoint(z);
        }

        /// <summary>
        /// Generate specifed world points
        /// </summary>
        public void Initialize()
        {
            foreach (ChunkDetalization z in worldInstance.settings.detalization.GetDetalizations(worldInstance.CurChunkCoord))
            {
                GenerateChunk(worldInstance.Model.chunksNavigator.GetChunk(z.chunkCoord), z.detalization);
            }
        }
    }
}
