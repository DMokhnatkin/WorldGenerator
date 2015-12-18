using System;
using UnityEngine;
using Map.MapModels.Areas;
using Map.World;
using Map.MapModels.Extensions;
using UnityEditor;
using Map.MapModels.Points;
using Map.MapModels.Navigation.Points;
using System.Collections.Generic;
using Map.Debugger.Extensions;

namespace Map.Debugger.MapModel
{
    [AddComponentMenu("Map/Debug/Model/Debugger")]
    public class DebugWorldModel : MonoBehaviour
    {
        public Landscape land;

        public float verticalGridOffset = 1.0f;

        public int depthLayer = -1;

        public int neighborsRadius = -1;

        public float pointRadius = 0.04f;

        public GUIStyle pointInfoStyle = new GUIStyle();

        private readonly Color pointColor = Color.white;

        LandscapeSettings sett;

        public bool drawPoints = false;

        public bool drawEdges = false;

        public event Action<PointDrawArgs> DrawPoint;

        public event Action<EdgeDrawArgs> DrawEdge;

        protected virtual void OnDrawPoint(PointDrawArgs args)
        {
            var drawPointEvent = DrawPoint;
            if (drawPointEvent != null)
                DrawPoint(args);
        }

        protected virtual void OnDrawEdge(EdgeDrawArgs args)
        {
            var drawEdgeEvent = DrawEdge;
            if (drawEdgeEvent != null)
                DrawEdge(args);
        }

        void _DrawPoint(IMapPoint pt, Vector3 pos)
        {
            PointDrawArgs args = new PointDrawArgs(pt, pos);
            OnDrawPoint(args);
            if (!args.IsHandled)
            {
                Gizmos.color = pointColor;
                Gizmos.DrawSphere(pos, HandleUtility.GetHandleSize(pos) * pointRadius);
                string text = "";
                foreach (KeyValuePair<string, object> z in args.Print)
                {
                    text += z.Key + "=" + z.Value.ToString() + Environment.NewLine;
                }
                Handles.Label(pos, new GUIContent(text), pointInfoStyle);
            }
        }

        void _DrawEdge(IMapPoint p1, IMapPoint p2, Vector3 pos1, Vector3 pos2)
        {
            OnDrawEdge(new EdgeDrawArgs(p1, p2, pos1, pos2));
        }

        void DrawArea(Area area, int depth, Vector3 leftTop)
        {
            int resolution = (int)Math.Pow(2, depth);
            float edgeLentgh = (int)sett.chunkSize / (float)resolution;

            MapPointInLayer[,] map = area.UnwrapPoints(depth);

            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (drawPoints)
                    {
                        Vector3 pos =
                            leftTop + new Vector3(j * edgeLentgh, map[i, j].Height * sett.height + verticalGridOffset, -i * edgeLentgh);
                        _DrawPoint(map[i, j], pos);
                    }
                    if (drawEdges)
                    {
                        if (j + 1 < map.GetLength(1))
                        {
                            _DrawEdge(map[i, j],
                                map[i, j + 1],
                                leftTop + new Vector3(j * edgeLentgh, map[i, j].Height * sett.height + verticalGridOffset, -i * edgeLentgh),
                                leftTop + new Vector3((j + 1) * edgeLentgh, map[i, j + 1].Height * sett.height + verticalGridOffset, -i * edgeLentgh));
                        }
                        if (i + 1 < map.GetLength(0))
                        {
                            _DrawEdge(map[i, j],
                                map[i + 1, j],
                                leftTop + new Vector3(j * edgeLentgh, map[i, j].Height * sett.height + verticalGridOffset, -i * edgeLentgh),
                                leftTop + new Vector3(j * edgeLentgh, map[i + 1, j].Height * sett.height + verticalGridOffset, -(i + 1) * edgeLentgh));
                        }
                    }
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
