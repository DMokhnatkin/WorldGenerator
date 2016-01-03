using UnityEngine;
using System;

namespace World.Debugger.WorldModel
{
    [Serializable]
    public class WorldModelDebuggerSettings
    {
        public int radius = 1;
        public float height = 500;
        public bool drawPointCoords = false;
    }
}
