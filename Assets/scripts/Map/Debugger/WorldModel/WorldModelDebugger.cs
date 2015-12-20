using UnityEngine;
using Map.World;
using System.Collections.Generic;
using Map.MapModels.WorldModel;

namespace Map.Debugger.WorldModel
{
    public class WorldModelDebugger : MonoBehaviour
    {
        public WorldInstance worldInstance;

        public int detalization = 1;

        public float radius = 5.0f;

        public const float ptRadius = 0.1f;
        public Color ptColor = Color.green; 

        void Start()
        {
            detalization = worldInstance.model.GetMaxDetalizationLayer().Detalization;
        }

        void DrawPoint(Vector3 pos)
        {
            Gizmos.color = ptColor;
            Gizmos.DrawSphere(pos, ptRadius);
        }

        void OnDrawGizmos()
        {
            if (worldInstance == null)
                return;
            if (!Application.isPlaying)
                return;
            List<WorldPoint> pts = PointNavigation.GetAround(worldInstance.model,
                worldInstance.model.GetLayer(detalization), 
                worldInstance.CurModelPoint,
                radius);
            foreach (WorldPoint pt in pts)
            {
                Vector2 pos = worldInstance.ModelCoordToGlobalCoord.ModelCoordToGlobal(pt.NormalCoord);
                Vector3 pos3 = new Vector3(pos.x, 0, pos.y);
                DrawPoint(pos3);
            }
        }
    }
}
