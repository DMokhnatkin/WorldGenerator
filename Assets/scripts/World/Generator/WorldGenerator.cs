using World.Model;
using World.Model.PointCollections;
using World.Generator.Algorithms.PerlinNoise;
using System.Collections;
using World.Model.Frames;
using UnityEngine;
using World.Instance;

namespace World.Generator
{
    [RequireComponent(typeof(WorldInstance))]
    public class WorldGenerator : MonoBehaviour
    {
        const float BASE_HARSHNESS = 1f; // For better harshness representation
        public const float eps = 0.01f;

        Perlin2D perlin2d;

        WorldInstance worldInstance;

        /// <summary>
        /// Cur generated frame
        /// </summary>
        public SquareFrame CurGenerateFrame { get; private set; }

        public WorldGeneratorSettings settings = new WorldGeneratorSettings();

        void Awake()
        {
            perlin2d = new Perlin2D(new System.Random().Next());
            worldInstance = GetComponent<WorldInstance>();
        }

        /// <summary>
        /// Generate single point data
        /// </summary>
        /// <param name="pt"></param>
        private void GeneratePoint(ModelCoord coord)
        {
            worldInstance.Model[coord].Data.Height = perlin2d.Noise(0.0001f * coord.x + eps, 0.0001f * coord.y + eps) * 0.8f +
                    perlin2d.Noise(0.0005f * coord.x + eps, 0.0005f * coord.y + eps, 10) * 0.2f;
            worldInstance.Model[coord].Data.heightGenerated = true;
        }

        /// <summary>
        /// Generate specifed world points
        /// </summary>
        public void Initialize()
        {
            CurGenerateFrame = new SquareFrame(
                new ModelCoord(-settings.generateRadius, -settings.generateRadius),
                settings.generateRadius * 2 + 1);
            foreach (ModelCoord z in CurGenerateFrame.GetCoords())
            {
                if (!worldInstance.Model.Contains(z))
                    worldInstance.Model.CreatePoint(z);
                GeneratePoint(z);
            }
        }

        /// <summary>
        /// Move frame top
        /// </summary>
        void MoveFrameTop(int dy)
        {
            for (int x = CurGenerateFrame.LeftDown.x; x < CurGenerateFrame.LeftDown.x + CurGenerateFrame.Size; x++)
            {
                for (int y = CurGenerateFrame.LeftDown.y + CurGenerateFrame.Size; y < CurGenerateFrame.LeftDown.y + CurGenerateFrame.Size + dy; y++)
                {
                    ModelCoord coord = new ModelCoord(x, y);
                    if (!worldInstance.Model.Contains(coord))
                        worldInstance.Model.CreatePoint(coord);
                    if (worldInstance.Model[coord].Data.heightGenerated)
                        continue;
                    GeneratePoint(coord);
                }
            }
            CurGenerateFrame = new SquareFrame(
                new ModelCoord(CurGenerateFrame.LeftDown.x, CurGenerateFrame.LeftDown.y + dy),
                CurGenerateFrame.Size);
        }

        /// <summary>
        /// Move frame down
        /// </summary>
        void MoveFrameDown(int dy)
        {
            for (int x = CurGenerateFrame.LeftDown.x; x < CurGenerateFrame.LeftDown.x + CurGenerateFrame.Size; x++)
            {
                for (int y = CurGenerateFrame.LeftDown.y - 1; y >= CurGenerateFrame.LeftDown.y - dy; y--)
                {
                    ModelCoord coord = new ModelCoord(x, y);
                    if (!worldInstance.Model.Contains(coord))
                        worldInstance.Model.CreatePoint(coord);
                    if (worldInstance.Model[coord].Data.heightGenerated)
                        continue;
                    GeneratePoint(coord);
                }
            }
            CurGenerateFrame = new SquareFrame(
                new ModelCoord(CurGenerateFrame.LeftDown.x, CurGenerateFrame.LeftDown.y - dy),
                CurGenerateFrame.Size);
        }

        /// <summary>
        /// Listen player moved in model
        /// </summary>
        public void PlayerMovedInModel(ModelCoord lastPos, ModelCoord newPos)
        {
            if (newPos.y - lastPos.y >= 1)
            {
                MoveFrameTop(newPos.y - lastPos.y);
            }
            if (lastPos.y - newPos.y >= 1)
            {
                MoveFrameDown(lastPos.y - newPos.y);
            }
        }
    }
}
