using System;
using System.Collections.Generic;
using Map.Generator.MapModels;
using Map.Generator.Algorithms;
using UnityEngine;
using System.Collections;
using System.Linq;

namespace Map.Generator.World
{
    public class Generator
    {
        /// <summary>
        /// Is we generate smth now
        /// (needs for coroutines)
        /// </summary>
        bool _generating = false;

        DiamondSquare sq;

        public Generator(LandscapeSettings sett)
        {
            sq = new DiamondSquare((byte)(sett.depths.Length - 1));
        }

        /// <summary>
        /// Generate areas around area in depth.
        /// </summary>
        /// <param name="radius">Radius of areas to generate in depth</param>
        public void Generate(Area area, byte depth, int radius = 0)
        {
            area.CreateAreasAround(depth + radius);
            for (int i = 1; i <= depth; i++)
            {
                Area[,] areas = area.GetAreasAround(depth + radius - i);
                foreach (Area t in areas)
                {
                    sq.ExtendResolution(t, (byte)i);
                }
            }
        }

        public void GenerateAround(Area area, LandscapeSettings sett)
        {
            int maxR = sett.depths.Sum((x) => { return x; });
            int r = maxR;
            area.CreateAreasAround(maxR);
            Area[,] areas = area.GetAreasAround(maxR);

            for (int i = 1; i < sett.depths.Length; i++)
            {
                // Get areas with cur radius
                for (int y = maxR - r; y < maxR + r; y++)
                {
                    for (int z = maxR - r; z < maxR + r; z++)
                    {
                        sq.ExtendResolution(areas[y, z], (byte)i);
                    }
                }
                r -= sett.depths[i];
            }
        }
    }
}
