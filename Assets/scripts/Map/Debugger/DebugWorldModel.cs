﻿using System;
using UnityEngine;
using Map.MapModels.Areas;
using Map.World;
using Map.MapModels.Extensions;
using UnityEditor;
using Map.MapModels.Points;

namespace Map.Debugger
{
    public class DebugWorldModel : MonoBehaviour
    {
        public Landscape land;
        LandscapeSettings sett;

        public float verticalGridOffset = 1.0f;

        public int depthLayer = -1;

        public int neighborsRadius = -1;

        public bool drawPoints = false;

        public float pointRadius = 0.05f;

        void DrawArea(Area area, int depth, Vector3 leftTop)
        {
            int resolution = (int)Math.Pow(2, depth);
            float edgeLentgh = (int)sett.chunkSize / (float)resolution;

            MapPoint[,] map = area.UnwrapPoints(depth);

            for (int i = 0; i < map.GetLength(0) - 1; i++)
                for (int j = 0; j < map.GetLength(1) - 1; j++)
                {
                    if (drawPoints)
                    {
                        Vector3 pos =
                            leftTop + new Vector3(j * edgeLentgh, map[i, j].Height * sett.height + verticalGridOffset, -i * edgeLentgh);
                        Gizmos.DrawSphere(pos, HandleUtility.GetHandleSize(pos) * pointRadius);
                    }
                    // Horizontal
                    Gizmos.DrawLine(
                        leftTop + new Vector3(j * edgeLentgh, map[i, j].Height * sett.height + verticalGridOffset, -i * edgeLentgh),
                        leftTop + new Vector3((j + 1) * edgeLentgh, map[i, j + 1].Height * sett.height + verticalGridOffset, -i * edgeLentgh));
                    // Vertical
                    Gizmos.DrawLine(
                        leftTop + new Vector3(j * edgeLentgh, map[i, j].Height * sett.height + verticalGridOffset, -i * edgeLentgh),
                        leftTop + new Vector3(j * edgeLentgh, map[i + 1, j].Height * sett.height + verticalGridOffset, -(i + 1) * edgeLentgh));
                }
            // We didn't draw down edge of area
            for (int j = 0; j < map.GetLength(1) - 1; j++)
            {
                Gizmos.DrawLine(
                        leftTop + new Vector3(j * edgeLentgh, map[map.GetLength(0) - 1, j].Height * sett.height + verticalGridOffset, -(map.GetLength(0) - 1) * edgeLentgh),
                        leftTop + new Vector3((j + 1) * edgeLentgh, map[map.GetLength(0) - 1, j + 1].Height * sett.height + verticalGridOffset, -(map.GetLength(0) - 1) * edgeLentgh));
            }
            // We didn't draw right edge of area
            for (int i = 0; i < map.GetLength(0) - 1; i++)
            {
                Gizmos.DrawLine(
                        leftTop + new Vector3((map.GetLength(1) - 1) * edgeLentgh, map[i, map.GetLength(1) - 1].Height * sett.height + verticalGridOffset, -i * edgeLentgh),
                        leftTop + new Vector3((map.GetLength(1) - 1) * edgeLentgh, map[i + 1, map.GetLength(1) - 1].Height * sett.height + verticalGridOffset, -(i + 1) * edgeLentgh));
            }
        }

        void OnDrawGizmos()
        {
            if (enabled)
            {
                if (!Application.isPlaying)
                    return;
                if (land == null)
                    return;
                if (sett == null)
                    sett = land.Settings;

                if (land.CurArea == null)
                    return;

                int maxDepth = land.CurArea.CalcDepth();

                if (depthLayer == -1)
                    depthLayer = maxDepth;

                if (depthLayer > maxDepth)
                {
                    depthLayer = maxDepth;
                    Debug.LogWarning("depthLayer > depth of area. depthLayer was setted to depth");
                }

                if (neighborsRadius == -1)
                {
                    neighborsRadius = depthLayer;
                }

                Area[,] z = land.CurArea.GetAreasAround(neighborsRadius);
                for (int i = 0; i < z.GetLength(0); i++)
                    for (int j = 0; j < z.GetLength(1); j++)
                    {
                        if (z[i, j] == null)
                            continue;
                        Vector3 leftTop = new Vector3((land.CurCoord.x - 0.5f) * (int)sett.chunkSize, 0,
                            (land.CurCoord.y - 0.5f) * (int)sett.chunkSize) +
                            new Vector3((j - neighborsRadius) * (int)sett.chunkSize, 0, (neighborsRadius - i + 1) * (int)sett.chunkSize);
                        DrawArea(z[i, j], z[i, j].CalcDepth(), leftTop);
                    }
            }
        }
    }
}
