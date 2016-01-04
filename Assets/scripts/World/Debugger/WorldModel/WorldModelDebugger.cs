using UnityEngine;
using World.Instance;
using World.Model;
using World.Model.PointCollections;
using UnityEditor;

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

        void OnDrawGizmos()
        {
            if (!enabled)
                return;
            if (worldInstance == null)
                return;
            if (!Application.isPlaying)
                return;
            var frame = PointNavigation.GetAround(worldInstance.CurModelCoord,
                settings.radius);
            foreach (ModelCoord z in frame.GetCoords())
            {
                if (!worldInstance.Model.Contains(z))
                    continue;
                Vector2 pos = worldInstance.Model.CoordTransformer.ModelCoordToGlobal(z);
                Vector3 pos3 = new Vector3(pos.x, worldInstance.Model[z].Data.Height * settings.height, pos.y);
                DrawPoint(pos3, z);
            }
        }
    }
}
