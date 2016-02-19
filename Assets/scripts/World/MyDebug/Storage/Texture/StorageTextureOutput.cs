using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using World.Model;
using World.DataStructures;
using World.DataStructures.ChunksGrid;
using World.Instance;
using System.IO;

namespace World.MyDebug.Storage.Texture
{
    /// <summary>
    /// Render texures using values from storage
    /// </summary>
    public class StorageTextureOutput : MonoBehaviour
    {
        public WorldInstance world;
        public float minVal = 0;
        public float maxVal = 1;
        public ValueGetter valueGetter;
        public string mapName = "";

        public void BuildTexture(IntCoord coord, DetalizationRadius rad)
        {
            foreach (ChunkDetalization z in rad.GetDetalizations(coord))
            {
                if (z.detalization == 6)
                    BuildTexture(world.Model.chunksNavigator.GetChunk(z.chunkCoord),
                        z.detalization);
            }
        }

        private void BuildTexture(Chunk chunk, int detalization)
        {
            Vector2 pos = world.Model.CoordTransformer.ModelCoordToGlobal(
                chunk.leftDown);
            int sizeInLayer = world.Model.detalizationAccessor.GetSizeInLayer(chunk, detalization);
            Texture2D texture = new Texture2D(sizeInLayer, sizeInLayer);
            for (int y = 0; y < sizeInLayer; y++)
                for (int x = 0; x < sizeInLayer; x++)
                {
                    Color data = valueGetter.GetValue(
                        world.Model.detalizationAccessor.GetBaseCoord(
                            new IntCoord(x, y), chunk, detalization),
                        (IPointsStorage)typeof(WorldModel).GetField(mapName).GetValue(world.Model));
                    texture.SetPixel(x, y, data);
                }
            texture.Apply();

            GameObject z = new GameObject(chunk.chunkCoord.ToString(),
                typeof(MeshFilter),
                typeof(MeshRenderer));
            z.transform.SetParent(transform);
            Mesh mesh = new Mesh();
            Vector2 v0 = world.Model.CoordTransformer.ModelCoordToGlobal(chunk.leftDown);
            Vector2 v1 = world.Model.CoordTransformer.ModelCoordToGlobal(chunk.LeftTop);
            Vector2 v2 = world.Model.CoordTransformer.ModelCoordToGlobal(chunk.RightTop);
            Vector2 v3 = world.Model.CoordTransformer.ModelCoordToGlobal(chunk.RightDown);

            float chunkSize = world.Model.CoordTransformer.ModelDistToGlobal(chunk.Size - 1);
            Vector2 offset = new Vector2(-chunkSize / 2.0f, -chunkSize / 2.0f);
            mesh.vertices = new Vector3[]
            {
                new Vector3(v0.x + offset.x, 0, v0.y + offset.y),
                new Vector3(v1.x + offset.x, 0, v1.y + offset.y),
                new Vector3(v2.x + offset.x, 0, v2.y + offset.y),
                new Vector3(v3.x + offset.x, 0, v3.y + offset.y),
            };
            mesh.triangles = new int[]
            {
                0, 1, 2, 2, 3, 0
            };
            mesh.uv = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0)
            };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.Optimize();
            z.GetComponent<MeshFilter>().mesh = mesh;
            z.GetComponent<MeshRenderer>().material.mainTexture = texture;
        }

        public void Clear()
        {
        }
    }
}
