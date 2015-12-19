using UnityEditor;
using UnityEngine;

namespace Map.Debugger.MapModel.SeismicMap
{
    [CustomEditor(typeof(SeismicMapDebugger))]
    public class SeismicMapDebuggerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SeismicMapDebugger deb = (SeismicMapDebugger)target;
            if (GUILayout.Button("UpdateTexture"))
            {
                deb.UpdateTexture();
            }
        }
    }
}
