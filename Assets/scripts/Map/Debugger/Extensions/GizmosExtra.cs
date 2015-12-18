using UnityEngine;

namespace Map.Debugger.Extensions
{
    public static class GizmosExtra
    {
        public static void DrawArrowEnd(Vector3 pos1, Vector3 pos2, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Vector3 direction = pos2 - pos1;
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back;
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back;
            Vector3 up = Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back;
            Vector3 down = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back;
            Gizmos.DrawRay(pos2, right * arrowHeadLength);
            Gizmos.DrawRay(pos2, left * arrowHeadLength);
            Gizmos.DrawRay(pos2, up * arrowHeadLength);
            Gizmos.DrawRay(pos2, down * arrowHeadLength);
        }

        public static void DrawArrow(Vector3 pos1, Vector3 pos2, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.DrawLine(pos1, pos2);
            DrawArrowEnd(pos1, pos2, arrowHeadLength, arrowHeadAngle);
        }
    }
}
