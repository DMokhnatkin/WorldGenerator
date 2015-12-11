using System;
using System.Collections.Generic;
using UnityEngine;
using Map.MapModels;
using Map.MapModels.Areas;
using Map.World;
using Map.MapView.Textures;
using Map.MapModels.Extensions;
using Map.MapModels.Points;

namespace Map.MapView
{
    [RequireComponent(typeof(MapTextureSettings))]
    [RequireComponent(typeof(LandscapeSettings))]
    public class MapViewer : MonoBehaviour
    {
        Dictionary<Area, ChunkViewInfo> chunksInfo = new Dictionary<Area, ChunkViewInfo>();

        // Settings of viewer
        LandscapeSettings _settings;
        MapTextureSettings _textureSettings;

        void Awake()
        {
            _settings = GetComponent<LandscapeSettings>();
            _textureSettings = GetComponent<MapTextureSettings>();
        }

        private float[,] ToHeightMap(Area chunkArea)
        {
            MapPoint[,] map = chunkArea.UnwrapPoints();
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
            GameObject obj = null;
            MeshFilter meshFilter;
            MeshRenderer render;

            byte depth = area.CalcDepth();

            if (chunksInfo.ContainsKey(area))
            {
                if (chunksInfo[area].IsStatic)
                {
                    // This chunk was rendered as static before
                    // We can't change models of static chunks, so just render it
                    RenderStaticChunk(area, leftDownPos);
                    return;
                }
                else
                {
                    obj = chunksInfo[area].ChunkObject;
                    if (chunksInfo[area].Depth >= depth)
                    {
                        // Chunk was rendered before, just show it
                        if (!obj.activeSelf)
                            obj.SetActive(true);
                    }
                }
            }

            if (!chunksInfo.ContainsKey(area))
                chunksInfo.Add(area, new ChunkViewInfo());
            chunksInfo[area].Depth = depth;
            chunksInfo[area].LeftDownPos = leftDownPos;

            if (obj == null)
            {
                obj = new GameObject();
                obj.isStatic = true;
                meshFilter = obj.AddComponent<MeshFilter>();
                render = obj.AddComponent<MeshRenderer>();
                render.receiveShadows = false;
                render.useLightProbes = false;
                render.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            }
            else
            {
                meshFilter = obj.GetComponent<MeshFilter>();
                render = obj.GetComponent<MeshRenderer>();
            }
            Mesh mesh = meshFilter.mesh;
            mesh.Clear();
            //mesh.MarkDynamic();

            List<Vector3> vert = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uv = new List<Vector2>();

            MapPoint[,] z = area.UnwrapPoints();
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
            
            render.material = _settings.baseMaterial;
            chunksInfo[area].ChunkObject = obj;
        }

        /// <summary>
        /// Render chunk which mapModel won't be changed in future
        /// </summary>
        public void RenderStaticChunk(Area area, Vector3 leftDownPos)
        {
            if (chunksInfo.ContainsKey(area))
            {
                if (chunksInfo[area].IsStatic)
                {
                    // This chunk was rendered before, so it is in memory
                    chunksInfo[area].ChunkObject.SetActive(true);
                    return;
                }
                else
                {
                    GameObject.Destroy(chunksInfo[area].ChunkObject);
                }
            }
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

            TerrainTextureGenerator.GenerateTerrainTexture(area.UnwrapPoints(), tData, _textureSettings);

            terr = Terrain.CreateTerrainGameObject(tData);
            terr.transform.position = leftDownPos;

            chunksInfo[area].ChunkObject = terr;
            chunksInfo[area].IsStatic = true;
        }
    }
}
