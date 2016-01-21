using System;
using System.Collections.Generic;
using UnityEngine;
using World.Instance;
using World.Model;
using World.Model.Frames;
using World.Common;
using World.Render.Chunks;
using World.Model.Chunks;

namespace World.Render
{
    /// <summary>
    /// This script renders world
    /// Square frame near player is rendered
    /// </summary>
    [RequireComponent(typeof(WorldInstance))]
    public class WorldRender : MonoBehaviour
    {
        /// <summary>
        /// World to render
        /// </summary>
        public WorldInstance World;

        /// <summary>
        /// Settings for render
        /// </summary>
        public RenderSettings settings = new RenderSettings();

        public Dictionary<ModelChunk, TerrainRenderChunk> renderedChunks;

        /// <summary>
        /// Listen player moved from chunk
        /// </summary>
        public void PlayerChunkCoordChanged(ModelCoord prevChunkCoord, ModelCoord newChunkCoord)
        {
            renderedChunks.Clear();
            foreach (ChunkDetalization z in settings.detalization.GetDetalizations(World.CurChunkCoord))
            {
                ModelChunk chunk = World.Model.ChunksGrid.GetChunk(z.chunkCoord);
                if (!renderedChunks.ContainsKey(chunk))
                {
                    renderedChunks.Add(chunk, new TerrainRenderChunk(chunk, z.detalization, settings));
                    SetNeighbors(chunk, true);
                }
            }
        }

        /// <summary>
        /// Create chunks for curRender frame
        /// </summary>
        public void Initialize()
        {
            renderedChunks = new Dictionary<ModelChunk, TerrainRenderChunk>();
            foreach(ChunkDetalization z in settings.detalization.GetDetalizations(World.CurChunkCoord))
            {
                ModelChunk chunk = World.Model.ChunksGrid.GetChunk(z.chunkCoord);
                if (!renderedChunks.ContainsKey(chunk))
                {
                    renderedChunks.Add(chunk, new TerrainRenderChunk(chunk, z.detalization, settings));
                    SetNeighbors(chunk, true);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="rec">If true for each neighbor will be setted neighbors too</param>
        private void SetNeighbors(ModelChunk chunk, bool rec)
        {
            if (!renderedChunks.ContainsKey(chunk))
                return;
            ModelChunk top = ChunksNavigator.TopNeighbor(chunk);
            ModelChunk right = ChunksNavigator.RightNeighbor(chunk);
            ModelChunk down = ChunksNavigator.DownNeighbor(chunk);
            ModelChunk left = ChunksNavigator.LeftNeighbor(chunk);

            Terrain topTerrain = renderedChunks.ContainsKey(top) ? renderedChunks[top].TerrainComponent : null;
            Terrain rightTerrain = renderedChunks.ContainsKey(right) ? renderedChunks[right].TerrainComponent : null;
            Terrain downTerrain = renderedChunks.ContainsKey(down) ? renderedChunks[down].TerrainComponent : null;
            Terrain leftTerrain = renderedChunks.ContainsKey(left) ? renderedChunks[left].TerrainComponent : null;

            renderedChunks[chunk].TerrainComponent.SetNeighbors(leftTerrain, topTerrain, rightTerrain, downTerrain);
            if (rec)
            {
                SetNeighbors(top, false);
                SetNeighbors(right, false);
                SetNeighbors(down, false);
                SetNeighbors(left, false);
            }
        }

    }
}
