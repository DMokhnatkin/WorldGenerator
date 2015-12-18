using System;
using Map.MapModels.Points;
using UnityEngine;
using System.Collections.Generic;

namespace Map.Debugger.MapModel
{
    public class PointDrawArgs : EventArgs
    {
        public IMapPoint Sender { get; private set; }
        public Vector3 Position { get; private set; }
        public bool IsHandled { get; set; }
        public List<KeyValuePair<string, object>> Print { get; private set; }

        public PointDrawArgs(IMapPoint sender, Vector3 pos)
        {
            Sender = sender;
            Position = pos;
            IsHandled = false;
            Print = new List<KeyValuePair<string, object>>();
        }
    }
}
