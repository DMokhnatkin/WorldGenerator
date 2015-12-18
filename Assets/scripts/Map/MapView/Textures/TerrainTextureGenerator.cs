using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Map.MapModels;
using Map.Generator.Geometry;
using Map.MapModels.Points;

namespace Map.MapView.Textures
{
    public class TerrainTextureGenerator
    {
        public static void GenerateTerrainTexture(IMapPoint[,] points, TerrainData terrData, MapTextureSettings sett)
        {
            terrData.splatPrototypes = sett.GetSplatPrototypes();
            terrData.alphamapResolution = points.GetLength(0);
            float[,,] alphaMap = terrData.GetAlphamaps(0, 0,
                terrData.alphamapWidth, terrData.alphamapHeight);
            for (int i = 0; i < terrData.alphamapHeight; i++)
                for (int j = 0; j < terrData.alphamapWidth; j++)
                {
                    float? top;
                    if (i > 0)
                        top = points[i - 1, j].Height;
                    else
                        top = null;

                    float? right;
                    if (j < points.GetLength(1) - 1)
                        right = points[i, j + 1].Height;
                    else
                        right = null;

                    float? down;
                    if (i < points.GetLength(1) - 1)
                        down = points[i + 1, j].Height;
                    else
                        down = null;

                    float? left;
                    if (j > 0)
                        left = points[i, j - 1].Height;
                    else
                        left = null;

                    float slope = Slope.CalcMaxSlope(points[i, j].Height,
                        top, right, down, left);
                    int slopeLevelId = sett.GetSlopeLevel(slope);
                    for (int k = 0; k < terrData.splatPrototypes.Length; k++)
                    {
                        // Terrain i inversed!
                        if (k != slopeLevelId)
                            alphaMap[terrData.alphamapHeight - i - 1, j, k] = 0;
                        else
                        {
                            alphaMap[terrData.alphamapHeight - i - 1, j, slopeLevelId] = 1;
                        }
                    }
                }
            terrData.SetAlphamaps(0, 0, alphaMap);
        }
    }
}
