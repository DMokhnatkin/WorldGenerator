using System;
using System.Collections.Generic;
using Map.MapModels;
using Map.Generator.Algorithms;
using UnityEngine;
using System.Collections;
using System.Linq;
using Map.MapModels.Areas;

namespace Map.World
{
    public class Generator
    {
        DiamondSquare sq;

        public Generator(LandscapeSettings sett)
        {
            sq = new DiamondSquare((byte)(sett.depths.Length - 1), (float)sett.chunkSize);
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
                for (int y = maxR - r; y < maxR + r + 1; y++)
                {
                    for (int z = maxR - r; z < maxR + r + 1; z++)
                    {
                        if (areas[y, z] != null)
                            sq.ExtendResolution(areas[y, z], (byte)i);
                    }
                }
                r -= sett.depths[i];
            }
        }
    }
}
