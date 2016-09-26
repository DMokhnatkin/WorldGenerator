using World.Model;
using World.Generator.Algorithms.PerlinNoise;
using System.Collections;
using UnityEngine;
using World.Instance;
using System.Collections.Generic;
using World.DataStructures.ChunksGrid;
using World.DataStructures;
using World.Generator.Algorithms.Erosion;
using World.Generator.Algorithms.WaterFlow;
using World.Generator.Algorithms.River;

namespace World.Generator
{
    [RequireComponent(typeof(WorldInstance))]
    public class WorldGenerator : MonoBehaviour
    {
        const float BASE_HARSHNESS = 1f; // For better harshness representation
        public const float eps = 0.01f;

        Perlin2D perlin2d_0;
        Perlin2D perlin2d_1;
        Perlin2D perlin2d_2;
        Perlin2D perlin2d_3;
        Erosion erosion;
        RiverMapBuilder riverBuilder;

        public ErosionSettings erosionSettings = new ErosionSettings();
        WaterFlowCalc waterFlow;
        public WaterFlowSettings waterFlowSettings = new WaterFlowSettings();
        public RiverSettings riverSettings = new RiverSettings();

        WorldInstance worldInstance;

        /// <summary>
        /// Current generated detalization for chunks
        /// </summary>
        Dictionary<Chunk, int> generatedChunks = new Dictionary<Chunk, int>();

        public WorldGeneratorSettings settings = new WorldGeneratorSettings();

        void Awake()
        {
            perlin2d_0 = new Perlin2D(new System.Random().Next());
            perlin2d_1 = new Perlin2D(new System.Random().Next());
            perlin2d_2 = new Perlin2D(new System.Random().Next());
            perlin2d_3 = new Perlin2D(new System.Random().Next());
            erosion = new Erosion(erosionSettings);
            waterFlow = new WaterFlowCalc(waterFlowSettings);
            riverBuilder = new RiverMapBuilder(riverSettings, this);
            worldInstance = GetComponent<WorldInstance>();
        }

        /// <summary>
        /// Generate single point data
        /// </summary>
        /// <param name="pt"></param>
        public void GeneratePoint(IntCoord baseCoord)
        {
            Vector2 pos = worldInstance.Model.CoordTransformer.ModelCoordToGlobal(baseCoord);
            // Mountain map
            if (!worldInstance.Model.mountainMap.Contains(baseCoord))
                worldInstance.Model.mountainMap.Initialize(baseCoord);
            worldInstance.Model.mountainMap[baseCoord] = perlin2d_0.Noise(0.0001f * pos.x + eps, 0.0001f * pos.y + eps);
            // Height map
            if (!worldInstance.Model.heighmap.Contains(baseCoord))
                worldInstance.Model.heighmap.Initialize(baseCoord);
            // 1
            
            worldInstance.Model.heighmap[baseCoord] =
                worldInstance.Model.mountainMap[baseCoord] *
                (perlin2d_2.Noise(0.001f * pos.x + eps, 0.001f * pos.y + eps) * 0.7f +
                (perlin2d_3.Noise(0.003f * pos.x + eps, 0.003f * pos.y + eps) - 0.5f) * 0.3f);
            // 2
            /*
            worldInstance.Model.heighmap[baseCoord] = Mathf.Pow(
                (perlin2d_2.Noise(0.0005f * pos.x + eps, 0.0005f * pos.y + eps) * 0.7f +
                (perlin2d_3.Noise(0.0007f * pos.x + eps, 0.0007f * pos.y + eps) - 0.5f)* 0.3f), 1 -  worldInstance.Model.mountainMap[baseCoord]);*/
        }

        public void GenerateChunk(Chunk chunk, int detalization)
        {
            foreach (IntCoord z in worldInstance.Model.detalizationAccessor.GetBaseCoordsInLayer(chunk, detalization, 1))
                GeneratePoint(z);

            if (detalization == 6)
            {
                riverBuilder.BuildRiverMap(chunk, worldInstance.Model.heighmap, worldInstance.Model.riverMap);
                riverBuilder.AffectChunk(chunk, worldInstance.Model.heighmap, worldInstance.Model.riverMap);
                //waterFlow.CalcWaterFlow(chunk, worldInstance.Model.heighmap, worldInstance.Model.waterFlowMap, worldInstance.Model.CoordTransformer.ModelUnitWidth, 10);
                //erosion.CalcChunkErosion(chunk, worldInstance.Model, 10);
            }
        }

        /// <summary>
        /// Generate specifed world points
        /// </summary>
        public void Initialize()
        {
            foreach (ChunkDetalization z in worldInstance.settings.detalization.GetDetalizations(worldInstance.CurChunk.chunkCoord))
            {
                GenerateChunk(worldInstance.Model.chunksNavigator.GetChunk(z.chunkCoord), z.detalization);
            }
        }
    }
}
