using System;
using System.Collections.Generic;
using UnityEngine;
using World.Instance;
using World.Model;
using World.DataStructures;
using World.DataStructures.ChunksGrid;
using System.Collections;
using World.Render.Texture;
using World.Render.RenderedChunks;
using World.Render.Height;
using World.Render.Water;

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
        public WorldInstance worldInstance;

        public WorldModel WorldModel { get { return worldInstance.Model; } }

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

        #region Render parts
        HeightRender render;
        TextureRender textureRender;
        WaterRender waterRender;

        public HeightRenderSettings renderSettings = new HeightRenderSettings();
        public TextureRenderSettings textureRenderSettings = new TextureRenderSettings();
        public WaterRenderSettings waterRenderSettings = new WaterRenderSettings();
        #endregion

        public WorldRender()
        {
            textureRender = new TextureRender(textureRenderSettings);
        }

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
            if (worldInstance.Model.detalizationAccessor.GetSizeInLayer(renderedChunk.Chunk, newDetalization) >= 33)
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
        /// Render new chunk. (chunk shouldn't be rendered before)
        /// </summary>
        private void RenderNewChunk(Chunk chunk, int detalization)
        {
            if (renderedChunks.ContainsKey(chunk))
                throw new ArgumentException("Chunk was rendered before");
            if (detalization < 0 ||
                detalization >= worldInstance.Model.detalizationAccessor.detalizationLayersCount)
                throw new ArgumentException("Forbidden detalization(" + detalization + ")");
            // Can we use unity terrain?
            // Min terrain heighmap resolution = 33
            if (worldInstance.Model.detalizationAccessor.GetSizeInLayer(chunk, detalization) >= 33)
            {
                TerrainRenderedChunk res = render.RenderTerrainChunk(chunk, detalization, true);
                textureRender.ApplyTexture(res, worldInstance.Model);
                waterRender.RenderWater(res);
                renderedChunks.Add(chunk, res);
                SetNeighbors(chunk, true);
            }
            if (worldInstance.Model.detalizationAccessor.GetSizeInLayer(chunk, detalization) < 33)
            {
                MeshRenderedChunk res = render.RenderMeshChunk(chunk, detalization);
                textureRender.ApplyTexture(res, worldInstance.Model);
                renderedChunks.Add(chunk, res);
            }
        }

        /// <summary>
        /// Create chunks for curRender frame
        /// </summary>
        public void Initialize()
        {
            if (render == null)
            {
                if (worldInstance == null)
                    Debug.LogError("Set worldInstance for worldRender");
                else
                {
                    render = new HeightRender(renderSettings, worldInstance);
                    waterRender = new WaterRender(waterRenderSettings, worldInstance);
                }
            }
            if (parentObject == null)
            {
                parentObject = new GameObject("rendered");
                parentObject.transform.SetParent(this.transform);
            }
            renderedChunks = new Dictionary<Chunk, RenderedChunk>();
            foreach(ChunkDetalization z in worldInstance.settings.detalization.GetDetalizations(worldInstance.CurChunk.chunkCoord))
            {
                Chunk chunk = worldInstance.Model.chunksNavigator.GetChunk(z.chunkCoord);
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
            Chunk top = worldInstance.Model.chunksNavigator.TopNeighbor(chunk);
            Chunk right = worldInstance.Model.chunksNavigator.RightNeighbor(chunk);
            Chunk down = worldInstance.Model.chunksNavigator.DownNeighbor(chunk);
            Chunk left = worldInstance.Model.chunksNavigator.LeftNeighbor(chunk);

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
            renderedChunks.TryGetValue(worldInstance.Model.chunksNavigator.GetChunk(chunk.chunkCoord), out t);
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
