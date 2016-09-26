using UnityEditor;
using UnityEngine;
using World.MyDebug.Storage.Texture;

namespace World.MyDebug.Editor
{
    [CustomEditor(typeof(StorageTextureOutput))]
    public class StorageTextureOutputEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            StorageTextureOutput t = (StorageTextureOutput)target;
            if (t.valueGetter == null)
            {
                GUI.enabled = false;
                GUILayout.Label("Set 'valueGetter' first");
            }
            if (t.world == null)
            {
                GUI.enabled = false;
                GUILayout.Label("Set 'world' first");
            }
            if (!Application.isPlaying)
            {
                GUI.enabled = false;
                GUILayout.Label("Start game first");
            }
            if (GUILayout.Button("Build map"))
            {
                t.BuildTexture(t.world.CurChunk.chunkCoord, t.world.settings.detalization);
            }
            GUI.enabled = true;
            if (GUILayout.Button("Clear"))
            {
                t.Clear();
            }
        }
    }
}
