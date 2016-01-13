using UnityEngine;
using World.Instance;
using World.Model;
using UnityEditor;
using World.Model.Frames;
using World.Model.Chunks;

namespace World.Debugger.WorldModel
{
    public class WorldModelDebugger : MonoBehaviour
    {
        public WorldInstance worldInstance;

        public int detalization = 1;

        public const float ptRadius = 0.05f;
        public Color ptColor = Color.red;

        /// <summary>
        /// Settings for worldModelDebugger
        /// </summary>
        public WorldModelDebuggerSettings settings = new WorldModelDebuggerSettings();

        void Start()
        {
            detalization = worldInstance.Model.MaxDetalizationLayer.Id;
        }

        void DrawPoint(Vector3 pos, ModelCoord coord)
        {
            Gizmos.color = ptColor;
            Gizmos.DrawSphere(pos, HandleUtility.GetHandleSize(pos) * ptRadius);
            if (settings.drawPointCoords)
            {
                Handles.Label(pos, coord.ToString());
            }
        }

        void DrawChunk(ModelChunk chunk)
        {
            Gizmos.color = settings.chunkColor;
            Vector2 chunkPos = worldInstance.Model.CoordTransformer.ModelCoordToGlobal(chunk.Frame.Center);
            float chunkSize = worldInstance.Model.CoordTransformer.ModelDistToGlobal(worldInstance.Model.ChunksGrid.ChunkSize);
            Gizmos.DrawWireCube(new Vector3(chunkPos.x, worldInstance.settings.height / 2.0f, chunkPos.y), 
                new Vector3(chunkSize, worldInstance.settings.height, chunkSize));
        }

        void OnDrawGizmos()
        {
            if (!enabled)
                return;
            if (worldInstance == null)
                return;
            if (!Application.isPlaying)
                return;
            var frame = FrameBuilder.GetSquareAround(worldInstance.CurModelCoord,
                settings.radius);
            foreach (ModelCoord z in frame.GetCoords())
            {
                if (!worldInstance.Model.Contains(z))
                    continue;
                Vector2 pos = worldInstance.Model.CoordTransformer.ModelCoordToGlobal(z);
                Vector3 pos3 = new Vector3(pos.x, worldInstance.Model[z].Data.Height * worldInstance.settings.height, pos.y);
                DrawPoint(pos3, z);
            }
            if (settings.drawChunks)
            {
                ModelChunk chunk = worldInstance.CurModelChunk;
                SquareFrame chunksFrame = FrameBuilder.GetSquareAround(chunk.Coord, settings.chunksDrawRadius);
                foreach (ModelCoord z in chunksFrame.GetCoords())
                {
                    DrawChunk(worldInstance.Model.ChunksGrid.GetChunk(z));
                }
            }
        }
    }
}
