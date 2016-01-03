using UnityEngine;
using System;

namespace World.Render
{
    [Serializable]
    public class RenderSettings
    {
        /// <summary>
        /// Size(in model coords of max detalization layer) of one chunk 
        /// </summary>
        public int chunkSize = 128;
        public int chunksToRender = 2;
        public float worldHeight = 500f;
        public Texture2D baseTexture;
    }
}
