using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using World.DataStructures.ChunksGrid;

namespace World.Render.RenderedChunks
{
    /// <summary>
    /// For this chunk was used terrain to render.
    /// </summary>
    public class TerrainRenderedChunk : RenderedChunk
    {
        /// <summary>
        /// Associated terrain component
        /// </summary>
        public Terrain TerrainComponent { get { return GameObject.GetComponent<Terrain>(); } }

        public TerrainRenderedChunk(GameObject gameObject, Chunk chunk, int detalization) :
            base(gameObject, chunk, detalization)
        { }
    }
}
