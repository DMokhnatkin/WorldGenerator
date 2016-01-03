using System;
using UnityEngine;
using World.Model.Frames;

namespace World.Debugger.WorldRender
{
    public class WorldRenderDebugger : MonoBehaviour
    {
        public World.Render.WorldRender worldRender;

        /// <summary>
        /// Settings for WorldRenderDebugger
        /// </summary>
        public WorldRenderDebuggerSettings settings = new WorldRenderDebuggerSettings();

        void DrawBounds(Bounds bounds)
        {
            Gizmos.color = settings.chunkBorderColor;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }

        void DrawFrame(SquareFrame frame)
        {
            Vector2 center = worldRender.World.Model.CoordTransformer.ModelCoordToGlobal(
                frame.LeftDown.x + frame.Size / 2.0f, frame.LeftDown.y + frame.Size / 2.0f);
            Gizmos.color = settings.renderFrameColor;
            Gizmos.DrawWireCube(new Vector3(center.x, 0, center.y), new Vector3(frame.Size / 2.0f, 500, frame.Size / 2.0f));
        }

        void OnDrawGizmos()
        {
            if (worldRender == null)
                return;
            if (!Application.isPlaying)
                return;
            if (settings == null)
                settings = GetComponent<WorldRenderDebuggerSettings>();
            foreach (var z in worldRender.renderedChunks)
            {
                if (settings.drawChunkBorders)
                    DrawBounds(z.Value.MeshFilter.mesh.bounds);
                if (settings.drawRenderFrame)
                    DrawFrame(worldRender.CurRenderFrame);
            }
        }
    }
}
