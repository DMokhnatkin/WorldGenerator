using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Map.MapView.Textures
{
    [Serializable]
    public class SlopeLevel
    {
        public float maxSlope = 0;
        public Texture2D texture;
        public Texture2D normalMap;
    }

    public class MapTextureSettings : MonoBehaviour
    {
        public float blendStrength = 0.1f;
        public List<SlopeLevel> slopes = new List<SlopeLevel>();

        /// <summary>
        /// Get id of slope level
        /// </summary>
        public int GetSlopeLevel(float slope)
        {
            for (int i = 0; i < slopes.Count; i++)
            {
                if (slope <= slopes[i].maxSlope)
                    return i;
            }
            return 0;
        } 

        /// <summary>
        /// Get array of splat prototypes
        /// </summary>
        /// <returns></returns>
        public SplatPrototype[] GetSplatPrototypes()
        {
            return slopes.Select(
                (x) => 
                {
                    return new SplatPrototype() { texture = x.texture, normalMap = x.normalMap };
                }).ToArray();
        }
    }
}
