using System;
using System.Collections.Generic;
using UnityEngine;
using World.DataStructures.ChunksGrid;

namespace World.Render.RenderedChunks
{
    public abstract class RenderedChunk
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
}
