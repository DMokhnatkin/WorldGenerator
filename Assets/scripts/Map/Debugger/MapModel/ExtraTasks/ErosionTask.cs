using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Map.Debugger.Extensions;
using UnityEditor;

namespace Map.Debugger.MapModel.ExtraTasks
{
    [RequireComponent(typeof(DebugWorldModel))]
    [AddComponentMenu("Map/Debug/Model/ErosionTask")]
    public class ErosionTask : MonoBehaviour
    {
        DebugWorldModel debugger;

        public bool drawFloodDirection = true;

        public bool printFloodVal = true;

        void DrawPoint(PointDrawArgs args)
        {
#if !DEBUG
            if (printFloodVal)
                Debug.LogError("It isn't debug mode, so flood values will not be rendered");
#else
            if (printFloodVal)
                args.Print.Add(new KeyValuePair<string, object>("fld", args.Sender.NatureConf.flood.ToString("0.###")));
#endif
        }

        void DrawEdge(EdgeDrawArgs args)
        {
#if !DEBUG
            if (drawFloodDirection)
                Debug.LogError("It isn't debug mode, so flood directions will not be rendered");
#else
            if (drawFloodDirection)
            {
                Gizmos.color = Color.white;
                // Get flood direction
                Vector3 p1, p2;
                if (args.Pt1.NatureConf.falledTo.Contains(args.Pt2))
                {
                    p1 = args.Pos1;
                    p2 = args.Pos2;
                }
                else
                {
                    p1 = args.Pos2;
                    p2 = args.Pos1;
                }
                GizmosExtra.DrawArrowEnd(p1,
                    p1 + (p2 - p1) * 0.5f,
                    HandleUtility.GetHandleSize(p2) * 0.25f,
                    15.0f);
            }
#endif
        }

        void OnEnable()
        {
            debugger = GetComponent<DebugWorldModel>();
            debugger.DrawPoint += DrawPoint;
            debugger.DrawEdge += DrawEdge;
        }

        void OnDisable()
        {
            debugger.DrawPoint -= DrawPoint;
            debugger.DrawEdge -= DrawEdge;
        }
    }
}
