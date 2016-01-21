using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using World.Model.Chunks;
using World.Model;

namespace World.Render.Chunks
{
    public class TerrainRenderChunk
    {
        public GameObject ChunkObject { get; private set; }

        public ModelChunk Chunk { get; private set; }

        public Terrain TerrainComponent { get { return ChunkObject.GetComponent<Terrain>(); } }

        public TerrainRenderChunk(ModelChunk chunk, int detalization, RenderSettings settings)
        {
            Chunk = chunk;
            TerrainData data = new TerrainData();
            float chunkSize = chunk.Model.CoordTransformer.ModelDistToGlobal(chunk.Size);
            data.size = new Vector3(chunkSize, settings.worldHeight, chunkSize);
            data.heightmapResolution = chunk.GetSizeInLayer(detalization);
            float[,] heighmap = new float[data.heightmapWidth, data.heightmapHeight];
            for (int x = 0; x < data.heightmapWidth; x++)
                for (int y = 0; y < data.heightmapHeight; y++)
                {
                    ModelPoint pt = chunk.GetPointInLayer(new ModelCoord(x, y), detalization);
                    if (pt == null)
                        heighmap[y, x] = 0;
                    else
                        heighmap[y, x] = pt.Data.Height;
                }
            data.SetHeights(0, 0, heighmap);

            data.splatPrototypes = new SplatPrototype[1] { new SplatPrototype() { texture = settings.baseTexture } };
            float[,,] alphamap = new float[data.alphamapWidth, data.alphamapHeight, 1];
            for (int x = 0; x < data.alphamapWidth; x++)
                for (int y = 0; y < data.alphamapHeight; y++)
                {
                    alphamap[x, y, 0] = 1;
                }
            data.SetAlphamaps(0, 0, alphamap);

            ChunkObject = Terrain.CreateTerrainGameObject(data);

            Vector2 chunkPos = chunk.Model.CoordTransformer.ModelCoordToGlobal(chunk.Frame.LeftDown);
            ChunkObject.transform.position = new Vector3(chunkPos.x, 0, chunkPos.y);
        }
    }
}
