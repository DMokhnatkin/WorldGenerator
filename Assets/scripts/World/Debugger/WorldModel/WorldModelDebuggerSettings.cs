using UnityEngine;
using System;

namespace World.Debugger.WorldModel
{
    [Serializable]
    public class WorldModelDebuggerSettings
    {
        public Color chunkColor = Color.white;
        public int radius = 1;
        public bool drawChunks = false;
        public int chunksDrawRadius = 2;
        public bool drawPointCoords = false;
    }
}
