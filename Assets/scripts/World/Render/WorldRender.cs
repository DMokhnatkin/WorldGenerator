using System;
using System.Collections.Generic;
using UnityEngine;
using World.Instance;
using World.Model;
using World.Common;
using World.DataStructures;
using World.DataStructures.ChunksGrid;
using System.Collections;

namespace World.Render
{
    /// <summary>
    /// This script renders world
    /// Square frame near player is rendered
    /// </summary>
    [RequireComponent(typeof(WorldInstance))]
    public partial class WorldRender : MonoBehaviour
    {
        /// <summary>
        /// World to render
        /// </summary>
        public WorldInstance World;

        public WorldModel WorldModel { get { return World.Model; } }

        /// <summary>
        /// Settings for render
        /// </summary>
        public RenderSettings settings = new RenderSettings();

        private Dictionary<Chunk, RenderedChunk> renderedChunks;

        /// <summary>
        /// GameObject which is parent for all chunk's GameObjects
        /// </summary>
        private GameObject parentObject;

        public const float CHUNKS_IMPOSITION = 0.1f;

        #region Rendered chunks
        private abstract class RenderedChunk
        {
            /// <summary>
            /// Associated game object
            /// </summary>
            public GameObject GameObject { get; protected set; }

            /// <summary>
            /// Associated chunk
            /// </summary>
            public Chunk Chunk { get; protected set; }

            /// <summary>
            /// Rendered detalizaton
            /// </summary>
            public int Detalization { get; protected set; }

            public RenderedChunk(GameObject gameObject, Chunk chunk, int detalization)
            {
                GameObject = gameObject;
                Chunk = chunk;
                Detalization = detalization;
            }

            public void Destroy()
            {
                GameObject.Destroy(GameObject);
            }
        }

        /// <summary>
        /// For this chunk was used terrain to render.
        /// </summary>
        private class TerrainRenderedChunk : RenderedChunk
        {
            /// <summary>
            /// Associated terrain component
            /// </summary>
            public Terrain TerrainComponent { get { return GameObject.GetComponent<Terrain>(); } }

            public TerrainRenderedChunk(GameObject gameObject, Chunk chunk, int detalization) : 
                base(gameObject, chunk, detalization)
            { }
        }

        /// <summary>
        /// For this chunk was used mesh to render.
        /// </summary>
        private class MeshRenderedChunk : RenderedChunk
        {
            /// <summary>
            /// Associated MeshRenderer component
            /// </summary>
            public MeshRenderer MeshRendererComponent { get { return GameObject.GetComponent<MeshRenderer>(); } }

            /// <summary>
            /// Associated MeshFilter component
            /// </summary>
            public MeshFilter MeshFilterComponent { get { return GameObject.GetComponent<MeshFilter>(); } }

            /// <summary>
            /// Associated LodGroup component 
            /// </summary>
            public LODGroup LodGroupComponent { get { return GameObject.GetComponent<LODGroup>(); } }

            public MeshRenderedChunk(GameObject gameObject, Chunk chunk, int detalization) : 
                base(gameObject, chunk, detalization)
            {}
        }
        #endregion

        /// <summary>
        /// Increase detalization
        /// </summary>
        private void IncreaseTerrainChunkDetalization(TerrainRenderedChunk renderedChunk, int newDetalization)
        {
            renderedChunks[renderedChunk.Chunk].Destroy();
            renderedChunks.Remove(renderedChunk.Chunk);
            RenderNewChunk(renderedChunk.Chunk, newDetalization);
        }

        /// <summary>
        /// Increase detalization for MeshRenderedChunk
        /// </summary>
        private void IncreaseMeshChunkDetalization(MeshRenderedChunk renderedChunk, int newDetalization)
        {
            if (World.Model.detalizationAccessor.GetSizeInLayer(renderedChunk.Chunk, newDetalization) >= 33)
            {
                // Change this rendered chunk to TerrainRenderedChunk
                renderedChunks[renderedChunk.Chunk].Destroy();
                renderedChunks.Remove(renderedChunk.Chunk);
                RenderNewChunk(renderedChunk.Chunk, newDetalization);
            }
        }

        /// <summary>
        /// Increase detalization of RenderedChunk
        /// </summary>
        private void IncreaseChunkDetalization(RenderedChunk renderedChunk, int newDetalization)
        {
            if (renderedChunk is TerrainRenderedChunk)
            {
                IncreaseTerrainChunkDetalization(renderedChunk as TerrainRenderedChunk, newDetalization);
                return;
            }
            if (renderedChunk is MeshRenderedChunk)
            {
                IncreaseMeshChunkDetalization(renderedChunk as MeshRenderedChunk, newDetalization);
                return;
            }
        }

