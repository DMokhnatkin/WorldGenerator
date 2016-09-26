using System;
using System.Collections.Generic;
using UnityEngine;

namespace World.Render.Texture
{
    [Serializable]
    public class TextureRenderSettings
    {
        public Texture2D baseTexture;
        public Texture2D baseNormal;
        public Vector2 baseTile;

        public Texture2D waterMoodTexure;
        public Texture2D waterMoodNormal;
        public Vector2 waterMoodTile;
    }
}
