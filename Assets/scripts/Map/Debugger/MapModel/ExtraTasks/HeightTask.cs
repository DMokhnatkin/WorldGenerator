using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Map.Debugger.MapModel.ExtraTasks
{
    [AddComponentMenu("Map/Debug/Model/HeightTask")]
    public class HeightTask : MonoBehaviour
    {
        DebugWorldModel debugger;

        public bool printHeight = true;

        void DrawPoint(PointDrawArgs args)
        {
#if !DEBUG
            if (drawHeight)
                Debug.LogError("It isn't debug mode, so height values will not be rendered");
#else
            if (printHeight)
                args.Print.Add(new KeyValuePair<string, object>("hgt", args.Sender.Height.ToString("0.###")));
#endif
        }

        void DrawEdge(EdgeDrawArgs args)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(args.Pos1, args.Pos2);
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
