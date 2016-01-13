using System;
using System.Collections.Generic;
using UnityEngine;
using World.Model.Frames;
using World.Model;

namespace World.Render
{
    public class RenderChunk
    {
        public GameObject ChunkObject { get; private set; }

        public MeshRenderer MeshRender { get; private set; }

        public MeshFilter MeshFilter { get; private set; }

        public MeshCollider MeshCollider { get; private set; }

        public SquareFrame Frame { get; private set; }

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        public RenderChunk()
        {
            ChunkObject = new GameObject("chunk", 
                typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
            MeshRender = ChunkObject.GetComponent<MeshRenderer>();
            MeshFilter = ChunkObject.GetComponent<MeshFilter>();
            MeshCollider = ChunkObject.GetComponent<MeshCollider>();
        }

        private void ApplyMesh()
        {
            MeshFilter.mesh.Clear();
            MeshFilter.mesh.vertices = vertices.ToArray();
            MeshFilter.mesh.triangles = triangles.ToArray();
            MeshFilter.mesh.RecalculateNormals();
            MeshFilter.mesh.Optimize();
            MeshCollider.sharedMesh = MeshFilter.sharedMesh;

            vertices.Clear();
            triangles.Clear();
        }

        public void ApplyTexture(Texture2D baseTexture)
        {
            MeshRender.materials[0].mainTexture = baseTexture;
        }

        /// <summary>
        /// Create mesh for specifed frame in model
        /// </summary>
        public void Initialize(SquareFrame frame, WorldModel model, float maxHeight)
        {
            Frame = frame;
            for (int y = frame.LeftDown.y; y < frame.LeftDown.y + frame.Size; y++)
                for (int x = frame.LeftDown.x; x < frame.LeftDown.x + frame.Size; x++)
                {
                    ModelCoord pt1 = new ModelCoord(x, y);
                    if (!model.Contains(pt1))
                        throw new ArgumentException("Can't create mesh for chunk because model point wasn't created");
                    Vector2 pt1Pos = model.CoordTransformer.ModelCoordToGlobal(pt1);
                    vertices.Add(new Vector3(pt1Pos.x, model[pt1].Data.Height * maxHeight, pt1Pos.y));
                    int pt1Id = (x - frame.LeftDown.x) + (y - frame.LeftDown.y) * frame.Size;

                    ModelCoord pt2 = new ModelCoord(x, y - 1);
                    ModelCoord pt3 = new ModelCoord(x - 1, y - 1);
                    ModelCoord pt4 = new ModelCoord(x - 1, y);

                    if (frame.Contains(pt2) && frame.Contains(pt3) &&
                        model.Contains(pt2) && model.Contains(pt3))
                    {
                        Vector2 pt2Pos = model.CoordTransformer.ModelCoordToGlobal(pt2);
                        Vector2 pt3Pos = model.CoordTransformer.ModelCoordToGlobal(pt3);
                        int pt2Id = pt1Id - frame.Size;
                        int pt3Id = pt1Id - frame.Size - 1;
                        triangles.Add(pt1Id);
                        triangles.Add(pt2Id);
                        triangles.Add(pt3Id);
                    }
                    if (frame.Contains(pt3) && frame.Contains(pt4) &&
                        model.Contains(pt3) && model.Contains(pt4))
                    {
                        Vector2 pt3Pos = model.CoordTransformer.ModelCoordToGlobal(pt3);
                        Vector2 pt4Pos = model.CoordTransformer.ModelCoordToGlobal(pt4);
                        int pt3Id = pt1Id - frame.Size - 1;
                        int pt4Id = pt1Id - 1;
                        triangles.Add(pt1Id);
                        triangles.Add(pt3Id);
                        triangles.Add(pt4Id);
                    }
                }
            ApplyMesh();
        }
    }
}
