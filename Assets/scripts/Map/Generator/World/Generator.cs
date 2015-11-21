using System;
using System.Collections.Generic;
using Map.Generator.MapModels;
using Map.Generator.Algorithms;
using UnityEngine;
using System.Collections;

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
            sq = new DiamondSquare(sett.HightQualityDepth);
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
    }
}
