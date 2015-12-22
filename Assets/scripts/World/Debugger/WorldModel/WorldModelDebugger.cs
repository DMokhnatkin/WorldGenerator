using UnityEngine;
using World.Instance;
using World.Model;
using World.Model.PointCollections;
using UnityEditor;

namespace World.Debugger.WorldModel
{
    [RequireComponent(typeof(WorldModelDebuggerSettings))]
    public class WorldModelDebugger : MonoBehaviour
    {
        public WorldInstance worldInstance;

        public int detalization = 1;

        public float radius = 5.0f;

        public const float ptRadius = 0.05f;
        public Color ptColor = Color.green;

        private WorldModelDebuggerSettings settings;

        void Start()
        {
            detalization = worldInstance.Model.MaxDetalizationLayer.Detalization;
            settings = GetComponent<WorldModelDebuggerSettings>();
        }

        void DrawPoint(Vector3 pos)
        {
            Gizmos.color = ptColor;
            Gizmos.DrawSphere(pos, HandleUtility.GetHandleSize(pos) * ptRadius);
        }

        void OnDrawGizmos()
        {
            if (!enabled)
                return;
            if (worldInstance == null)
                return;
            if (!Application.isPlaying)
                return;
            var pts = PointNavigation.GetAround(worldInstance.Model.GetLayer(detalization), 
                worldInstance.CurModelPoint,
                worldInstance.Model.CoordTransformer.GlobalDistToModel(radius, worldInstance.Model.GetLayer(detalization)));
            foreach (ModelPoint pt in pts)
            {
                Vector2 pos = worldInstance.Model.CoordTransformer.ModelCoordToGlobal(pt.NormalCoord);
                Vector3 pos3 = new Vector3(pos.x, pt.Data.height * settings.height, pos.y);
                DrawPoint(pos3);
            }
        }
    }
}
