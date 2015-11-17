using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map.Generator.World
{
    public enum ChunkSize
    {
        Size32x32 = 32,
        Size64x64 = 64,
        Size128x128 = 128,
        Size256x256 = 256,
        Size512x512 = 512,
        Size1024x1024 = 1024
    }

    public class LandscapeSettings : MonoBehaviour
    {
        /// <summary>
        /// How much units must be in one heightmap pixel 
        /// </summary>
        public int unitPerPixel = 1;

        /// <summary>
        /// Size in unity units of each chunk
        /// </summary>
        public ChunkSize chunkSize = ChunkSize.Size32x32;

        public Texture2D baseTexture;

        /// <summary>
        /// Max height of terrain
        /// </summary>
        public float height = 500;

        public int LowQualityWidth = 5;
        public byte LowQualityDepth = 2;

        public int MediumQualityWidth = 4;
        public byte MediumQualityDepth = 4;

        public int HightQualityWidth = 3;
        public byte HightQualityDepth = 8;
    }
}
