using UnityEngine;
using System;
using World.Model;

namespace World.Render
{
    [Serializable]
    public class RenderSettings
    {
        public Texture2D baseTexture;
        public Vector2 tileSize = new Vector2(15, 15);
        public Texture2D baseTexture1;
        public Texture2D baseTexture1_normal;
    }
}
