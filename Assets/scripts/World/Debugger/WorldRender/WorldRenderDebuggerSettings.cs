using System;
using UnityEngine;

namespace World.Debugger.WorldRender
{
    [Serializable]
    public class WorldRenderDebuggerSettings
    {
        public bool drawChunkBorders = true;
        public Color chunkBorderColor = Color.white;
        public bool drawRenderFrame = true;
        public Color renderFrameColor = Color.red;
    }
}
