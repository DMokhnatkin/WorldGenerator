using UnityEngine;
using System;
using World.Model;

namespace World.Render
{
    [Serializable]
    public class RenderSettings
    {
        /// <summary>
        /// Size(in model coords of max detalization layer) of one chunk 
        /// </summary>
        public float worldHeight = 500f;
        public Texture2D baseTexture;
    }
}
