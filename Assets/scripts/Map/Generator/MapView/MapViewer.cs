using System;
using System.Collections.Generic;
using UnityEngine;
using Map.Generator.MapModels;
using Map.Generator.World;

namespace Map.Generator.MapView
{
    public class MapViewer
    {
        Dictionary<Area, ChunkViewInfo> chunksInfo = new Dictionary<Area, ChunkViewInfo>();

        // Settings of viewer
        LandscapeSettings _settings;

        public MapViewer(LandscapeSettings settings)
        {
            _settings = settings;
        }

        private float[,] ToHeightMap(Area chunkArea)
        {
            MapVertex[,] map = chunkArea.ToArray();
            float[,] heights = new float[map.GetLength(0), map.GetLength(1)];
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    heights[heights.GetLength(0) - 1 - i, j] = map[i, j].Height;
            return heights;
        }

        public ChunkViewInfo GetViewInfo(Area area)
        {
            if (!chunksInfo.ContainsKey(area))
                return null;
            else
                return chunksInfo[area];
        }

        /// <summary>
        /// Render chunk which mapModel can be changed in future
        /// </summary>
        public void RenderDynamicChunk(Area area, Vector3 leftDownPos)
        {
            if (!chunksInfo.ContainsKey(area))
                chunksInfo.Add(area, new ChunkViewInfo());
            chunksInfo[area].Depth = area.CalcDepth();
            chunksInfo[area].LeftDownPos = leftDownPos;

            GameObject obj = new GameObject();
            MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
            Mesh mesh = meshFilter.mesh;
            mesh.Clear();

            List<Vector3> vert = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uv = new List<Vector2>();

            MapVertex[,] z = area.ToArray();
            float edgeLength = (int)_settings.chunkSize / ((float)z.GetLength(0) - 1);
            for (int i = 0; i < z.GetLength(0); i++)
                for (int j = 0; j < z.GetLength(1); j++)
                {
                    vert.Add(new Vector3(j * edgeLength, z[i, j].Height * _settings.height, (z.GetLength(0) - i - 1) * edgeLength));
                    uv.Add(new Vector2((z.GetLength(1) / (float)j) * (int)_settings.chunkSize, (z.GetLength(1) / (float)(z.GetLength(0) - i)) * (int)_settings.chunkSize));
                    if (j >= 1 && i < z.GetLength(0) - 1)
                    {
                        int curPt = i * z.GetLength(0) + j;
                        int curDownPt = (i + 1) * z.GetLength(0) + j;
                        int prevTopPt = i * z.GetLength(0) + (j - 1);
                        int prevDownPt = (i + 1) * z.GetLength(1) + (j - 1);
                        triangles.Add(curPt);
                        triangles.Add(prevDownPt);
                        triangles.Add(prevTopPt);

                        triangles.Add(curPt);
                        triangles.Add(curDownPt);
                        triangles.Add(prevDownPt);
                    }
                }

            mesh.vertices = vert.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uv.ToArray();

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.Optimize();

            obj.transform.position = leftDownPos;
            MeshRenderer render = obj.AddComponent<MeshRenderer>();
            render.material = _settings.baseMaterial;
            chunksInfo[area].ChunkObject = obj;
        }

        /// <summary>
        /// Render chunk which mapModel won't be changed in future
        /// </summary>
        public void RenderStaticChunk(Area area, Vector3 leftDownPos)
        {
            if (!chunksInfo.ContainsKey(area))
                chunksInfo.Add(area, new ChunkViewInfo());
            chunksInfo[area].Depth = area.CalcDepth();
            chunksInfo[area].LeftDownPos = leftDownPos;

            // Generate terrain gameObject
            GameObject terr = null;
            TerrainData tData = new TerrainData();
            float[,] h = ToHeightMap(area);
            tData.heightmapResolution = h.GetLength(0);
            tData.size = new Vector3((int)_settings.chunkSize, _settings.height, (int)_settings.chunkSize);
            tData.SetHeights(0, 0, h);

            SplatPrototype newSplat = new SplatPrototype();
            newSplat.texture = _settings.baseTexture;

            tData.splatPrototypes = new SplatPrototype[] { newSplat };

            terr = Terrain.CreateTerrainGameObject(tData);
            terr.transform.position = leftDownPos;

            chunksInfo[area].ChunkObject = terr;
        }
    }
}