        /// <summary>
        /// Render new chunk using terrain.
        /// </summary>
        private void RenderNewTerrainChunk(Chunk chunk, int detalization)
        {
            if (WorldModel.detalizationAccessor.GetSizeInLayer(chunk, detalization) < 33)
            {
                throw new ArgumentException("Can't render chunk using terrain. Size in current detalization is less then 33");
            }
            TerrainRenderedChunk res;
            TerrainData data = new TerrainData();
            float chunkSize = WorldModel.CoordTransformer.ModelDistToGlobal(chunk.Size - 1);
            // Terrain heights
            data.heightmapResolution = WorldModel.detalizationAccessor.GetSizeInLayer(chunk, detalization);
            float[,] heighmap = new float[data.heightmapWidth, data.heightmapHeight];
            for (int x = 0; x < data.heightmapHeight; x++)
                for (int y = 0; y < data.heightmapWidth; y++)
                {
                    heighmap[y, x] = World.Model.detalizationAccessor.GetData<float>(new IntCoord(x, y), 
                        chunk, 
                        World.Model.heighmap, 
                        detalization);
                }
            data.SetHeightsDelayLOD(0, 0, heighmap);

            // Water
            /*
            GameObject water = new GameObject("test", typeof(MeshFilter), typeof(MeshRenderer));
            var waterFilter = water.GetComponent<MeshFilter>(); 
            waterFilter.mesh = new Mesh();

            int sizeInLayer = WorldModel.detalizationAccessor.GetSizeInLayer(chunk, detalization);

            
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            foreach (IntCoord baseCoord in World.Model.detalizationAccessor.GetBaseCoordsInLayer(chunk, detalization))
            {
                float height = World.Model.detalizationAccessor.GetData<float>(baseCoord, World.Model.heighmap) + World.Model.riverMap[baseCoord].waterLevel;
                Vector2 pos = World.Model.CoordTransformer.ModelCoordToGlobal(baseCoord);
                vertices.Add(new Vector3(pos.x - chunkSize / 2.0f, height * World.settings.height, pos.y - chunkSize / 2.0f));
                int v0 = vertices.Count - 1;
                int v1 = vertices.Count - 2;
                int v2 = vertices.Count - 1 - sizeInLayer;
                int v3 = vertices.Count - 2 - sizeInLayer;
                if (v3 >= 0)
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
            waterFilter.mesh.vertices = vertices.ToArray();
            waterFilter.mesh.triangles = triangles.ToArray();
            waterFilter.mesh.Optimize();*/


            // Terrain textures
            data.alphamapResolution = data.heightmapResolution;
            data.SetDetailResolution(data.heightmapResolution * 2, 8);
            data.splatPrototypes = new SplatPrototype[2] { new SplatPrototype() { texture = settings.baseTexture, tileSize = settings.tileSize }, new SplatPrototype() { texture = settings.baseTexture1 } };
            float[,,] alphamap = new float[data.alphamapWidth, data.alphamapHeight, data.splatPrototypes.Length];
            for (int x = 0; x < data.alphamapWidth; x++)
                for (int y = 0; y < data.alphamapHeight; y++)
                {
                    IntCoord cur = new IntCoord(x, y);
                    alphamap[x, y, 0] = 1;
                }
            data.SetAlphamaps(0, 0, alphamap);

            // Create TerrainRenderChunk and add it to renderedChunks
            data.size = new Vector3(chunkSize, World.settings.height, chunkSize);
            res = new TerrainRenderedChunk(Terrain.CreateTerrainGameObject(data), chunk, detalization);
            Vector2 chunkPos = WorldModel.CoordTransformer.ModelCoordToGlobal(chunk.leftDown);
            res.GameObject.transform.position = new Vector3(chunkPos.x - chunkSize / 2.0f, 0, chunkPos.y - chunkSize / 2.0f);
            res.GameObject.name = "Chunk " + chunk.chunkCoord.ToString();
            res.GameObject.transform.SetParent(parentObject.transform);
            renderedChunks.Add(chunk, res);
            res.TerrainComponent.ApplyDelayedHeightmapModification();
            return;
        }

