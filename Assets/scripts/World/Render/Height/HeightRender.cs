using System;
using System.Collections.Generic;
using UnityEngine;
using World.DataStructures;
using World.DataStructures.ChunksGrid;
using World.Instance;
using World.Model;
using World.Render.RenderedChunks;

namespace World.Render.Height
{
    /// <summary>
    /// Class implements mesh(or terrain) heights generation using model
    /// </summary>
    public class HeightRender
    {
        public readonly HeightRenderSettings settings;

        public readonly WorldInstance worldInstance;

        public HeightRender(HeightRenderSettings settings, WorldInstance worldInstance)
        {
            this.settings = settings;
            this.worldInstance = worldInstance;
        }

        /// <summary>
        /// Render new chunk using terrain.
        /// </summary>
        /// <returns></returns>
        public TerrainRenderedChunk RenderTerrainChunk(Chunk chunk, int detalization, bool renderWater)
        {
            if (worldInstance.Model.detalizationAccessor.GetSizeInLayer(chunk, detalization) < 33)
            {
                throw new ArgumentException("Can't render chunk using terrain. Size in current detalization is less then 33");
            }
            TerrainRenderedChunk res;
            TerrainData data = new TerrainData();
            float chunkSize = worldInstance.Model.CoordTransformer.ModelDistToGlobal(chunk.Size - 1);
            // Terrain heights
            data.heightmapResolution = worldInstance.Model.detalizationAccessor.GetSizeInLayer(chunk, detalization);
            float[,] heighmap = new float[data.heightmapWidth, data.heightmapHeight];
            for (int x = 0; x < data.heightmapHeight; x++)
                for (int y = 0; y < data.heightmapWidth; y++)
                {
                    heighmap[y, x] = worldInstance.Model.detalizationAccessor.GetData<float>(new IntCoord(x, y),
                        chunk,
                        worldInstance.Model.heighmap,
                        detalization);
                }
            data.SetHeightsDelayLOD(0, 0, heighmap);

            // Create TerrainRenderChunk and add it to renderedChunks
            data.size = new Vector3(chunkSize, worldInstance.settings.height, chunkSize);
            res = new TerrainRenderedChunk(Terrain.CreateTerrainGameObject(data), chunk, detalization);
            Vector2 chunkPos = worldInstance.Model.CoordTransformer.ModelCoordToGlobal(chunk.leftDown);
            res.GameObject.transform.position = new Vector3(chunkPos.x - chunkSize / 2.0f, 0, chunkPos.y - chunkSize / 2.0f);
            res.GameObject.name = "Chunk " + chunk.chunkCoord.ToString();
            res.GameObject.transform.SetParent(worldInstance.transform);
            res.TerrainComponent.ApplyDelayedHeightmapModification();
            return res;
        }

        /// <summary>
        /// Render new chunk using mesh.
        /// </summary>
        public MeshRenderedChunk RenderMeshChunk(Chunk chunk, int detalization)
        {
            MeshRenderedChunk res = new MeshRenderedChunk(
                new GameObject("Chunk " + chunk.chunkCoord.ToString(),
                    typeof(MeshFilter), typeof(MeshRenderer)),
                chunk,
                detalization);
            res.GameObject.transform.SetParent(worldInstance.transform);
            res.GameObject.isStatic = true;

            float chunkSize = worldInstance.Model.CoordTransformer.ModelDistToGlobal(chunk.Size - 1);
            res.MeshFilterComponent.mesh = new Mesh();
            int sizeInLayer = worldInstance.Model.detalizationAccessor.GetSizeInLayer(chunk, detalization);

            // Generate mesh and uv map
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uv = new List<Vector2>();
            foreach (IntCoord baseCoord in worldInstance.Model.detalizationAccessor.GetBaseCoordsInLayer(chunk, detalization))
            {
                float data = worldInstance.Model.detalizationAccessor.GetData<float>(baseCoord, worldInstance.Model.heighmap);
                Vector2 pos = worldInstance.Model.CoordTransformer.ModelCoordToGlobal(baseCoord);
                vertices.Add(new Vector3(pos.x - chunkSize / 2.0f, data * worldInstance.settings.height, pos.y - chunkSize / 2.0f));
                uv.Add(new Vector2((baseCoord.x - chunk.LeftBorder) / (float)chunk.Size, (baseCoord.y - chunk.LeftBorder) / (float)chunk.Size));
                int v0 = vertices.Count - 1;
                int v1 = vertices.Count - 2;
                int v2 = vertices.Count - 1 - sizeInLayer;
                int v3 = vertices.Count - 2 - sizeInLayer;
                if (baseCoord.x != chunk.LeftBorder && baseCoord.y != chunk.DownBorder)
                {
                    triangles.Add(v1);
                    triangles.Add(v0);
                    triangles.Add(v3);

                    triangles.Add(v0);
                    triangles.Add(v2);
                    triangles.Add(v3);
                }
            }

            // Apply generated mesh
            res.MeshFilterComponent.mesh.vertices = vertices.ToArray();
            res.MeshFilterComponent.mesh.triangles = triangles.ToArray();
            res.MeshFilterComponent.mesh.RecalculateNormals();
            res.MeshFilterComponent.mesh.Optimize();

            // Apply uv
            res.MeshFilterComponent.mesh.uv = uv.ToArray();

            return res;
        }
    }
}
