using UnityEngine;
using Map.MapModels.Seismic;
using Map.World;
using System.Collections.Generic;
using Map.MapModels.Common;
using Map.Generator.Algorithms.PerlinNoise;

namespace Map.Debugger.MapModel.SeismicMap
{
    public class SeismicMapDebugger : MonoBehaviour
    {
        public Landscape land;

        [HideInInspector]
        public Texture2D texture = new Texture2D(1000, 1000);

        public void UpdateTexture()
        {
            Perlin2D noise = new Perlin2D(new System.Random().Next());
            for (int x = 0; x < 1000; x++)
                for (int y = 0; y < 1000; y++)
                {
                    float curNoise = noise.Noise(x / 500.0f + 0.01f, y / 500.0f + 0.01f, 3);
                    texture.SetPixel(x, y, new Color(255 * curNoise, 0, 0));
                }
            texture.Apply();
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawGUITexture(new Rect(0, 0, 1000, 1000), texture);
        }
    }
}
