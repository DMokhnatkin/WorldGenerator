using Map.MapModels.Points;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map.Debugger.MapModel
{
    public class EdgeDrawArgs : EventArgs
    {
        public IMapPoint Pt1 { get; private set; }
        public Vector3 Pos1 { get; private set; }

        public IMapPoint Pt2 { get; private set; }
        public Vector3 Pos2 { get; private set; }
        public bool IsHandled { get; set; }

        public EdgeDrawArgs(IMapPoint pt1, IMapPoint pt2, Vector3 pos1, Vector3 pos2)
        {
            Pt1 = pt1;
            Pos1 = pos1;
            Pt2 = pt2;
            Pos2 = pos2;
            IsHandled = false;
        }
    }
}
