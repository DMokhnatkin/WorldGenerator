using System;
using System.Collections.Generic;
using UnityEngine;
using World.DataStructures;
using World.DataStructures.ChunksGrid;
using World.Model;
using World.Render.RenderedChunks;

namespace World.Render.Texture
{
    public class TextureRender
    {
        public readonly TextureRenderSettings settings;

        public TextureRender(TextureRenderSettings settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Increase visiblity for one layer and proportionaly decrease for others
        /// </summary>
        private void IncreseTextureVisiblity(float[,,] alphamap, IntCoord coord, int layer, float delta)
        {
            for (int i = 0; i < alphamap.GetLength(3); i++)
            {
                if (i == layer)
                    alphamap[coord.x, coord.y, i] += delta;
                else
                    alphamap[coord.x, coord.y, i] -= delta / (alphamap.GetLength(3) - 1);
            }
        }

        /// <summary>
        /// Apply texture for TerrainRenderedChunk
        /// </summary>
        public void ApplyTexture(TerrainRenderedChunk renderedChunk, WorldModel worldModel)
        {
            renderedChunk.TerrainComponent.terrainData.alphamapResolution = renderedChunk.TerrainComponent.terrainData.heightmapResolution;
            renderedChunk.TerrainComponent.terrainData.SetDetailResolution(renderedChunk.TerrainComponent.terrainData.heightmapResolution * 2, 8);
            renderedChunk.TerrainComponent.terrainData.splatPrototypes = new SplatPrototype[]
            {
                new SplatPrototype() { texture = settings.baseTexture, normalMap = settings.baseNormal, tileSize = settings.baseTile },
                new SplatPrototype() { texture = settings.waterMoodTexure, normalMap = settings.waterMoodNormal, tileSize = settings.waterMoodTile }
            };
            float[,,] alphaMap = renderedChunk.TerrainComponent.terrainData.GetAlphamaps(0, 0, renderedChunk.TerrainComponent.terrainData.alphamapWidth, renderedChunk.TerrainComponent.terrainData.heightmapHeight);
            for (int x = 0; x < renderedChunk.TerrainComponent.terrainData.heightmapHeight; x++)
                for (int y = 0; y < renderedChunk.TerrainComponent.terrainData.heightmapWidth; y++)
                {
                    alphaMap[y, x, 0] = 1;
                    /*
                    alphaMap[y, x] = worldModel.detalizationAccessor.GetData<float>(new IntCoord(x, y),
                        chunk,
                        World.Model.heighmap,
                        detalization);*/
                }
            renderedChunk.TerrainComponent.terrainData.SetAlphamaps(0, 0, alphaMap);
        }

        /// <summary>
        /// Apply texture for MeshRenderedChunk
        /// </summary>
        public void ApplyTexture(MeshRenderedChunk renderedChunk, WorldModel worldModel)
        {
            float realChunkSize = worldModel.CoordTransformer.ModelDistToGlobal(renderedChunk.Chunk.Size);
            renderedChunk.MeshRendererComponent.material.mainTexture = settings.baseTexture;
            renderedChunk.MeshRendererComponent.material.mainTextureScale = new Vector2(realChunkSize / settings.baseTile.x, realChunkSize / settings.baseTile.y);
            //renderedChunk.MeshRendererComponent.material.SetTexture("_DetailNormalMap", settings.baseNormal);
            renderedChunk.MeshRendererComponent.material.SetFloat("_Glossiness", 0);
        }
    }
}