        /// <summary>
        /// Render new chunk using mesh.
        /// </summary>
        private void RenderNewMeshChunk(Chunk chunk, int detalization)
        {
            MeshRenderedChunk res = new MeshRenderedChunk(
                new GameObject("Chunk " + chunk.chunkCoord.ToString(), 
                    typeof(MeshFilter), typeof(MeshRenderer)),
                chunk, 
                detalization);
            res.GameObject.transform.SetParent(parentObject.transform);
            res.GameObject.isStatic = true;

            float chunkSize = WorldModel.CoordTransformer.ModelDistToGlobal(chunk.Size - 1);
            res.MeshFilterComponent.mesh = new Mesh();
            int sizeInLayer = WorldModel.detalizationAccessor.GetSizeInLayer(chunk, detalization);

            // Generate mesh and uv map
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uv = new List<Vector2>();
            foreach (IntCoord baseCoord in World.Model.detalizationAccessor.GetBaseCoordsInLayer(chunk, detalization))
            {
                float data = World.Model.detalizationAccessor.GetData<float>(baseCoord, World.Model.heighmap);
                Vector2 pos = World.Model.CoordTransformer.ModelCoordToGlobal(baseCoord);
                vertices.Add(new Vector3(pos.x - chunkSize / 2.0f, data * World.settings.height, pos.y - chunkSize / 2.0f));
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

            // Apply textures
            res.MeshFilterComponent.mesh.uv = uv.ToArray();
            res.MeshRendererComponent.material.mainTexture = settings.baseTexture;
            res.MeshRendererComponent.material.mainTextureScale = new Vector2(chunkSize / settings.tileSize.x , chunkSize / settings.tileSize.y);
            res.MeshRendererComponent.material.SetFloat("_Glossiness", 0);

            renderedChunks.Add(chunk, res);
        }

        /// <summary>
        /// Render new chunk. (chunk shouldn't be rendered before)
        /// </summary>
        private void RenderNewChunk(Chunk chunk, int detalization)
        {
            if (renderedChunks.ContainsKey(chunk))
                throw new ArgumentException("Chunk was rendered before");
            if (detalization < 0 ||
                detalization >= World.Model.detalizationAccessor.detalizationLayersCount)
                throw new ArgumentException("Forbidden detalization(" + detalization + ")");
            // Can we use unity terrain?
            // Min terrain heighmap resolution = 33
            if (World.Model.detalizationAccessor.GetSizeInLayer(chunk, detalization) >= 33)
            {
                RenderNewTerrainChunk(chunk, detalization);
                SetNeighbors(chunk, true);
            }
            if (World.Model.detalizationAccessor.GetSizeInLayer(chunk, detalization) < 33)
            {
                RenderNewMeshChunk(chunk, detalization);
            }
        }

        /// <summary>
        /// Create chunks for curRender frame
        /// </summary>
        public void Initialize()
        {
            if (parentObject == null)
            {
                parentObject = new GameObject("rendered");
                parentObject.transform.SetParent(this.transform);
            }
            renderedChunks = new Dictionary<Chunk, RenderedChunk>();
            foreach(ChunkDetalization z in World.settings.detalization.GetDetalizations(World.CurChunk.chunkCoord))
            {
                Chunk chunk = World.Model.chunksNavigator.GetChunk(z.chunkCoord);
                if (!renderedChunks.ContainsKey(chunk))
                {
                    RenderNewChunk(chunk, z.detalization);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="rec">If true for each neighbor will be setted neighbors too</param>
        private void SetNeighbors(Chunk chunk, bool rec)
        {
            if (!renderedChunks.ContainsKey(chunk))
                return;
            if (!(renderedChunks[chunk] is TerrainRenderedChunk))
                return;
            Chunk top = World.Model.chunksNavigator.TopNeighbor(chunk);
            Chunk right = World.Model.chunksNavigator.RightNeighbor(chunk);
            Chunk down = World.Model.chunksNavigator.DownNeighbor(chunk);
            Chunk left = World.Model.chunksNavigator.LeftNeighbor(chunk);

            Terrain topTerrain = null;
            Terrain rightTerrain = null;
            Terrain downTerrain = null;
            Terrain leftTerrain = null;
            if (renderedChunks.ContainsKey(top) &&
                renderedChunks[top] is TerrainRenderedChunk &&
                renderedChunks[top].Detalization == renderedChunks[chunk].Detalization)
            {
                topTerrain = (renderedChunks[top] as TerrainRenderedChunk).TerrainComponent;
            }
            if (renderedChunks.ContainsKey(right) &&
                renderedChunks[right] is TerrainRenderedChunk &&
                renderedChunks[right].Detalization == renderedChunks[chunk].Detalization)
            {
                rightTerrain = (renderedChunks[right] as TerrainRenderedChunk).TerrainComponent;
            }
            if (renderedChunks.ContainsKey(down) &&
                renderedChunks[down] is TerrainRenderedChunk &&
                renderedChunks[down].Detalization == renderedChunks[chunk].Detalization)
            {
                downTerrain = (renderedChunks[down] as TerrainRenderedChunk).TerrainComponent;
            }
            if (renderedChunks.ContainsKey(left) &&
                renderedChunks[left] is TerrainRenderedChunk &&
                renderedChunks[left].Detalization == renderedChunks[chunk].Detalization)
            {
                leftTerrain = (renderedChunks[left] as TerrainRenderedChunk).TerrainComponent;
            }

            (renderedChunks[chunk] as TerrainRenderedChunk).TerrainComponent.SetNeighbors(leftTerrain, topTerrain, rightTerrain, downTerrain);
            (renderedChunks[chunk] as TerrainRenderedChunk).TerrainComponent.Flush();
            if (rec)
            {
                SetNeighbors(top, false);
                SetNeighbors(right, false);
                SetNeighbors(down, false);
                SetNeighbors(left, false);
            }
        }

        /// <summary>
        /// Update detalization of RenderedChunk.
        /// If not exists, it will be created
        /// </summary>
        public void UpdateRenderedChunk(Chunk chunk, int newDetalization)
        {
            RenderedChunk t;
            renderedChunks.TryGetValue(World.Model.chunksNavigator.GetChunk(chunk.chunkCoord), out t);
            if (t != null)
            {
                // Update existing rendered chunk
                IncreaseChunkDetalization(t, newDetalization);
            }
            else
            {
                // Create new rendered chunk
                RenderNewChunk(chunk, newDetalization);
            }
        }
    }
}
