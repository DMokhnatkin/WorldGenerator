using System;
using System.Collections.Generic;
using UnityEngine;
using World.DataStructures.ChunksGrid;

namespace World.Render.RenderedChunks
{
    /// <summary>
    /// For this chunk was used mesh to render.
    /// </summary>
    public class MeshRenderedChunk : RenderedChunk
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
        { }
    }
}
