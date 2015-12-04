using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Map.Generator.MapModels;
using Map.Generator.World;
using Map.Generator.MapView;

namespace Map.Generator.Debugger
{
    [RequireComponent(typeof(Landscape))]
    public class DebugWorldModel : MonoBehaviour
    {
        Landscape land;
        LandscapeSettings sett;

        public float verticalGridOffset = 1.0f;

        public int depthLayer = -1;

        public int neighborsRadius = -1;

        void Start()
        {
            land = GetComponent<Landscape>();
            sett = GetComponent<LandscapeSettings>();
        }

        void DrawArea(Area area, int depth, Vector3 leftTop)
        {
            int resolution = (int)Math.Pow(2, depth);
            float edgeLentgh = (int)sett.chunkSize / (float)resolution;

            MapPoint[,] map = area.ToArray(depth);

            for (int i = 0; i < map.GetLength(0) - 1; i++)
                for (int j = 0; j < map.GetLength(1) - 1; j++)
                {
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
                if (land == null)
                    land = GetComponent<Landscape>();
                if (sett == null)
                    sett = GetComponent<LandscapeSettings>();

                if (land.CurArea == null)
                    return;
                if (land.mapViewer.GetViewInfo(land.CurArea) == null)
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
                        Vector3 leftTop = land.mapViewer.GetViewInfo(land.CurArea).LeftDownPos +
                            new Vector3((j - neighborsRadius) * (int)sett.chunkSize, 0, (neighborsRadius - i + 1) * (int)sett.chunkSize);
                        DrawArea(z[i, j], z[i, j].CalcDepth(), leftTop);
                    }
            }
        }
    }
}
