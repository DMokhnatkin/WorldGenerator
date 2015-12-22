using System;
using System.Collections.Generic;
using UnityEngine;
using World.Instance;
using World.Model;
using World.Model.PointCollections;

namespace World.Render
{
    /// <summary>
    /// This script renders world 
    /// </summary>
    [RequireComponent(typeof(RenderSettings))]
    public class WorldRender : MonoBehaviour
    {
        /// <summary>
        /// World to render
        /// </summary>
        public WorldInstance World;

        /// <summary>
        /// Rendered object
        /// </summary>
        private GameObject renderedObject;

        private RenderSettings renderSettings;

        void Start()
        {
            if (World != null)
                World.PlayerMovedInModel += PlayerMovedInModel;
            renderSettings = GetComponent<RenderSettings>();
            renderedObject = new GameObject("worldObject", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
            PlayerMovedInModel(null, World.CurModelCoord);
        }

        private void PlayerMovedInModel(ModelCoord lastPos, ModelCoord newPos)
        {
            var points = PointNavigation.GetAround(World.Model.MaxDetalizationLayer, 
                World.Model[newPos], 
                World.Model.CoordTransformer.GlobalDistToModel(renderSettings.radius, World.Model.MaxDetalizationLayer));
            var meshFilter = renderedObject.GetComponent<MeshFilter>();
            var meshRender = renderedObject.GetComponent<MeshRenderer>();
            var meshCollider = renderedObject.GetComponent<MeshCollider>();
            meshFilter.mesh.Clear();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            foreach (ModelPointTriangle triangle in points.GetTriangles())
            {
                Vector2 pt1Coord = World.Model.CoordTransformer.ModelCoordToGlobal(triangle.v1.NormalCoord);
                Vector2 pt2Coord = World.Model.CoordTransformer.ModelCoordToGlobal(triangle.v2.NormalCoord);
                Vector2 pt3Coord = World.Model.CoordTransformer.ModelCoordToGlobal(triangle.v3.NormalCoord);
                vertices.Add(new Vector3(pt1Coord.x, triangle.v1.Data.height * renderSettings.worldHeight, pt1Coord.y));
                triangles.Add(vertices.Count - 1);
                vertices.Add(new Vector3(pt2Coord.x, triangle.v2.Data.height * renderSettings.worldHeight, pt2Coord.y));
                triangles.Add(vertices.Count - 1);
                vertices.Add(new Vector3(pt3Coord.x, triangle.v3.Data.height * renderSettings.worldHeight, pt3Coord.y));
                triangles.Add(vertices.Count - 1);
            }
            meshFilter.mesh.vertices = vertices.ToArray();
            meshFilter.mesh.triangles = triangles.ToArray();
            meshFilter.mesh.RecalculateNormals();
            meshFilter.mesh.RecalculateNormals();
            meshFilter.mesh.Optimize();
            meshCollider.sharedMesh = meshFilter.sharedMesh;
            meshRender.materials[0].mainTexture = renderSettings.baseTexture;
        }
    }
}
