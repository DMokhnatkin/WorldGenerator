using System;
using System.Collections.Generic;
using UnityEngine;
using World.DataStructures;
using World.DataStructures.ChunksGrid;
using World.Instance;
using World.Render.RenderedChunks;

namespace World.Render.Water
{
    /// <summary>
    /// Render water
    /// </summary>
    public class WaterRender
    {
        public readonly WaterRenderSettings settings;

        public readonly WorldInstance worldInstance;

        public WaterRender(WaterRenderSettings settings, WorldInstance worldInstance)
        {
            this.settings = settings;
            this.worldInstance = worldInstance;
        }

        private void AddPointToGroup(IntCoord cur, Dictionary<IntCoord, int> group, int groupId)
        {
            if (group.ContainsKey(cur) || !worldInstance.Model.riverMap.riverData.Contains(cur))
                return;
            group.Add(cur, groupId);
            AddPointToGroup(cur.Top, group, groupId);
            AddPointToGroup(cur.Right, group, groupId);
            AddPointToGroup(cur.Down, group, groupId);
            AddPointToGroup(cur.Left, group, groupId);
        }

        /// <summary>
        /// Render new chunk using terrain.
        /// </summary>
        /// <returns></returns>
        public void RenderWater(TerrainRenderedChunk renderedChunk)
        {

            float chunkSize = worldInstance.Model.CoordTransformer.ModelDistToGlobal(renderedChunk.Chunk.Size - 1);
            int sizeInLayer = worldInstance.Model.detalizationAccessor.GetSizeInLayer(renderedChunk.Chunk, renderedChunk.Detalization);

            // Divide water points into groups
            int grouptId = 0;
            Dictionary<IntCoord, int> groups = new Dictionary<IntCoord, int>();
            foreach (IntCoord baseCoord in worldInstance.Model.detalizationAccessor.GetBaseCoordsInLayer(renderedChunk.Chunk, renderedChunk.Detalization))
            {
                int ctBefore = groups.Count;
                AddPointToGroup(baseCoord, groups, grouptId);
                if (ctBefore != groups.Count)
                    grouptId++;
            }

            // Generate new mesh and uv
            // For each group we will store vertices, triangles, uv, and 
            // Dictionary <IntCoord, int> (to find relation between point IntCoord and index in vertices array)
            List<Vector3>[] vertices = new List<Vector3>[grouptId];
            List<int>[] triangles = new List<int>[grouptId];
            List<Vector2>[] uv = new List<Vector2>[grouptId];
            Dictionary<IntCoord, int>[] vertexId = new Dictionary<IntCoord, int>[grouptId];
            for (int i = 0; i < grouptId; i++)
            {
                vertices[i] = new List<Vector3>();
                triangles[i] = new List<int>();
                uv[i] = new List<Vector2>();
                vertexId[i] = new Dictionary<IntCoord, int>();
            }
            foreach (IntCoord baseCoord in worldInstance.Model.detalizationAccessor.GetBaseCoordsInLayer(renderedChunk.Chunk, renderedChunk.Detalization))
            {
                if (!worldInstance.Model.riverMap.riverData.Contains(baseCoord))
                    continue;
                float height = worldInstance.Model.detalizationAccessor.GetData<float>(baseCoord, worldInstance.Model.heighmap) + worldInstance.Model.riverMap.riverData[baseCoord].waterAmount;
                Vector2 pos = worldInstance.Model.CoordTransformer.ModelCoordToGlobal(baseCoord);
                int group = groups[baseCoord];
                vertices[group].Add(new Vector3(pos.x - chunkSize / 2.0f, height * worldInstance.settings.height, pos.y - chunkSize / 2.0f));
                vertexId[group].Add(baseCoord, vertices[groups[baseCoord]].Count - 1);
                uv[group].Add(new Vector2((baseCoord.x - renderedChunk.Chunk.LeftBorder) / (float)renderedChunk.Chunk.Size, (baseCoord.y - renderedChunk.Chunk.LeftBorder) / (float)renderedChunk.Chunk.Size));
                if (vertexId[group].ContainsKey(baseCoord.Left) && vertexId[group].ContainsKey(baseCoord.LeftDown))
                {
                    triangles[group].Add(vertexId[group][baseCoord]);
                    triangles[group].Add(vertexId[group][baseCoord.LeftDown]);
                    triangles[group].Add(vertexId[group][baseCoord.Left]);
                }
                if (vertexId[group].ContainsKey(baseCoord.Down) && vertexId[group].ContainsKey(baseCoord.LeftDown))
                {
                    triangles[group].Add(vertexId[group][baseCoord]);
                    triangles[group].Add(vertexId[group][baseCoord.Down]);
                    triangles[group].Add(vertexId[group][baseCoord.LeftDown]);
                }
            }

            // Apply generated group meshes
            for (int i = 0; i < grouptId; i++)
            {
                GameObject water = (GameObject)GameObject.Instantiate(settings.waterPrefab, new Vector3(), new Quaternion());
                water.transform.localScale = new Vector3(1, 1, 1);
                water.transform.position = new Vector3(0, -worldInstance.WorldGenerator.riverSettings.waterAmountEps * worldInstance.settings.height, 0);
                var waterFilter = water.GetComponent<MeshFilter>();
                waterFilter.mesh = new Mesh();

                waterFilter.mesh.vertices = vertices[i].ToArray();
                waterFilter.mesh.triangles = triangles[i].ToArray();
                waterFilter.mesh.RecalculateNormals();
                waterFilter.mesh.Optimize();

                // Apply uv map
                waterFilter.mesh.uv = uv[i].ToArray();
            }
        }
    }
}
